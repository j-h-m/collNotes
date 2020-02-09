using collNotes.Data.Context;
using collNotes.Settings;
using collNotes.Settings.AutoComplete;
using collNotes.ViewModels;
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

            SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
            string autoCompleteType = 
                settingsViewModel.SelectedAutoCompleteType = settingsViewModel.SelectedAutoCompleteType ?? CollNotesSettings.AutoCompleteDefault;

            CollNotesSettings.AutoCompleteSource = AutoCompleteSettings.GetAutoCompleteData(autoCompleteType);
        }
    }
}