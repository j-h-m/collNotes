using collNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace collNotes.Views
{
    public partial class FieldIDSearchPage : ContentPage, INotifyPropertyChanged
    {
        private readonly SearchBarViewModel searchBarViewModel;
        public string SelectedFieldID { get; set; }

        public FieldIDSearchPage()
        {
            InitializeComponent();
            BindingContext = searchBarViewModel = new SearchBarViewModel();
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (Search_TextField.Text?.Length == 3)
            {
                string searchText = Search_TextField.Text.Substring(0, 1).ToUpper() + Search_TextField.Text.Substring(1);

                if (searchBarViewModel.AutoCompleteDict.ContainsKey(searchText))
                {
                    List<string> value = new List<string>();
                    searchBarViewModel.AutoCompleteDict.TryGetValue(searchText, out value);
                    searchBarViewModel.AutoCompleteSource = value;
                    SearchResult_ListView.ItemsSource = searchBarViewModel.AutoCompleteSource;
                }
            }
            else if (Search_TextField.Text?.Length > 3)
            {
                string searchPrefix = Search_TextField.Text.Substring(0, 1).ToUpper() + Search_TextField.Text.Substring(1, 2);

                if (searchBarViewModel.AutoCompleteDict.ContainsKey(searchPrefix))
                {
                    searchBarViewModel.AutoCompleteSource = searchBarViewModel.AutoCompleteDict[searchPrefix];
                }

                searchBarViewModel.AutoCompleteSource = searchBarViewModel.AutoCompleteSource.Where(acs =>
                    acs.StartsWith(Search_TextField.Text, ignoreCase: true, CultureInfo.CurrentCulture)).ToList();

                SearchResult_ListView.ItemsSource = searchBarViewModel.AutoCompleteSource;
            }
            else
            {
                if (searchBarViewModel.AutoCompleteSource.Count > 0)
                {
                    SearchResult_ListView.ItemsSource = new List<string>();
                }
            }
        }

        private void SearchResult_ItemSelected(object sender, EventArgs e)
        {
            SelectedFieldID = Search_TextField.Text = SearchResult_ListView.SelectedItem?.ToString();
        }
    }
}