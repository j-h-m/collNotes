using Android.Content;
using collNotes.Droid.ShareFolder;
using collNotes.ShareFolderInterface;
using System.Security;

[assembly: Xamarin.Forms.Dependency(typeof(ShareFolderAndroid))]
namespace collNotes.Droid.ShareFolder
{
    [SecurityCritical]
    public class ShareFolderAndroid : IShareFolder
    {
        [System.Obsolete]
        public string GetShareFolder_Obsolete()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "/";
        }

        /// <summary>
        /// No longer using external storage so issues on some devices without it should be solved.
        /// GetDir gets a directory that is created as part of your application data.
        /// </summary>
        [SecurityCritical]
        public string GetShareFolder()
        {
            Context currentContext = Android.App.Application.Context;

            return currentContext.GetDir(Android.OS.Environment.DirectoryDownloads, FileCreationMode.Private).AbsolutePath + "/";
        }
    }
}