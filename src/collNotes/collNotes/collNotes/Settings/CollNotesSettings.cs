using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace collNotes.Settings
{
    public static class CollNotesSettings
    {
        public const string CollectionCountKey = "collection_count";
        public const int CollectionCountDefault = 1;
        public const string CollectionCountStringDefault = "1";
        public const string PrimaryCollectorKey = "primary_collector";

        public const string ExportFormatKey = "export_format";
        public const string ExportFormatDefault = "Darwin Core";

        public const string ExportMethodKey = "export_method";
        public const string ExportMethodDefault = "Email";

        public const string AutoCompleteTypeKey = "autocomplete";
        public const string AutoCompleteDefault = "Plantae";

        public const string ColorThemeKey = "color_theme";
        public const string ColorThemeDefault = "Light [Default]";

        public const string DeviceInfoKey = "device_info";

        public static readonly string DeviceInfo = $"Manufacturer: {Xamarin.Essentials.DeviceInfo.Manufacturer}; " +
                    $"Model: {Xamarin.Essentials.DeviceInfo.Model}; " +
                    $"Name: {Xamarin.Essentials.DeviceInfo.Name}; " +
                    $"Platform: {Xamarin.Essentials.DeviceInfo.Platform}; " +
                    $"Version: {Xamarin.Essentials.DeviceInfo.VersionString}";

        public static bool IsAppStartingUp = true;

        public static List<string> LifeStages = new List<string>()
        {
            "Vegetative",
            "Reproductive",
            "Early Reproductive",
            "Peak Reproductive",
            "Late Reproductive",
            "Fruit/Seed Bearing"
        };

        public static List<string> ExportFormats = new List<string>()
        {
            "Darwin Core"
        };

        public static List<string> ExportMethods = new List<string>()
        {
            "Multi-Option Share",
            "Email"
        };

        public static List<string> AutoCompleteOptions = new List<string>()
        {
            "Fungi",
            "Plantae"
        };

        public static List<string> ColorThemes = new List<string>()
        {
            "Light",
            "High Contrast"
        };

        public static Dictionary<string, List<string>> AutoCompleteSource { get; set; }

        public enum ColorTheme
        {
            Light,
            HighContrast
        }

        public static string GetByEnum(ColorTheme colorTheme)
        {
            string themeName = string.Empty;

            switch (colorTheme)
            {
                case ColorTheme.Light:
                    themeName = ColorThemes[(int)ColorTheme.Light];
                    break;
                case ColorTheme.HighContrast:
                    themeName = ColorThemes[(int)ColorTheme.HighContrast];
                    break;
                default:
                    break;
            }

            return themeName;
        }

        public static ColorTheme GetByThemeName(string themeName)
        {
            if (ColorThemes.Contains(themeName))
            {
                ColorTheme colorTheme;
                switch (ColorThemes.IndexOf(themeName))
                {
                    case (int)ColorTheme.Light:
                        colorTheme = ColorTheme.Light;
                        break;
                    case (int)ColorTheme.HighContrast:
                        colorTheme = ColorTheme.HighContrast;
                        break;
                    default:
                        colorTheme = ColorTheme.Light;
                        break;
                }
                return colorTheme;
            }
            else
            {
                return ColorTheme.Light;
            }
        }
    }
}