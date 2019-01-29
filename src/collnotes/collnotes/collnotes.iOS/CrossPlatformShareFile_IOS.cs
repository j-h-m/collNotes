using System;

[assembly: Xamarin.Forms.Dependency(typeof(collnotes.iOS.CrossPlatformShareFile_IOS))]
namespace collnotes.iOS
{
    public class CrossPlatformShareFile_IOS : ICrossPlatform_GetShareFolder
    {
        public string GetShareFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/";
        }
    }
}