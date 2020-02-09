using System.Globalization;
using CsvHelper.Configuration;

namespace collNotes.CsvHelperMaps.DarwinCoreFormat
{
    public sealed class DarwinCoreMap : ClassMap<DarwinCore>
    {
        public DarwinCoreMap()
        {
            AutoMap(CultureInfo.CurrentCulture);
            Map(m => m.labelProject).Name("Label Project");
        }
    }
}