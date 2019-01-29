[assembly: Xamarin.Forms.Dependency(typeof(collnotes.Droid.CrossPlatformShareFile_Android))]
namespace collnotes.Droid
{
    public class CrossPlatformShareFile_Android : ICrossPlatform_GetShareFolder
    {
        public string GetShareFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "/";
        }
    }
}