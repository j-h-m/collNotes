using collNotes.Data.Models;
using collNotes.Services.Permissions;
using collNotes.Services.Settings;
using collNotes.Settings;
using collNotes.Settings.AutoComplete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace collNotes.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private SettingService SettingService { get; set; }
        public readonly PermissionsService permissionsService;

        #region Binding Properties

        private string _CurrentCollectorName;
        public string CurrentCollectorName
        {
            get { return _CurrentCollectorName; }
            set
            {
                _CurrentCollectorName = value;
                CreateOrUpdateSetting(CollNotesSettings.PrimaryCollectorKey, value).ConfigureAwait(false);
                OnPropertyChanged(nameof(CurrentCollectorName));
            }
        }
        private int _CurrentCollectionCount;
        public int CurrentCollectionCount
        {
            get { return _CurrentCollectionCount; }
            set
            {
                if (value > 0)
                {
                    _CurrentCollectionCount = value;
                    CreateOrUpdateSetting(CollNotesSettings.CollectionCountKey, value.ToString()).ConfigureAwait(false);
                    OnPropertyChanged(nameof(CurrentCollectionCount));
                }
            }
        }
        private string _SelectedExportFormat;
        public string SelectedExportFormat
        {
            get { return _SelectedExportFormat ?? CollNotesSettings.ExportFormatDefault; }
            set
            {
                _SelectedExportFormat = value;
                CreateOrUpdateSetting(CollNotesSettings.ExportFormatKey, value).ConfigureAwait(false);
                OnPropertyChanged(nameof(SelectedExportFormat));
            }
        }
        private string _SelectedExportMethod;
        public string SelectedExportMethod
        {
            get { return _SelectedExportMethod ?? CollNotesSettings.ExportMethodDefault; }
            set
            {
                _SelectedExportMethod = value;
                CreateOrUpdateSetting(CollNotesSettings.ExportMethodKey, value).ConfigureAwait(false);
                OnPropertyChanged(nameof(SelectedExportMethod));
            }
        }
        private string _SelectedAutoCompleteType;
        public string SelectedAutoCompleteType
        {
            get { return _SelectedAutoCompleteType ?? CollNotesSettings.AutoCompleteDefault; }
            set
            {
                _SelectedAutoCompleteType = value;
                CreateOrUpdateSetting(CollNotesSettings.AutoCompleteTypeKey, value).ConfigureAwait(false);
                OnPropertyChanged(nameof(SelectedAutoCompleteType));
            }
        }
        private string _SelectedColorTheme;
        public string SelectedColorTheme
        {
            get { return _SelectedColorTheme ?? CollNotesSettings.ColorThemeDefault; }
            set
            {
                _SelectedColorTheme = value;
                CreateOrUpdateSetting(CollNotesSettings.ColorThemeKey, value).ConfigureAwait(false);
                OnPropertyChanged(nameof(SelectedColorTheme));
            }
        }
        private string _LastSavedDateTimeString;
        public string LastSavedDateTimeString
        {
            get { return _LastSavedDateTimeString; }
            set
            {
                _LastSavedDateTimeString = value;
                OnPropertyChanged(nameof(LastSavedDateTimeString));
            }
        }

        #endregion Binding Properties

        #region Choice Bindings

        public IEnumerable<string> ExportFormats
        {
            get
            {
                return CollNotesSettings.ExportFormats;
            }
        }

        public IEnumerable<string> ExportMethods
        {
            get
            {
                return CollNotesSettings.ExportMethods;
            }
        }

        public IEnumerable<string> AutoCompleteOptions
        {
            get
            {
                return CollNotesSettings.AutoCompleteOptions;
            }
        }

        public IEnumerable<string> ColorThemes
        {
            get
            {
                return CollNotesSettings.ColorThemes;
            }
        }

        #endregion Choice Bindings

        public SettingsViewModel()
        {
            SettingService = new SettingService(Context);
            permissionsService = new PermissionsService(Context);

            Title = "Settings";

            LastSavedDateTimeString = GetLastSavedDateTimeString(GetLastSavedSettingDateTime());
        }

        public async Task CreateOrUpdateSetting(string settingKey, string settingValue)
        {
            var setting = await SettingService.GetByNameAsync(settingKey);
            if (setting is Setting)
            {
                setting.SettingValue = settingValue;
                await SettingService.UpdateAsync(setting);
            }
            else
            {
                await SettingService.CreateAsync(new Setting()
                {
                    SettingName = settingKey,
                    SettingValue = settingValue,
                    LastSaved = DateTime.Now
                });
            }

            // if creating or updating auto complete type we also want to update the source
            if (setting is Setting && setting.SettingName.Equals(CollNotesSettings.AutoCompleteTypeKey))
            {
                CollNotesSettings.AutoCompleteSource = AutoCompleteSettings.GetAutoCompleteData(setting.SettingValue);
            }
        }

        public DateTime? GetLastSavedSettingDateTime()
        {
            if (Context.Settings.Count() > 0)
                return Context.Settings.Max(s => s.LastSaved);
            else
                return null;
        }

        public string GetLastSavedDateTimeString(DateTime? lastSavedDateTime)
        {
            return (lastSavedDateTime is null) ?
                "Never" :
                $"{Convert.ToDateTime(lastSavedDateTime).ToShortDateString()} {Convert.ToDateTime(lastSavedDateTime).ToShortTimeString()}";
        }

        public void ResetSettings()
        {
            CurrentCollectionCount = 0;
            CurrentCollectorName = string.Empty;
            SelectedAutoCompleteType = CollNotesSettings.AutoCompleteDefault;
            SelectedColorTheme = CollNotesSettings.ColorThemeDefault;
            SelectedExportFormat = CollNotesSettings.ExportFormatDefault;
            SelectedExportMethod = CollNotesSettings.ExportMethodDefault;
            LastSavedDateTimeString = GetLastSavedDateTimeString(null);
        }
    }
}
