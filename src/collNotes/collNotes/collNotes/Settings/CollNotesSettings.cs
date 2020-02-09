using System.Collections.Generic;

namespace collNotes.Settings
{
    public static class CollNotesSettings
    {
        public const string CollectionCountKey = "collection_count";
        public const int CollectionCountDefault = 0;
        public const string PrimaryCollectorKey = "primary_collector";

        public const string ExportFormatKey = "export_format";
        public const string ExportFormatDefault = "Darwin Core";

        public const string ExportMethodKey = "export_method";
        public const string ExportMethodDefault = "Email";

        public const string AutoCompleteTypeKey = "autocomplete";
        public const string AutoCompleteDefault = "Plantae";

        public const string ColorThemeKey = "color_theme";
        public const string ColorThemeDefault = "Default";

        public const string DeviceInfoKey = "device_info";

        public static string DeviceInfo = $"Manufacturer: {Xamarin.Essentials.DeviceInfo.Manufacturer}; " +
                    $"Model: {Xamarin.Essentials.DeviceInfo.Model}; " +
                    $"Name: {Xamarin.Essentials.DeviceInfo.Name}; " +
                    $"Platform: {Xamarin.Essentials.DeviceInfo.Platform}; " +
                    $"Version: {Xamarin.Essentials.DeviceInfo.VersionString}";

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
            "Light [Default]",
            "Dark",
            "High Contrast Light",
            "High Contrast Dark"
        };

        public static Dictionary<string, List<string>> AutoCompleteSource { get; set; }
    }
}