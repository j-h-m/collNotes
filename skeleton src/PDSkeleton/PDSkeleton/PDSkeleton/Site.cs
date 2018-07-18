using System;
using System.Collections.Generic;
using System.Text;

namespace PDSkeleton
{
    public class Site
    {
        private List<Specimen> specimen = new List<Specimen>();
        public List<Specimen> Specimen { get; set; }
        private Dictionary<string, double> siteGPS = new Dictionary<string, double>(); // add longitude and latitude like this: (key) "longitude" -> (value) 3.14159
        public Dictionary<string, double> SiteGPS { get; set; }
        private string locality = "";
        public string Locality { get; set; }
        private string habitat = "";
        public string Habitat { get; set; }
        private string associatedTaxa = "";
        public string AssociatedTaxa { get; set; }
        private string locationNotes = "";
        public string LocationNotes { get; set; }
        private int tripNumber = 0;
        public int TripNumber { get; set; }
        private int myRecordNo = 0;
        public int MyRecordNo { get; set; }
        public Plugin.Media.Abstractions.MediaFile photo;

    }
}
