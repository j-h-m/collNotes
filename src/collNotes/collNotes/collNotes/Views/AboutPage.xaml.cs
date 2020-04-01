using System.ComponentModel;
using collNotes.Settings;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.Views
{
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        private readonly AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new AboutViewModel();

            BotanyNotebookImage.Aspect = (DeviceInfo.Platform == DevicePlatform.iOS) ?
                Aspect.AspectFit :
                Aspect.AspectFill;
        }
    }
}