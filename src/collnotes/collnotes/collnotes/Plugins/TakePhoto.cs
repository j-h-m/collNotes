using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

/*
 * Uses the Plugin.Media package to get a picture and save it
 *  - May want to look at saving to an album with the Project name?
 */

namespace collnotes.Plugins
{
    /// <summary>
    /// Take a photo.
    /// </summary>
    public static class TakePhoto
    {
        /// <summary>
        /// Calls the camera using the Plugin.Media.
        /// May require the Users permission: Camera, Location, Storage, and Photos (on iOS).
        /// </summary>
        /// <returns>The camera.</returns>
        /// <param name="fileName">File name.</param>
        public async static Task<Plugin.Media.Abstractions.MediaFile> CallCamera(string fileName)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted 
             || photosStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] {
                    Permission.Camera,
                    Permission.Storage,
                    Permission.Photos
                });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
                photosStatus = results[Permission.Photos];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted 
             && photosStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                    SaveToAlbum = true,
                    Directory = "collNotes",
                    Name = fileName
                });
            }

            return null;
        }
    }
}
