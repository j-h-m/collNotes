using System;
using System.Threading.Tasks;
using Plugin.Media;

/*
 * Uses the Plugin.Media package to get a picture and save it to a directory called PD-Photos with the name as the month-day-year hour:minute:seconds
 * returns a Task or null
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
            else
            {
                return null;
            }
        }
    }
}
