using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
            var appThemeResult = appThemeService.SetAppTheme(savedTheme).Result;
        }
    }
}
