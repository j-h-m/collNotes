[assembly: Xamarin.Forms.Dependency(typeof(PDSkeleton.Droid.CrossPlatformShareFile_Android))]
namespace PDSkeleton.Droid
{
    public class CrossPlatformShareFile_Android : ICrossPlatform_GetShareFolder
    {
        public string GetShareFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "/";
        }
    }
}