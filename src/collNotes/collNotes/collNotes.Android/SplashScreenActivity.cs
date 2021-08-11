using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace collNotes.Droid
{
    [Activity(Label = "collNotes", Theme = "@style/SplashTheme.Screen", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashScreenActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            if (savedInstanceState is null)
            {
                throw new ArgumentNullException(nameof(savedInstanceState));
            }

            base.OnCreate(savedInstanceState, persistentState);
            Console.WriteLine(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        void SimulateStartup()
        {
            Console.WriteLine(TAG, "starting MainActivity.");
            Intent intent = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}