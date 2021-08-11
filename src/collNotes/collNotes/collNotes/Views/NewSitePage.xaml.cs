using System;
using System.Linq;
using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.Camera;
using collNotes.DeviceServices.Geolocation;
using collNotes.DeviceServices.Permissions;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;
using static collNotes.DeviceServices.Permissions.PermissionsService;

namespace collNotes.Views
{
    public partial class NewSitePage : ContentPage
    {
        private readonly NewSiteViewModel viewModel;
        private Location CurrentLocation { get; set; }
        private Xamarin.Forms.Maps.Map Map { get; set; }
        private const double DEGREES = 0.01;

        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);
        private readonly IGeoLocationService geoLocationService =
            DependencyService.Get<IGeoLocationService>(DependencyFetchTarget.NewInstance);
        private readonly ICameraService cameraService =
            DependencyService.Get<ICameraService>(DependencyFetchTarget.NewInstance);

        public NewSitePage(NewSiteViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            if (viewModel.IsClone)
                AssociatedTripSelector.Text = viewModel.Site.AssociatedTripName;
        }

        private async void Location_Clicked(object sender, EventArgs e)
        {
            if (await viewModel.CheckOrRequestPermission(PermissionName.Location))
            {
                var dialogConfig = await xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Getting your location.",
                    configuration: dialogConfig))
                {
                    CurrentLocation = await geoLocationService.GetCurrentLocation();
                }

#if DEBUG
                if (DeviceInfo.DeviceType == DeviceType.Virtual &&
                    DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // mock with home location on iOS emulator
                    // iOS emulator does not support GPS emulation
                    CurrentLocation = new Location()
                    {
                        Latitude = 34.72247,
                        Longitude = -85.28398666,
                        Altitude = 0,
                        Accuracy = 20
                    };
                }
#endif

                if (CurrentLocation is null)
                {
                    // location was not determined for some reason
                    await MaterialDialog.Instance.AlertAsync("Your current location wasn't found, is GPS enabled on your device?");
                }
                else
                {
                    UpdateCurrentLocation();

                    var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                    await MaterialDialog.Instance.SnackbarAsync(message: "Touch icon to view and refine location information.",
                                                actionButtonText: "OK",
                                                msDuration: MaterialSnackbar.DurationLong,
                                                configuration: snackbarConfig);

                    LocationStatusButton.IsVisible = true;
                }
            }
            else
            {
                // going to need permission!
            }
        }

        private async void TakePicture_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Site.SiteName))
            {
                var alertConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync(message: "A photo requires a Site Name",
                    configuration: alertConfig);
                return;
            }
            else
            {
                if (await viewModel.CheckOrRequestPermission(PermissionName.Camera))
                {
                    var photoAsBase64 = await cameraService.TakePicture(viewModel.Site.SiteName);
                    if (!string.IsNullOrEmpty(photoAsBase64))
                    {
                        viewModel.Site.PhotoAsBase64 = photoAsBase64;
                    }
                }
                else
                {
                    // going to need permission!
                }
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var alertConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();

            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Site.SiteName))
            {
                await MaterialDialog.Instance.AlertAsync(message: "Site must have a name!",
                    configuration: alertConfig);
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.AssociatedTripName))
            {
                await MaterialDialog.Instance.AlertAsync(message: "A Site must be associated with a Trip!",
                    configuration: alertConfig);
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.Site.Longitude) || string.IsNullOrEmpty(viewModel.Site.Latitude))
            {
                await MaterialDialog.Instance.AlertAsync(message: "Please set GPS location for site before saving!",
                    configuration: alertConfig);
                return;
            }

            viewModel.Site.AssociatedTripName = viewModel.AssociatedTripName;

            var associatedTrip = viewModel.AssociableTrips.First(t => t.TripName == viewModel.AssociatedTripName);
            viewModel.Site.AssociatedTripNumber = associatedTrip.TripNumber;

            await viewModel.siteService.CreateAsync(viewModel.Site);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void CheckLocation_Clicked(object sender, EventArgs e)
        {
            Position copyCurrentLocation = new Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
            MapSpan mapSpan = new MapSpan(copyCurrentLocation, DEGREES, DEGREES);

            Pin pin = new Pin()
            {
                Label = "Site Location" + Environment.NewLine +
                        $"Lat: {CurrentLocation.Latitude}" + Environment.NewLine +
                        $"Long: {CurrentLocation.Longitude}",
                Type = PinType.Generic,
                Position = copyCurrentLocation
            };

            Map = new Xamarin.Forms.Maps.Map(mapSpan)
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 300.0,
                WidthRequest = 300.0
            };
            Map.Pins.Add(pin);
            Map.MapClicked += View_MapClicked;

            var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
            var result = await MaterialDialog.Instance.ShowCustomContentAsync(Map, "Change Location", null, "Update", "Cancel",
                configuration: alertDialogConfig);
            if (result == true && Map.Pins.Count == 1)
            {
                CurrentLocation = new Location()
                {
                    Latitude = Map.Pins[0].Position.Latitude,
                    Longitude = Map.Pins[0].Position.Longitude
                };
                UpdateCurrentLocation();
            }
        }

        private void View_MapClicked(object sender, MapClickedEventArgs e)
        {
            Map.Pins.Clear();

            Map.Pins.Add(new Pin()
            {
                Label = "Site Location" + Environment.NewLine +
                        $"Lat: {e.Position.Latitude}" + Environment.NewLine +
                        $"Long: {e.Position.Longitude}",
                Type = PinType.Generic,
                Position = e.Position
            });
        }

        private void UpdateCurrentLocation()
        {
            // location was determined successfully
            viewModel.Site.Latitude = CurrentLocation.Latitude.ToString();
            latLbl.Text = $"Latitude: {CurrentLocation.Latitude.ToString()}";
            viewModel.Site.Longitude = CurrentLocation.Longitude.ToString();
            lngLbl.Text = $"Longitude: {CurrentLocation.Longitude.ToString()}";
            viewModel.Site.MinimumElevationInMeters = CurrentLocation.Altitude.ToString();
            altLbl.Text = $"Altitude: {CurrentLocation.Altitude.ToString()}";
            viewModel.Site.CoordinateUncertaintyInMeters = CurrentLocation.Accuracy.ToString();
            accLbl.Text = $"Accuracy: {CurrentLocation.Accuracy.ToString()}";
        }

        private void LocationStatusButton_Clicked(object sender, EventArgs e)
        {
            gpsInfoCard.IsVisible = !gpsInfoCard.IsVisible;
            GetLocation_Button.IsEnabled = !GetLocation_Button.IsEnabled;
        }
    }
}