using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Permissions;
using collNotes.Domain.Models;
using collNotes.Services.Data;
using collNotes.Services;
using collNotes.Settings;
using collNotes.Settings.AutoComplete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using collNotes.Ef.Context;
using collNotes.Views;

namespace collNotes.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly CollNotesContext Context =
            DependencyService.Get<CollNotesContext>(DependencyFetchTarget.GlobalInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);
        private readonly ISettingService settingService =
            DependencyService.Get<ISettingService>(DependencyFetchTarget.NewInstance);
        private readonly IAppThemeService appThemeService =
            DependencyService.Get<IAppThemeService>(DependencyFetchTarget.NewInstance);
        private readonly I_ImportRecordService importRecordService =
            DependencyService.Get<I_ImportRecordService>(DependencyFetchTarget.NewInstance);

        #region Binding Properties

        private string _CurrentCollectorName;
        public string CurrentCollectorName
        {
            get { return _CurrentCollectorName; }
            set
            {
                _CurrentCollectorName = value;
                OnPropertyChanged(nameof(CurrentCollectorName));
            }
        }
        private int _CurrentCollectionCount;
        public int CurrentCollectionCount
        {
            get { return _CurrentCollectionCount; }
            set
            {
                _CurrentCollectionCount = value;
                OnPropertyChanged(nameof(CurrentCollectionCount));
            }
        }
        private string _CurrentCollectionCountString;
        public string CurrentCollectionCountString
        {
            get { return _CurrentCollectionCountString ?? CollNotesSettings.CollectionCountStringDefault; }
            set
            {
                _CurrentCollectionCountString = value;
                OnPropertyChanged(nameof(CurrentCollectionCountString));

                int parseResult = 0;
                CurrentCollectionCount = int.TryParse(value, out parseResult) ?
                    parseResult : CollNotesSettings.CollectionCountDefault;
            }
        }
        private string _SelectedExportFormat;
        public string SelectedExportFormat
        {
            get { return _SelectedExportFormat ?? CollNotesSettings.ExportFormatDefault; }
            set
            {
                _SelectedExportFormat = value;
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
            Title = "Settings";

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteImportRecords", (sender) =>
            {
                importRecordService.DeleteAll();
            });

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteSettings", async (sender) =>
            {
                await settingService.DeleteAllAsync();
            });

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteExceptionRecords", async (sender) =>
            {
                await exceptionRecordService.DeleteAllAsync();
            });
        }

        public async Task<bool> CreateOrUpdateSetting(string settingKey, string settingValue)
        {
            var setting = await settingService.GetByNameAsync(settingKey);
            bool result = false;
            if (setting is Setting)
            {
                setting.SettingValue = settingValue;
                result = await settingService.UpdateAsync(setting);
            }
            else
            {
                result = await settingService.CreateAsync(new Setting()
                {
                    SettingName = settingKey,
                    SettingValue = settingValue,
                    LastSaved = DateTime.Now
                });
            }

            // if creating or updating auto complete type we also want to update the source
            if (setting is Setting && setting.SettingName.Equals(CollNotesSettings.AutoCompleteTypeKey, StringComparison.CurrentCulture))
            {
                CollNotesSettings.AutoCompleteSource = AutoCompleteSettings.GetAutoCompleteData(setting.SettingValue);
            }

            return result;
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

        public async Task ResetSettings()
        {
            CurrentCollectionCountString = CollNotesSettings.CollectionCountStringDefault;
            CurrentCollectorName = string.Empty;
            SelectedAutoCompleteType = CollNotesSettings.AutoCompleteDefault;
            SelectedExportFormat = CollNotesSettings.ExportFormatDefault;
            SelectedExportMethod = CollNotesSettings.ExportMethodDefault;
            SelectedColorTheme = CollNotesSettings.ColorThemeDefault;
            LastSavedDateTimeString = GetLastSavedDateTimeString(null);

            await UpdateSettings();
        }

        public async Task<bool> SaveTheme(CollNotesSettings.ColorTheme colorTheme)
        {
            return await appThemeService.SaveAppTheme(colorTheme);
        }

        public async Task<bool> SetSettingsToSavedValues()
        {
            bool result = false;

            try
            {
                var autoCompleteTypeSetting = await settingService.GetByNameAsync(CollNotesSettings.AutoCompleteTypeKey);
                var collectionCountSetting = await settingService.GetByNameAsync(CollNotesSettings.CollectionCountKey);
                var colorThemeSetting = await settingService.GetByNameAsync(CollNotesSettings.ColorThemeKey);
                var exportFormatSetting = await settingService.GetByNameAsync(CollNotesSettings.ExportFormatKey);
                var exportMethodSetting = await settingService.GetByNameAsync(CollNotesSettings.ExportMethodKey);
                var primaryCollectorSetting = await settingService.GetByNameAsync(CollNotesSettings.PrimaryCollectorKey);

                SelectedAutoCompleteType = (autoCompleteTypeSetting is null) ?
                    CollNotesSettings.AutoCompleteDefault : autoCompleteTypeSetting.SettingValue;
                CurrentCollectionCountString = (collectionCountSetting is null) ?
                    CollNotesSettings.CollectionCountStringDefault : collectionCountSetting.SettingValue;
                SelectedColorTheme = (colorThemeSetting is null) ?
                    CollNotesSettings.ColorThemeDefault : colorThemeSetting.SettingValue;
                SelectedExportFormat = (exportFormatSetting is null) ?
                    CollNotesSettings.ExportFormatDefault : exportFormatSetting.SettingValue;
                SelectedExportMethod = (exportMethodSetting is null) ?
                    CollNotesSettings.ExportMethodDefault : exportMethodSetting.SettingValue;
                CurrentCollectorName = (primaryCollectorSetting is null) ?
                    string.Empty : primaryCollectorSetting.SettingValue;

                result = true;
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }

        public async Task<bool> UpdateSettings()
        {
            bool result = false;

            try
            {
                await CreateOrUpdateSetting(CollNotesSettings.AutoCompleteTypeKey, SelectedAutoCompleteType);
                await CreateOrUpdateSetting(CollNotesSettings.CollectionCountKey, CurrentCollectionCount.ToString());
                await CreateOrUpdateSetting(CollNotesSettings.ColorThemeKey, SelectedColorTheme);
                await CreateOrUpdateSetting(CollNotesSettings.ExportFormatKey, SelectedExportFormat);
                await CreateOrUpdateSetting(CollNotesSettings.ExportMethodKey, SelectedExportMethod);
                await CreateOrUpdateSetting(CollNotesSettings.PrimaryCollectorKey, CurrentCollectorName);

                result = true;
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
                result = false;
            }

            return result;
        }
    }
}
