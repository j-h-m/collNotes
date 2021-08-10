using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using System;
using System.Security;

namespace collNotes.Droid
{
    [Activity(Label = "collNotes", Icon = "@drawable/icon", Theme = "@style/MainTheme", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
#pragma warning disable CS3009 // Base type is not CLS-compliant
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
#pragma warning restore CS3009 // Base type is not CLS-compliant
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        protected override void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (savedInstanceState is null)
            {
                throw new ArgumentNullException(nameof(savedInstanceState));
            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (grantResults is null)
            {
                throw new ArgumentNullException(nameof(grantResults));
            }

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}