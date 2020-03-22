using collNotes.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class NewSitePage : ContentPage
    {
        private readonly NewSiteViewModel viewModel;
        private Location CurrentLocation { get; set; }
        private Xamarin.Forms.Maps.Map Map { get; set; }
        private const double DEGREES = 0.01;

        public NewSitePage(NewSiteViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            if (viewModel.IsClone)
                AssociatedTripSelector.Text = viewModel.Site.AssociatedTripName;
        }

        private async void Location_Clicked(object sender, EventArgs e)
        {
            if (!await viewModel.permissionsService.CheckLocationPermission())
                await viewModel.permissionsService.RequestLocationPermission();

            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Getting your location."))
            {
                CurrentLocation = await viewModel.geoLocationService.GetCurrentLocation(viewModel.exceptionRecordService);
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

                await MaterialDialog.Instance.SnackbarAsync(message: "Touch icon to view and refine location information.",
                                            actionButtonText: "OK",
                                            msDuration: MaterialSnackbar.DurationLong);

                LocationStatusChip.IsVisible = true;
            }
        }

        private async void TakePicture_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Site.SiteName))
            {
                await MaterialDialog.Instance.AlertAsync("A photo requires a Site Name");
                return;
            }
            else
            {
                if (!await viewModel.permissionsService.CheckCameraPermission())
                    await viewModel.permissionsService.RequestCameraPermission();

                var photoAsBase64 = await viewModel.cameraService.TakePicture(viewModel.exceptionRecordService, viewModel.Site.SiteName);
                if (!string.IsNullOrEmpty(photoAsBase64))
                {
                    viewModel.Site.PhotoAsBase64 = photoAsBase64;
                }
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Site.SiteName))
            {
                await MaterialDialog.Instance.AlertAsync("Site must have a name!");
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.AssociatedTripName))
            {
                await MaterialDialog.Instance.AlertAsync("A Site must be associated with a Trip!");
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.Site.Longitude) || string.IsNullOrEmpty(viewModel.Site.Latitude))
            {
                await MaterialDialog.Instance.AlertAsync("Please set GPS location for site before saving!");
                return;
            }

            viewModel.Site.AssociatedTripName = viewModel.AssociatedTripName;
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

            var result = await MaterialDialog.Instance.ShowCustomContentAsync(Map, "Change Location", null, "Update", "Cancel");
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

        private void LocationStatusChip_ActionImageTapped(object sender, EventArgs e)
        {
            gpsInfoCard.IsVisible = !gpsInfoCard.IsVisible;
            GetLocation_Button.IsEnabled = !GetLocation_Button.IsEnabled;
            LocationStatusChip.Text = gpsInfoCard.IsVisible ? "Touch icon to close" : "Touch icon to open";
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
    }
}