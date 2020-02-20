using collNotes.Data.Context;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.Services.Permissions
{
    public class PermissionsService
    {
        private ExceptionRecordService exceptionRecordService;

        public PermissionsService(CollNotesContext collNotesContext)
        {
            exceptionRecordService = new ExceptionRecordService(collNotesContext);
        }

        public async Task<bool> CheckStoragePermission()
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            return storageStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestStoragePermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            return requestResults.ContainsValue(PermissionStatus.Denied) ||
                        requestResults.ContainsValue(PermissionStatus.Disabled) ||
                        requestResults.ContainsValue(PermissionStatus.Restricted) ||
                        requestResults.ContainsValue(PermissionStatus.Unknown);
        }

        public async Task<bool> CheckLocationPermission()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            return locationStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestLocationPermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
            return requestResults.ContainsValue(PermissionStatus.Denied) ||
                        requestResults.ContainsValue(PermissionStatus.Disabled) ||
                        requestResults.ContainsValue(PermissionStatus.Restricted) ||
                        requestResults.ContainsValue(PermissionStatus.Unknown);
        }

        public async Task<bool> CheckCameraPermission()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            return cameraStatus == PermissionStatus.Granted;
        }

        public async Task<bool> RequestCameraPermission()
        {
            var requestResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
            return requestResults.ContainsValue(PermissionStatus.Denied) ||
                        requestResults.ContainsValue(PermissionStatus.Disabled) ||
                        requestResults.ContainsValue(PermissionStatus.Restricted) ||
                        requestResults.ContainsValue(PermissionStatus.Unknown);
        }

        public async Task<bool> RequestAllPermissionsAsync()
        {
            bool result;

            try
            {
                var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                var mediaLibStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.MediaLibrary);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                List<Permission> permissions = new List<Permission>();

                if (locationStatus != PermissionStatus.Granted) permissions.Add(Permission.Location);
                if (cameraStatus != PermissionStatus.Granted) permissions.Add(Permission.Camera);
                if (photosStatus != PermissionStatus.Granted) permissions.Add(Permission.Photos);
                if (mediaLibStatus != PermissionStatus.Granted) permissions.Add(Permission.MediaLibrary);
                if (storageStatus != PermissionStatus.Granted) permissions.Add(Permission.Storage);

                if (permissions.Count > 0)
                {
                    var requestResults = await CrossPermissions.Current.RequestPermissionsAsync(permissions.ToArray());
                    result = requestResults.ContainsValue(PermissionStatus.Denied) ||
                        requestResults.ContainsValue(PermissionStatus.Disabled) ||
                        requestResults.ContainsValue(PermissionStatus.Restricted) ||
                        requestResults.ContainsValue(PermissionStatus.Unknown);
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }
    }
}