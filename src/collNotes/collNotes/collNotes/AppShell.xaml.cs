﻿using collNotes.DeviceServices.Permissions;
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
            Routing.RegisterRoute(nameof(TripsPage), typeof(TripsPage));
            Routing.RegisterRoute(nameof(SitesPage), typeof(SitesPage));
            Routing.RegisterRoute(nameof(SpecimenPage), typeof(SpecimenPage));
            Routing.RegisterRoute(nameof(ExportImportPage), typeof(ExportImportPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

            viewModel = new AppShellViewModel();
            viewModel.AppThemeInit();
        }
    }
}
