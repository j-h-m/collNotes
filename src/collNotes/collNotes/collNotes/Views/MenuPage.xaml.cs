using System.Collections.Generic;
using collNotes.Data.Models;
using collNotes.Services;
using Xamarin.Forms;

namespace collNotes.Views
{
    public partial class MenuPage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private List<HomeMenuItem> MenuItems;
        private MenuPageService MenuPageService { get; set; }

        public MenuPage()
        {
            InitializeComponent();
            MenuPageService = new MenuPageService();
            MenuItems = MenuPageService.GetMenuPagesAsync().Result;
            ListViewMenu.ItemsSource = MenuItems;
            ListViewMenu.SelectedItem = MenuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}