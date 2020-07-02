using collNotes.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.Views
{
    public partial class MainPage : MasterDetailPage
    {
        private Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add((int)MenuItemType.About, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;

                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;

                    case (int)MenuItemType.ExportImport:
                        MenuPages.Add(id, new NavigationPage(new ExportImportPage()));
                        break;

                    case (int)MenuItemType.Trips:
                        MenuPages.Add(id, new NavigationPage(new TripsPage()));
                        break;

                    case (int)MenuItemType.Sites:
                        MenuPages.Add(id, new NavigationPage(new SitesPage()));
                        break;

                    case (int)MenuItemType.Specimen:
                        MenuPages.Add(id, new NavigationPage(new SpecimenPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}