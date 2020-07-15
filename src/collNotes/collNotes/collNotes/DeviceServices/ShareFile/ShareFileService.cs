using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.DeviceServices
{
    public class ShareFileService : IShareFileService
    {
        public ShareFileService() { }

        public async Task ShareFile(string filePath, string title)
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = new ShareFile(filePath)
            });
        }
    }
}
