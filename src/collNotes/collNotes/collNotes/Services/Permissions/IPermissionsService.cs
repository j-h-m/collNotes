using System.Threading.Tasks;

namespace collNotes.Services.Permissions
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