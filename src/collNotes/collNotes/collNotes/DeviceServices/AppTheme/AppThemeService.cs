using collNotes.ColorThemes;
using collNotes.Domain.Models;
using collNotes.Services.Data;
using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static collNotes.Settings.CollNotesSettings;

namespace collNotes.DeviceServices.AppTheme
{
    public class AppThemeService : IAppThemeService
    {
        private ISettingService settingService = 
            DependencyService.Get<ISettingService>(DependencyFetchTarget.NewInstance);
        
        private IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public AppThemeService() { }

        public async Task<ColorTheme> GetSavedTheme()
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

        public async Task<bool> SaveAppTheme(ColorTheme colorTheme)
        {
            bool result = false;
            try
            {
                var themeSetting = await settingService.GetByNameAsync(CollNotesSettings.ColorThemeKey);

                if (themeSetting is null)
                {
                    result = await settingService.CreateAsync(new Setting()
                    {
                        SettingName = CollNotesSettings.ColorThemeKey,
                        SettingValue = GetByEnum(colorTheme)
                    });
                }
                else
                {
                    themeSetting.SettingValue = GetByEnum(colorTheme);
                    result = await settingService.UpdateAsync(themeSetting);
                }
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
                result = false;
            }
            return result;
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
                    dictsToKeep.ForEach(keep =>
                    {
                        mergedDictionaries.Add(keep);
                    });

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
