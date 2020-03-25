using System.Collections.Generic;
using System.Linq;
using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Services.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.Views
{
    public partial class MenuPage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private List<HomeMenuItem> MenuItems;
        private MenuPageService menuPageService;
        private IConnectivityService connectivityService;

        public MenuPage()
        {
            InitializeComponent();
            menuPageService = new MenuPageService();
            connectivityService = new ConnectivityService();
            MenuItems = menuPageService.GetMenuPagesAsync().Result;
            ListViewMenu.ItemsSource = MenuItems;
            ListViewMenu.SelectedItem = MenuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            LogoContentView.Padding = (DeviceInfo.Platform == DevicePlatform.iOS) ?
                new Thickness(0, 75, 0, 0) :
                new Thickness(0, 20, 0, 20);
        }
    }
}