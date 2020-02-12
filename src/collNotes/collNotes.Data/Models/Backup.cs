using System.Collections.Generic;

namespace collNotes.Data.Models
{
    public class Backup
    {
        public List<Trip> Trips { get; set; }
        public List<Site> Sites { get; set; }
        public List<Specimen> Specimen { get; set; }
        public List<Setting> Settings { get; set; }
        public List<ExceptionRecord> ExceptionRecords { get; set; }
    }
}