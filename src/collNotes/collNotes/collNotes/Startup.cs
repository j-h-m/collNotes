using collNotes.Data.Context;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using collNotes.Settings;
using collNotes.Settings.AutoComplete;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes
{
    public static class Startup
    {
        public static void Init()
        {
            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                SQLitePCL.Batteries.Init();
            }
            // ef core context
            DependencyService.Register<CollNotesContext>();
            // settings view model, registered to use as a Singleton
            // for changes to reflect on settings view, need to use one instance
            DependencyService.Register<SettingsViewModel>();

            CollNotesSettings.IsAppStartingUp = true;
        }
    }
}