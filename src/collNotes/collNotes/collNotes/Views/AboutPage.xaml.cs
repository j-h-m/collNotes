using collNotes.ViewModels;
using System;
using System.ComponentModel;
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
        }
    }
}