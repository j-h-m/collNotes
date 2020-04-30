using collNotes.Droid.ShareFolder;
using collNotes.ShareFolderInterface;

[assembly: Xamarin.Forms.Dependency(typeof(ShareFolderAndroid))]
namespace collNotes.Droid.ShareFolder
{
    public class ShareFolderAndroid : IShareFolder
    {
        public string GetShareFolder()
        {
            // deprecated
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "/";
        }
    }
}