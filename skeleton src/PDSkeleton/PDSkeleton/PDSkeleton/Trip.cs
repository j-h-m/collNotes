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
        // how to save image? may have to save what we get as a byte array <--- byte array is usually what you store in a database.
        // private ?Image? groupPhoto = new ?Image?
        // public ?Image? GroupPhoto {get; set; }
    }
}
