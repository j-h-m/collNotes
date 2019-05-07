using System;
using System.IO;
using Xamarin.Essentials;

namespace collnotes.Data
{
    /// <summary>
    /// App variables.
    /// </summary>
    public static class AppVariables
    {
        public static int CollectionCount { get; set; }
        public static string DataExportFormat { get; set; }
        public static string CollectorName { get; set; }
        public static string LastProject { get; set; }
        public static string ExportTypeIOS { get; set; }
    }

    /// <summary>
    /// App variables file.
    /// </summary>
    public static class AppVarsFile
    {
        static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "appvars.txt");

        /// <summary>
        /// Writes the app variables.
        /// </summary>
        public static void WriteAppVars()
        {
            string vars = "";
            if (AppVariables.CollectorName != null)
            {
                vars += "CN-" + AppVariables.CollectorName + "|";
            }
            if (AppVariables.CollectionCount != 0)
            {
                vars += "CC-" + AppVariables.CollectionCount.ToString() + "|";
            }
            if (AppVariables.DataExportFormat != null)
            {
                vars += "DF-" + AppVariables.DataExportFormat + "|";
            }
            if (AppVariables.LastProject != null)
            {
                vars += "LP-" + AppVariables.LastProject + "|";
            }
            if (AppVariables.ExportTypeIOS != null && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                vars += "ET-" + AppVariables.ExportTypeIOS + "|";
            }

            string[] varSplit = vars.Split('|');

            if (varSplit.Length > 0)
            {
                File.WriteAllText(filePath, vars);
            }
        }

        /// <summary>
        /// Reads the app variables.
        /// </summary>
        /// <returns><c>true</c>, if app variables was  read, <c>false</c> otherwise.</returns>
        public static bool ReadAppVars()
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            else
            {
                string vars = File.ReadAllText(filePath);
                string[] varSplit = vars.Split('|');

                if (varSplit.Length > 0) {
                    foreach (var item in varSplit)
                    {
                        if (item.Equals(""))
                        {
                            break;
                        }
                        switch (item.Substring(0, 2))
                        {
                            case "CN":
                                AppVariables.CollectorName = item.Substring(3);
                                break;
                            case "CC":
                                AppVariables.CollectionCount = int.Parse(item.Substring(3));
                                break;
                            case "DF":
                                AppVariables.DataExportFormat = item.Substring(3);
                                break;
                            case "LP":
                                AppVariables.LastProject = item.Substring(3);
                                break;
                            case "ET":
                                if (DeviceInfo.Platform == DevicePlatform.iOS)
                                {
                                    AppVariables.ExportTypeIOS = item.Substring(3);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                return true;
            }
        }
    }
}