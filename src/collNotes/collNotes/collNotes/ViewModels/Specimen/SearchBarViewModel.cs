using collNotes.Settings;
using System.Collections.Generic;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class SearchBarViewModel : BaseViewModel
    {
        public List<string> AutoCompleteSource { get; set; }
        public Dictionary<string, List<string>> AutoCompleteDict { get; set; }
        public string SelectedFieldID { get; set; }
        public string SearchBarLabel { get; set; }
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public SearchBarViewModel()
        {
            AutoCompleteDict = CollNotesSettings.AutoCompleteSource;
            AutoCompleteSource = new List<string>();
            SearchBarLabel = $"Search {settingsViewModel.SelectedAutoCompleteType} Sci. Names";
            Title = "Search Field IDs";
        }
    }
}