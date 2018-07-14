using System;
using System.Collections.Generic;
using System.Text;

namespace PDSkeleton
{
    public class Specimen
    {
        private Dictionary<string, double> specimenGPS = new Dictionary<string, double>();
        public Dictionary<string, double> SpecimenGPS { get; set; }
        private string fieldID = "";
        public string FieldID { get; set; }
        private string occurrenceNotes = "";
        public string OccurrenceNotes { get; set; }
        private string substrate = "";
        public string Substrate { get; set; }
        private string lifeStage = "";
        public string LifeStage { get; set; }
        private string additionalInfo = "";
        public string AdditionalInfo { get; set; }
        private int individualCount = -1; // set this because we know it hasn't been set if it's -1 later
        public int IndividualCount { get; set; }
        private bool cultivated = false; // set as false initially
        public bool Cultivated { get; set; }
        // how to save image? may have to save what we get as a byte array <--- byte array is usually what you store in a database.
        // private ?Image? specimenImage = new ?Image?
        // public ?Image? SpecimenImage {get; set; }
    }
}
