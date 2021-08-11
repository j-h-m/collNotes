using collNotes.Services;
using collNotes.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace collNotes
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            Startup.Init();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
