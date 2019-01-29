using Android.App;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(collnotes.Droid.CrossPlatformToast_Android))]
namespace collnotes.Droid
{
    public class CrossPlatformToast_Android : ICrossPlatformToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}