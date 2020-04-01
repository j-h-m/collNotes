using collNotes.Data.Context;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using collNotes.Views;
using Xamarin.Forms;

namespace collNotes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            Startup.Init();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}