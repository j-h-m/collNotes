using System;
using System.Threading.Tasks;
using Plugin.Media;

/*
 * Uses the Plugin.Media package to get a picture and save it
 * May want to look at saving to an album ID by the Project Name ... if possible
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
                    Directory = "PD-Photos",
                    Name = fileNamePrefix + "-" + DateTime.Now.ToString("MM-dd-yyyy")
                };
                
                return await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            }
            return null;
        }
    }
}
