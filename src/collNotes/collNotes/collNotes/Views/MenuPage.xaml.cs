using System;
using System.Collections.Generic;
using System.Linq;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.Views
{
    public partial class MenuPage : ContentPage
    {
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private List<HomeMenuItem> MenuItems;
        private MenuPageViewModel menuPageViewModel;

        public MenuPage()
        {
            InitializeComponent();
            menuPageViewModel = new MenuPageViewModel();
            MenuItems = menuPageViewModel.GetMenuItems();
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

        private void ListViewMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedMenuItem = e.SelectedItem as HomeMenuItem;
                var otherMenuItems = (ListViewMenu.ItemsSource as List<HomeMenuItem>).FindAll(hmi => hmi.Id != selectedMenuItem.Id);

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                var currentTheme = mergedDictionaries.Where(dict => !SubstringExistsInStringList(dict.Keys, "Material")).FirstOrDefault();
                var foo = currentTheme["SecondaryBackgroundColor"];
                var bar = currentTheme["PageBackgroundColor"];

                if (currentTheme != null)
                {
                    selectedMenuItem.BackgroundColor = (Color)foo;
                    otherMenuItems.ForEach(hmi => hmi.BackgroundColor = (Color)bar);
                }
            }
        }

        private bool SubstringExistsInStringList(ICollection<string> list, string substring)
        {
            foreach (var item in list)
            {
                if (item.ToLowerInvariant().Contains(substring.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}