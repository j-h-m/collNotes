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

            /*if (currentTheme == ColorTheme.ContrastDark)
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = ContrastDark.PageBackgroundColor,
                    MessageTextColor = ContrastDark.PrimaryTextColor,
                    TitleTextColor = ContrastDark.PrimaryTextColor,
                    TintColor = ContrastDark.SecondaryColor
                };
            }*/
            if (currentTheme == ColorTheme.ContrastLight)
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = ContrastLight.PageBackgroundColor,
                    MessageTextColor = ContrastLight.PrimaryTextColor,
                    TitleTextColor = ContrastLight.PrimaryTextColor,
                    TintColor = ContrastLight.SecondaryColor
                };
            }
            /*else if (currentTheme == ColorTheme.Dark)
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = Dark.PageBackgroundColor,
                    MessageTextColor = Dark.PrimaryTextColor,
                    TitleTextColor = Dark.PrimaryTextColor,
                    TintColor = Dark.SecondaryColor
                };
            }*/
            else // use default
            {
                materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                {
                    BackgroundColor = LightDefault.PageBackgroundColor,
                    MessageTextColor = LightDefault.PrimaryTextColor,
                    TitleTextColor = LightDefault.PrimaryTextColor,
                    TintColor = LightDefault.SecondaryColor
                };
            }

            return materialAlertDialogConfiguration;
        }

        public async Task<MaterialSnackbarConfiguration> GetSnackbarConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialSnackbarConfiguration materialSnackbarConfiguration;

            /*if (currentTheme == ColorTheme.ContrastDark)
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = ContrastDark.PageBackgroundColor,
                    MessageTextColor = ContrastDark.PrimaryTextColor,
                    TintColor = ContrastDark.SecondaryColor
                };
            }*/
            if (currentTheme == ColorTheme.ContrastLight)
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = ContrastLight.PageBackgroundColor,
                    MessageTextColor = ContrastLight.PrimaryTextColor,
                    TintColor = ContrastLight.SecondaryColor
                };
            }
            /*else if (currentTheme == ColorTheme.Dark)
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = Dark.PageBackgroundColor,
                    MessageTextColor = Dark.PrimaryTextColor,
                    TintColor = Dark.SecondaryColor
                };
            }*/
            else // use default
            {
                materialSnackbarConfiguration = new MaterialSnackbarConfiguration()
                {
                    BackgroundColor = LightDefault.PageBackgroundColor,
                    MessageTextColor = LightDefault.PrimaryTextColor,
                    TintColor = LightDefault.SecondaryColor
                };
            }

            return materialSnackbarConfiguration;
        }

        public async Task<MaterialLoadingDialogConfiguration> GetLoadingDialogConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialLoadingDialogConfiguration materialLoadingDialogConfiguration;

            /*if (currentTheme == ColorTheme.ContrastDark)
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = ContrastDark.PageBackgroundColor,
                    MessageTextColor = ContrastDark.PrimaryTextColor,
                    TintColor = ContrastDark.SecondaryColor
                };
            }*/
            if (currentTheme == ColorTheme.ContrastLight)
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = ContrastLight.PageBackgroundColor,
                    MessageTextColor = ContrastLight.PrimaryTextColor,
                    TintColor = ContrastLight.SecondaryColor
                };
            }
            /*else if (currentTheme == ColorTheme.Dark)
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = Dark.PageBackgroundColor,
                    MessageTextColor = Dark.PrimaryTextColor,
                    TintColor = Dark.SecondaryColor
                };
            }*/
            else // use default
            {
                materialLoadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
                {
                    BackgroundColor = LightDefault.PageBackgroundColor,
                    MessageTextColor = LightDefault.PrimaryTextColor,
                    TintColor = LightDefault.SecondaryColor
                };
            }

            return materialLoadingDialogConfiguration;
        }

        public async Task<MaterialConfirmationDialogConfiguration> GetConfirmationDialogConfiguration()
        {
            var currentTheme = await appThemeService.GetSavedTheme();
            MaterialConfirmationDialogConfiguration materialConfirmationDialogConfiguration;

            /*if (currentTheme == ColorTheme.ContrastDark)
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = ContrastDark.PageBackgroundColor,
                    TextColor = ContrastDark.PrimaryTextColor,
                    TitleTextColor = ContrastDark.PrimaryTextColor,
                    TintColor = ContrastDark.SecondaryColor,
                    ControlSelectedColor = ContrastDark.SecondaryColor,
                    ControlUnselectedColor = ContrastDark.SecondaryColorDark
                };
            }*/
            if (currentTheme == ColorTheme.ContrastLight)
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = ContrastLight.PageBackgroundColor,
                    TextColor = ContrastLight.PrimaryTextColor,
                    TitleTextColor = ContrastLight.PrimaryTextColor,
                    TintColor = ContrastLight.SecondaryColor,
                    ControlSelectedColor = ContrastLight.SecondaryColor,
                    ControlUnselectedColor = ContrastLight.SecondaryColorDark
                };
            }
            /*else if (currentTheme == ColorTheme.Dark)
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = Dark.PageBackgroundColor,
                    TextColor = Dark.PrimaryTextColor,
                    TitleTextColor = Dark.PrimaryTextColor,
                    TintColor = Dark.SecondaryColor,
                    ControlSelectedColor = Dark.SecondaryColor,
                    ControlUnselectedColor = Dark.SecondaryColorDark
                };
            }*/
            else // use default
            {
                materialConfirmationDialogConfiguration = new MaterialConfirmationDialogConfiguration()
                {
                    BackgroundColor = LightDefault.PageBackgroundColor,
                    TextColor = LightDefault.PrimaryTextColor,
                    TitleTextColor = LightDefault.PrimaryTextColor,
                    TintColor = LightDefault.SecondaryColor,
                    ControlSelectedColor = LightDefault.SecondaryColor,
                    ControlUnselectedColor = LightDefault.SecondaryColorDark
                };
            }

            return materialConfirmationDialogConfiguration;
        }
    }
}
