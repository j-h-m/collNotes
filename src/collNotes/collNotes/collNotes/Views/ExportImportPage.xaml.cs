using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.Connectivity;
using collNotes.Domain.Models;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Settings;
using collNotes.ShareFolderInterface;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportImportPage : ContentPage
    {
        private bool IsDeviceIosSimulator = DeviceInfo.Platform == DevicePlatform.iOS &&
                DeviceInfo.DeviceType == DeviceType.Virtual;

        private readonly ExportImportViewModel viewModel;

        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);
        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);
        private readonly IConnectivityService connectivityService =
            DependencyService.Get<IConnectivityService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);
        private readonly IImportRecordService importRecordService =
            DependencyService.Get<IImportRecordService>(DependencyFetchTarget.NewInstance);

        public ExportImportPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ExportImportViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var netStatus = connectivityService.GetNetworkStatus();
            viewModel.IsConnectionAvailable = netStatus == ConnectivityService.ActualConnectivity.Connected;
            viewModel.ShowConnectionErrorMsg = !viewModel.IsConnectionAvailable;
        }

        private async void ImportTrip()
        {
            if (IsDeviceIosSimulator)
            {
                var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Import Trip not supported on iOS Simulator",
                    configuration: alertDialogConfig);
            }
            else
            {
                var stream = await OpenFileDialog();
                string message = string.Empty;

                if (await importRecordService.HasFileBeenImported(stream.Item2))
                {
                    message = "This file has already been imported!";
                }
                else
                {
                    var loadingDialogConfig = await xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Attempting to import Trip",
                        configuration: loadingDialogConfig))
                    {
                        if (await viewModel.ImportTrip(stream.Item1))
                        {
                            // log trip import
                            await importRecordService.AddAsync(new ImportRecord()
                            {
                                Created = DateTime.Now,
                                FileName = stream.Item2
                            });

                            message = "Trip imported successfully.";
                        }
                        else
                        {
                            message = stream.Item1 is null ?
                                "No Trip file selected." : "Trip import failed.";
                        }
                    }
                }

                var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }

        private async void ExportTrip()
        {
            if (IsDeviceIosSimulator)
            {
                var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Export Trip not supported on iOS Simulator",
                    configuration: alertDialogConfig);
            }
            else
            {
                var trips = await tripService.GetAllAsync();

                if (trips.Any())
                {
                    var confirmationDialogConfig = await xfMaterialColorConfigFactory.GetConfirmationDialogConfiguration();
                    var choices = trips.Select(t => t.TripName).ToList();
                    choices.Add("Export All");
                    var result = await MaterialDialog.Instance.SelectChoiceAsync("Select a trip", choices,
                        configuration: confirmationDialogConfig);

                    string message = string.Empty;

                    if (result != -1)
                    {
                        var selectedChoice = choices[result];
                        List<Trip> selectedTrips = new List<Trip>();
                        if (selectedChoice.Equals("Export All"))
                        {
                            selectedTrips = trips.ToList();
                        }
                        else
                        {
                            selectedTrips = trips.Where(t => t.TripName.Equals(selectedChoice)).ToList();
                        }
                        // var selectedTrip = trips.ToArray()[result];

                        if (await viewModel.ExportTrip(selectedTrips, GetFilePath("Export.csv")))
                        {
                            message = "Trip exported successfully";
                        }
                        else
                        {
                            message = "Trip export failed";
                        }

                        var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                        await MaterialDialog.Instance.SnackbarAsync(message: message,
                                                actionButtonText: "Ok",
                                                msDuration: MaterialSnackbar.DurationIndefinite,
                                                configuration: snackbarConfig);
                    }
                }
                else
                {
                    var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                    await MaterialDialog.Instance.AlertAsync("No Trips to export!",
                        configuration: alertDialogConfig);
                }
            }
        }

        private async void ImportBackup()
        {
            if (IsDeviceIosSimulator)
            {
                var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Import Backup not supported on iOS Simulator",
                    configuration: alertDialogConfig);
            }
            else
            {
                var stream = await OpenFileDialog();
                string message = string.Empty;

                if (await importRecordService.HasFileBeenImported(stream.Item2))
                {
                    message = "This file has already been imported!";
                }
                else
                {
                    var loadingDialogConfig = await xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Attempting to import Backup",
                        configuration: loadingDialogConfig))
                    {
                        if (await viewModel.ImportBackup(stream.Item1))
                        {
                            // log backup import
                            await importRecordService.AddAsync(new ImportRecord()
                            {
                                Created = DateTime.Now,
                                FileName = stream.Item2
                            });

                            message = "Backup imported successfully.";
                        }
                        else
                        {
                            message = stream.Item1 is null ?
                                    "No Backup file selected." : "Backup import failed.";
                        }
                    }
                }

                var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }

        private async void ExportBackup()
        {
            if (IsDeviceIosSimulator)
            {
                var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Export Backup not supported on iOS Simulator",
                    configuration: alertDialogConfig);
            }
            else
            {
                string message = string.Empty;
                if (await viewModel.ExportBackup(GetFilePath("collNotes_Backup.xml")))
                {
                    message = "Backup exported successfully.";
                }
                else
                {
                    message = "Backup export failed.";
                }

                var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: message,
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedDirection.Equals("Export"))
            {
                if (viewModel.SelectedType.Equals("Backup"))
                {
                    ExportBackup();
                }
                else if (viewModel.SelectedType.Equals("Trip"))
                {
                    ExportTrip();
                }
            }

            if (viewModel.SelectedDirection.Equals("Import"))
            {
                if (viewModel.SelectedType.Equals("Backup"))
                {
                    ImportBackup();
                }
                else if (viewModel.SelectedType.Equals("Trip"))
                {
                    ImportTrip();
                }
            }
        }

        private string GetFilePath(string fileName)
        {
            string filePath = DependencyService.Get<IShareFolder>().GetShareFolder();
            return Path.Combine(filePath, fileName);
        }

        private async Task<(Stream, string)> OpenFileDialog()
        {
            Stream stream = null;
            string fileName = string.Empty;
            try
            {
                PickOptions options = new PickOptions()
                {
                    PickerTitle = "Please select a CSV file"
                };
                var res = await FilePicker.PickAsync(options);
                if (res != null)
                {
                    fileName = res.FileName;
                    stream = await res.OpenReadAsync();
                }
            }
            catch (Exception ex)
            {
                await exceptionRecordService.AddAsync(new Domain.Models.ExceptionRecord()
                {
                    Created = DateTime.Now,
                    DeviceInfo = CollNotesSettings.DeviceInfo,
                    ExceptionInfo = ex.Message
                });
            }
            return (stream, fileName);
        }
    }
}