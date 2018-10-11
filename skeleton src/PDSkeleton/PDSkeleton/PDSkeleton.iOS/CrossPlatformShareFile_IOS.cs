using System;

[assembly: Xamarin.Forms.Dependency(typeof(PDSkeleton.iOS.CrossPlatformShareFile_IOS))]
namespace PDSkeleton.iOS
{
    public class CrossPlatformShareFile_IOS : ICrossPlatform_GetShareFolder
    {
        public string GetShareFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/";
        }
    }
}