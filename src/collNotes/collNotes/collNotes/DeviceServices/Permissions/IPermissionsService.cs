using System.Threading.Tasks;

namespace collNotes.DeviceServices.Permissions
{
    public interface IPermissionsService
    {
        Task RequestAllPermissionsAsync();
        Task<bool> CheckStoragePermission();
        Task<bool> RequestStoragePermission();
        Task<bool> CheckLocationPermission();
        Task<bool> RequestLocationPermission();
        Task<bool> CheckCameraPermission();
        Task<bool> RequestCameraPermission();
    }
}