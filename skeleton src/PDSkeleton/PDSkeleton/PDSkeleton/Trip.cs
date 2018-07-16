using System;
using System.Collections.Generic;
using System.Text;
// using Xamarin.Forms;

namespace PDSkeleton
{
    public class Trip
    {
        private string primaryCollector = "";
        public string PrimaryCollector { get; set; }
        private string additionalCollectors = "";
        public string AdditionalCollectors { get; set; }
        private DateTime collectionDate = new DateTime();
        public DateTime CollectionDate { get; set; }
        private List<Site> sites = new List<Site>();
        public List<Site> Sites{ get; set; }
        public Plugin.Media.Abstractions.MediaFile groupPhoto;
    }
}
