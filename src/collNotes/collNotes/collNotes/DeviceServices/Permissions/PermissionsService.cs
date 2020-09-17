using collNotes.Ef.Context;
using collNotes.Services.Data;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.DeviceServices.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public enum PermissionName
        {
            Storage,
            Location,
            Camera,
            Photos,
            MediaLib
        }

        public PermissionsService() { }

        /// <summary>
        /// Checks permission status.
        /// Returns true if granted.
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public async Task<bool> CheckPermission(PermissionName permissionName)
        {
            PermissionStatus status;

            if (permissionName == PermissionName.Camera)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            }
            else if (permissionName == PermissionName.Location)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            }
            else if (permissionName == PermissionName.MediaLib)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
            }
            else if (permissionName == PermissionName.Photos)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
            }
            else if (permissionName == PermissionName.Storage)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            }
            else { status = PermissionStatus.Unknown; }

            return status == PermissionStatus.Granted;
        }

        /// <summary>
        /// Requests permission from user if not already granted.
        /// Returns result of permission request.
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public async Task<bool> RequestPermission(PermissionName permissionName)
        {
            if (await CheckPermission(permissionName)) // permission status granted
            {
                return true;
            }

            PermissionStatus status;

            if (permissionName == PermissionName.Camera)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
            }
            else if (permissionName == PermissionName.Location)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }
            else if (permissionName == PermissionName.MediaLib)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
            }
            else if (permissionName == PermissionName.Photos)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
            }
            else if (permissionName == PermissionName.Storage)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }
            else
            {
                status = PermissionStatus.Unknown;
            }

            return status == PermissionStatus.Granted;
        }

        /// <summary>
        /// Request all permissions in PermissionName Enum.
        /// Returns dictionary with PermissionName and check/request result value {PermissionName, bool}.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<PermissionName, bool>> RequestAllPermissionsAsync()
        {
            Dictionary<PermissionName, bool> resultDict = new Dictionary<PermissionName, bool>();

            try
            {
                foreach (var pn in Enum.GetValues(typeof(PermissionName)).Cast<PermissionName>())
                {
                    if (Device.RuntimePlatform != Device.iOS ||
                        pn != PermissionName.MediaLib)
                    {
                        var result = await RequestPermission(pn);
                        resultDict.Add(pn, result);
                    }
                }
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return resultDict;
        }
    }
}