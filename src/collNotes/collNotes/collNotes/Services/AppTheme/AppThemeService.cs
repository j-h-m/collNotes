using collNotes.ColorThemes;
using collNotes.Services.Settings;
using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static collNotes.Settings.CollNotesSettings;

namespace collNotes.Services.AppTheme
{
    public class AppThemeService : IAppThemeService
    {
        private ISettingService settingService;
        private IExceptionRecordService exceptionRecordService;
        public AppThemeService(ISettingService settingService,
            IExceptionRecordService exceptionRecordService)
        {
            this.settingService = settingService;
            this.exceptionRecordService = exceptionRecordService;
        }

        public async Task<ColorTheme> GetCurrentTheme()
        {
            var themeSetting = await settingService.GetByNameAsync(CollNotesSettings.ColorThemeKey);

            if (themeSetting is null)
            {
                return ColorTheme.Light_Default;
            }
            else
            {
                return GetByThemeName(themeSetting.SettingValue);
            }
        }

        public async Task<bool> SetAppTheme(ColorTheme colorTheme)
        {
            bool result = false;
            try
            {
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    // keep material resource dictionaries to add back
                    var dictsToKeep = mergedDictionaries.Where(dict => SubstringExistsInStringList(dict.Keys, "Material")).ToList();

                    mergedDictionaries.Clear();

                    switch (colorTheme)
                    {
                        case ColorTheme.Dark:
                            mergedDictionaries.Add(new Dark());
                            break;
                        case ColorTheme.ContrastDark:
                            mergedDictionaries.Add(new ContrastDark());
                            break;
                        case ColorTheme.ContrastLight:
                            mergedDictionaries.Add(new ContrastLight());
                            break;
                        case ColorTheme.Light_Default:
                        default:
                            mergedDictionaries.Add(new Light_Default());
                            break;
                    }

                    dictsToKeep.ForEach(keep =>
                    {
                        mergedDictionaries.Add(keep);
                    });
                    result = true;
                }
            }
            catch(Exception ex)
            {
                result = false;
                await this.exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }

        private bool SubstringExistsInStringList(ICollection<string> list, string substring)
        {
            foreach (var item in list)
            {
                if (item.ToLowerInvariant().Contains(substring.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
