using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PDSkeleton
{
    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            // navigation page allows the user to have a built-in back button for navigation
            // launch MainPage on load
            MainPage = new NavigationPage(new MainPage());
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
            // load app variables from file
            bool result = AppVarsFile.ReadAppVars();
            // create tables for ORM, if not already created
            ORM.GetConnection().CreateTable<Project>();
            ORM.GetConnection().CreateTable<Trip>();
            ORM.GetConnection().CreateTable<Site>();
            ORM.GetConnection().CreateTable<Specimen>();
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            AppVarsFile.WriteAppVars();
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
            bool result = AppVarsFile.ReadAppVars();
		}
	}
}
