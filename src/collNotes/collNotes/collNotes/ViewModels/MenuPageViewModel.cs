using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace collNotes.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        public MenuPageViewModel()
        {
            ISettingService settingService = new SettingService(Context);
            IExceptionRecordService exceptionRecordService = new ExceptionRecordService(Context);
            IAppThemeService appThemeService = new AppThemeService(settingService, exceptionRecordService);

            appThemeService.SetAppTheme(appThemeService.GetSavedTheme().Result).Wait();
        }
    }
}
