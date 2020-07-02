using System.Globalization;
using CsvHelper.Configuration;

namespace collNotes.ServiceLayer.Data
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