using collNotes.Services.Data;
using Plugin.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.DeviceServices.Camera
{
    public class CameraService : ICameraService
    {
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public CameraService() { }

        public async Task<string> TakePicture(string photoName)
        {
            string photoAsBase64 = string.Empty;
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await MaterialDialog.Instance.AlertAsync("No camera available.");
                }
                else
                {
                    var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        Directory = "collNotes",
                        Name = photoName
                    });

                    if (!(photo is null))
                        photoAsBase64 = Convert.ToBase64String(File.ReadAllBytes(photo.Path));
                }
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return photoAsBase64;
        }
    }
}