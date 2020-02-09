using System.Threading.Tasks;

namespace collNotes.Services.Permissions
{
    public interface IPermissionsService
    {
        Task RequestAllPermissionsAsync();
    }
}