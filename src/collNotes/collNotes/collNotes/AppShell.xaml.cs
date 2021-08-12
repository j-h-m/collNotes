using collNotes.ViewModels;
using collNotes.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace collNotes
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private AppShellViewModel viewModel;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(ExportImportPage), typeof(ExportImportPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(CollectPage), typeof(CollectPage));

            viewModel = new AppShellViewModel();
            viewModel.AppThemeInit();
        }
    }
}
