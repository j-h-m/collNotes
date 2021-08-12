using collNotes.DeviceServices.AppTheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace collNotes.ViewModels
{
    public class AppShellViewModel: BaseViewModel
    {
        private IAppThemeService appThemeService;

        public AppShellViewModel()
        {
            appThemeService = new AppThemeService();
        }

        // Sets app theme to saved theme
        public void AppThemeInit()
        {
            var savedTheme = appThemeService.GetSavedTheme().Result;
            var result = appThemeService.SetAppTheme(savedTheme).Result;
        }
    }
}
