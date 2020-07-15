using System.Collections.Generic;
using System.Threading.Tasks;
using static collNotes.DeviceServices.Permissions.PermissionsService;

namespace collNotes.DeviceServices.Permissions
{
    public interface IPermissionsService
    {
        Task<Dictionary<PermissionName, bool>> RequestAllPermissionsAsync();
        Task<bool> CheckPermission(PermissionName permissionName);
        Task<bool> RequestPermission(PermissionName permissionName);
    }
}