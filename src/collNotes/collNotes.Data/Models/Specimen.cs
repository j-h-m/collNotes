using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace collNotes.Data.Models
{
    public class Specimen : INotifyPropertyChanged
    {
        [Key]
        public int SpecimenID { get; set; }

        public string FieldIdentification { get; set; }
        public string OccurrenceNotes { get; set; }
        public string Substrate { get; set; }
        public string LifeStage { get; set; }
        public string AdditionalInfo { get; set; }
        public string IndividualCount { get; set; }
        public bool Cultivated { get; set; }
        public string AssociatedSiteName { get; set; }
        public int AssociatedSiteNumber { get; set; }
        public int SpecimenNumber { get; set; }
        private string _SpecimenName;
        public string SpecimenName
        {
            get { return _SpecimenName; }
            set
            {
                _SpecimenName = value;
                OnPropertyChanged(nameof(SpecimenName));
            }
        }
        public string PhotoAsBase64 { get; set; }

        [NotMapped]
        public string LabelString
        {
            get { return _LabelString; }
            set
            {
                _LabelString = value;
                OnPropertyChanged(nameof(LabelString));
            }
        }
        private string _LabelString;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}