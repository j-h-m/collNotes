using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Permissions;
using collNotes.Services.Settings;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IAppThemeService appThemeService;
        private ISettingService settingService;
        private IExceptionRecordService exceptionRecordService;

        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";

            settingService = new SettingService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);

            OpenWebCommand = new Command(() => Launcher.OpenAsync(new Uri("https://github.com/j-h-m/collNotes/wiki")));
        }

        public async Task SetAppThemeToSaved()
        {
            var savedTheme = await appThemeService.GetSavedTheme();
            await appThemeService.SetAppTheme(savedTheme);
        }
    }
}