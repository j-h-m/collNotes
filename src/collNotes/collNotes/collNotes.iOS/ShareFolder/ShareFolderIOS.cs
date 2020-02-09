using collNotes.iOS.ShareFolder;
using collNotes.ShareFolderInterface;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(ShareFolderIOS))]
namespace collNotes.iOS.ShareFolder
{
    public class ShareFolderIOS : IShareFolder
    {
        public string GetShareFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/";
        }
    }
}