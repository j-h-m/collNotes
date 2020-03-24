using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace collNotes.Settings.AutoComplete
{
    public static class AutoCompleteSettings
    {
        private static List<string> FungiList { get; set; }
        private static Dictionary<string, List<string>> FungiDict { get; set; }
        private static List<string> PlantaeList { get; set; }
        private static Dictionary<string, List<string>> PlantaeDict { get; set; }

        private const string PlantaeRefLocation = "collNotes.EmbeddedResources.plantae_ref.txt";
        private const string FungiRefLocation = "collNotes.EmbeddedResources.fungi_ref.txt";

        public static Dictionary<string, List<string>> GetAutoCompleteData(string autoCompleteOption)
        {
            if (autoCompleteOption is null)
                throw new ArgumentNullException(nameof(autoCompleteOption));

            if (autoCompleteOption.Equals("Plantae"))
                return GetPlantaeData();

            if (autoCompleteOption.Equals("Fungi"))
                return GetFungiData();

            // default is Plantae
            return GetPlantaeData();
        }

        private static Dictionary<string, List<string>> GetPlantaeData()
        {
            PlantaeList = new List<string>();
            PlantaeDict = new Dictionary<string, List<string>>();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AutoCompleteSettings)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(PlantaeRefLocation);
            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    PlantaeList.Add(reader.ReadLine());
                }
            }

            var result = PlantaeList.GroupBy(g => g.Substring(0, 3)).ToList();

            foreach (var item in result)
            {
                PlantaeDict.Add(item.Key, item.ToList());
            }

            return PlantaeDict;
        }

        private static Dictionary<string, List<string>> GetFungiData()
        {
            FungiList = new List<string>();
            FungiDict = new Dictionary<string, List<string>>();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AutoCompleteSettings)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(FungiRefLocation);
            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    FungiList.Add(reader.ReadLine());
                }
            }

            var result = FungiList.GroupBy(g => (g.Length >= 3) ? g.Substring(0, 3) : g).ToList();

            foreach (var item in result)
            {
                FungiDict.Add(item.Key, item.ToList());
            }

            return FungiDict;
        }
    }
}