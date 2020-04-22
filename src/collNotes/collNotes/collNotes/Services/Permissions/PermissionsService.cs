using collNotes.Data.Context;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private IExceptionRecordService exceptionRecordService;

        public PermissionsService(CollNotesContext collNotesContext)
        {
            exceptionRecordService = new ExceptionRecordService(collNotesContext);
        }

        public async Task<bool> CheckStoragePermission()
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            return storageStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestStoragePermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            return requestResults.HasFlag(PermissionStatus.Denied) ||
                        requestResults.HasFlag(PermissionStatus.Disabled) ||
                        requestResults.HasFlag(PermissionStatus.Restricted) ||
                        requestResults.HasFlag(PermissionStatus.Unknown);
        }

        public async Task<bool> CheckLocationPermission()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            return locationStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestLocationPermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            return requestResults.HasFlag(PermissionStatus.Denied) ||
                        requestResults.HasFlag(PermissionStatus.Disabled) ||
                        requestResults.HasFlag(PermissionStatus.Restricted) ||
                        requestResults.HasFlag(PermissionStatus.Unknown);
        }

        public async Task<bool> CheckCameraPermission()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            return cameraStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestCameraPermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
            return requestResults.HasFlag(PermissionStatus.Denied) ||
                        requestResults.HasFlag(PermissionStatus.Disabled) ||
                        requestResults.HasFlag(PermissionStatus.Restricted) ||
                        requestResults.HasFlag(PermissionStatus.Unknown);
        }

        public async Task RequestAllPermissionsAsync()
        {
            try
            {
                var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                var mediaLibStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

                if (locationStatus != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }
                if (cameraStatus != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }
                if (photosStatus != PermissionStatus.Granted) 
                {
                    await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                }
                if (mediaLibStatus != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                }
                if (storageStatus != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }
        }
    }
}