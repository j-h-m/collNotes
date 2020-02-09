using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.Services
{
    public class ShareFileService : IShareFileService
    {
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
