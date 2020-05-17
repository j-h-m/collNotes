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
        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Launcher.OpenAsync(new Uri("https://github.com/j-h-m/collNotes/wiki")));
        }
    }
}