using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.Connectivity;
using collNotes.Domain.Models;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Settings;
using collNotes.ShareFolderInterface;
using collNotes.ViewModels;
/*using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;*/
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportImportPage : TabbedPage
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
            BindingContext = this.viewModel = new ExportImportViewModel();

            Title = GetTitle(string.Empty);

            CurrentPageChanged += ExportImportPage_CurrentPageChanged;
        }

        private void ExportImportPage_CurrentPageChanged(object sender, System.EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;

            if (tabbedPage.CurrentPage == ImportContentPage)
                Title = GetTitle("Import");

            if (tabbedPage.CurrentPage == ExportContentPage)
                Title = GetTitle("Export");
        }

        private async void ImportTrip_Clicked(object sender, System.EventArgs e)
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

        private async void ExportTrip_Clicked(object sender, System.EventArgs e)
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
                    var result = await MaterialDialog.Instance.SelectChoiceAsync("Select a trip", choices,
                        configuration: confirmationDialogConfig);

                    string message = string.Empty;

                    if (result != -1)
                    {
                        var selectedTrip = trips.ToArray()[result];
                        if (await viewModel.ExportTrip(selectedTrip, GetFilePath(selectedTrip.TripName + ".csv")))
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

        private async void ImportBackup_Clicked(object sender, System.EventArgs e)
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

        private async void ExportBackup_Clicked(object sender, System.EventArgs e)
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var netStatus = connectivityService.GetNetworkStatus();
            viewModel.IsConnectionAvailable = netStatus == ConnectivityService.ActualConnectivity.Connected;
            viewModel.ShowConnectionErrorMsg = !viewModel.IsConnectionAvailable;
        }

        private string GetTitle(string currentPageName)
        {
            return $"{currentPageName} Collection Data";
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