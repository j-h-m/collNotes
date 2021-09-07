using collNotes.ColorThemes;
using collNotes.DeviceServices.AppTheme;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs.Configurations;
using static collNotes.Settings.CollNotesSettings;

namespace collNotes.ColorThemes.ConfigFactory
{
    public class XfMaterialColorConfigFactory
    {
        private readonly IAppThemeService appThemeService =
            DependencyService.Get<IAppThemeService>(DependencyFetchTarget.NewInstance);

        public XfMaterialColorConfigFactory() { }

        public async Task<MaterialAlertDialogConfiguration> GetAlertDialogConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialAlertDialogConfiguration materialAlertDialogConfiguration;

            if (currentTheme == ColorTheme.HighContrast)
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = HighContrast.PageBackgroundColor,
                    MessageTextColor = HighContrast.PrimaryTextColor,
                    TitleTextColor = HighContrast.PrimaryTextColor,
                    TintColor = HighContrast.SecondaryColor
                };
            }
            else // use default
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = Light.PageBackgroundColor,
                    MessageTextColor = Light.PrimaryTextColor,
                    TitleTextColor = Light.PrimaryTextColor,
                    TintColor = Light.SecondaryColor
                };
            }

            return materialAlertDialogConfiguration;
        }

        public async Task<MaterialSnackbarConfiguration> GetSnackbarConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialSnackbarConfiguration materialSnackbarConfiguration;

            if (currentTheme == ColorTheme.HighContrast)
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = HighContrast.PageBackgroundColor,
                    MessageTextColor = HighContrast.PrimaryTextColor,
                    TintColor = HighContrast.SecondaryColor
                };
            }
            else // use default
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = Light.PageBackgroundColor,
                    MessageTextColor = Light.PrimaryTextColor,
                    TintColor = Light.SecondaryColor
                };
            }

            return materialSnackbarConfiguration;
        }

        public async Task<MaterialLoadingDialogConfiguration> GetLoadingDialogConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialLoadingDialogConfiguration materialLoadingDialogConfiguration;

            if (currentTheme == ColorTheme.HighContrast)
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = HighContrast.PageBackgroundColor,
                    MessageTextColor = HighContrast.PrimaryTextColor,
                    TintColor = HighContrast.SecondaryColor
                };
            }
            else // use default
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = Light.PageBackgroundColor,
                    MessageTextColor = Light.PrimaryTextColor,
                    TintColor = Light.SecondaryColor
                };
            }

            return materialLoadingDialogConfiguration;
        }

        public async Task<MaterialConfirmationDialogConfiguration> GetConfirmationDialogConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialConfirmationDialogConfiguration materialConfirmationDialogConfiguration;

            if (currentTheme == ColorTheme.HighContrast)
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = HighContrast.PageBackgroundColor,
                    TextColor = HighContrast.PrimaryTextColor,
                    TitleTextColor = HighContrast.PrimaryTextColor,
                    TintColor = HighContrast.SecondaryColor,
                    ControlSelectedColor = HighContrast.SecondaryColor,
                    ControlUnselectedColor = HighContrast.SecondaryColorDark
                };
            }
            else // use default
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = Light.PageBackgroundColor,
                    TextColor = Light.PrimaryTextColor,
                    TitleTextColor = Light.PrimaryTextColor,
                    TintColor = Light.SecondaryColor,
                    ControlSelectedColor = Light.SecondaryColor,
                    ControlUnselectedColor = Light.SecondaryColorDark
                };
            }

            return materialConfirmationDialogConfiguration;
        }
    }
}
