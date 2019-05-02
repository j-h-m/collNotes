using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace collnotes.Plugins
{
    public static class CheckPermissions
    {
        /// <summary>
        /// Checks the external file permissions.
        /// </summary>
        /// <returns>The external file permissions.</returns>
        public static async Task<bool> CheckExternalFilePermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    return false;
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                {
                    status = results[Permission.Storage];
                }
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                return false;
            }

            return false;
        }
    }
}
