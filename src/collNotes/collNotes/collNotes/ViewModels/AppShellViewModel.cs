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
        private readonly IPermissionsService permissionsService =
            DependencyService.Get<IPermissionsService>(DependencyFetchTarget.NewInstance);

        public AppShellViewModel()
        {
            appThemeService = new AppThemeService();
        }

        // Sets app theme to saved theme
        public void AppThemeInit()
        {
            var savedTheme = appThemeService.GetSavedTheme().Result;
            var appThemeResult = appThemeService.SetAppTheme(savedTheme).Result;
            var permissionsResult= permissionsService.RequestAllPermissionsAsync().Result;

        }
    }
}
