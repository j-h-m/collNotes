using System;
using Plugin.Media.Abstractions;

namespace PDSkeleton
{
    public class Trip
    {
        public string PrimaryCollector { get; set; }
        public string AdditionalCollectors { get; set; }
        public DateTime CollectionDate { get; set; }
        public MediaFile GroupPhoto { get; set; }
    }
}
