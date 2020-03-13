using collNotes.Services.Permissions;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public readonly IPermissionsService permissionsService;
        public ICommand OpenWebCommand { get; }
        public ICommand RequestPermissionsCommand { get; }

        public AboutViewModel()
        {
            Title = "About";

            permissionsService = new PermissionsService(Context);

            OpenWebCommand = new Command(() => Launcher.OpenAsync(new Uri("https://github.com/j-h-m/collNotes/wiki")));
        }
    }
}