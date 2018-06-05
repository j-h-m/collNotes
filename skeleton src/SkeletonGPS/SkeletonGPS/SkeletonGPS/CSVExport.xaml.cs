using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using PCLStorage;

namespace SkeletonGPS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CSVExport : ContentPage
	{
		public CSVExport ()
		{
			InitializeComponent();
		}

        public async Task OnClick_CSV(object sender, EventArgs e)
        {
            try
            {
                string gps = GPSPage.GPSLocation;
                IFileSystem fileSystem = FileSystem.Current;
                IFolder rootFolder = fileSystem.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("pdproject",
                    CreationCollisionOption.ReplaceExisting);
                IFile file = await rootFolder.CreateFileAsync("location.txt",
                    CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync(gps);
            }
            catch (Exception ex)
            {
                lbl_CSVStatus.Text = ex.Message;
                Debug.Write(ex.Message);
            }
        }

    }
}