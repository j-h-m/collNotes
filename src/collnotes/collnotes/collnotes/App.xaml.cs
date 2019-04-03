using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using collnotes.Data;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace collnotes
{
    /// <summary>
    /// App.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            // navigation page allows the user to have a built-in back button for navigation
            // launch MainPage on load
            MainPage = new NavigationPage(new MainPage());
        }

        /// <summary>
        /// Ons the start.
        /// </summary>
        protected override void OnStart()
        {
            // Handle when your app starts
            // load app variables from file
            bool result = AppVarsFile.ReadAppVars();

            if (!result)
            {
                AppVariables.CollectionCount = DataFunctions.GetSpecimenCount();
            }

            // create tables for ORM, if not already created
            DatabaseFile.GetConnection().CreateTable<Project>();
            DatabaseFile.GetConnection().CreateTable<Trip>();
            DatabaseFile.GetConnection().CreateTable<Site>();
            DatabaseFile.GetConnection().CreateTable<Specimen>();
        }

        /// <summary>
        /// Ons the sleep.
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            AppVarsFile.WriteAppVars();
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            // Handle when your app resumes
            bool result = AppVarsFile.ReadAppVars();
        }
    }
}
