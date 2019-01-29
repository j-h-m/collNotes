using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

/*
 * Uses the Plugin.Media package to get a picture and save it
 *  - May want to look at saving to an album with the Project name?
 */

namespace collnotes
{
    public static class TakePhoto
    {
        public async static Task<Plugin.Media.Abstractions.MediaFile> CallCamera(string fileNamePrefix)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "PD-Photos",
                    Name = fileNamePrefix + "-" + DateTime.Now.ToString("MM-dd-yyyy")
                });
            }

            return null;
        }
    }
}
