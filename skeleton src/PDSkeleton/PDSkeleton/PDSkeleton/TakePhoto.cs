using System;
using System.Threading.Tasks;
using Plugin.Media;

namespace PDSkeleton
{
    public static class TakePhoto
    {
        public async static Task<Plugin.Media.Abstractions.MediaFile> CallCamera()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "PD-Photos",
                    Name = DateTime.Now.ToString("MM-dd-yyyy")
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
