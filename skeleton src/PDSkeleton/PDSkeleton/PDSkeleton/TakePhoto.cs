using System;
using System.Threading.Tasks;
using Plugin.Media;

/*
 * Uses the Plugin.Media package to get a picture and save it
 *  - May want to look at saving to an album with the Project name?
 */ 

namespace PDSkeleton
{
    public static class TakePhoto
    {
        public async static Task<Plugin.Media.Abstractions.MediaFile> CallCamera(string fileNamePrefix)
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    // tried just PD-Photos
                    // Directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/PD-Photos",
                    Name = fileNamePrefix + "-" + DateTime.Now.ToString("MM-dd-yyyy"),
                    // try saving to album instead of directory
                    SaveToAlbum = true                    
                };
                
                return await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            }
            return null;
        }
    }
}
