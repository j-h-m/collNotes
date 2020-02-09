using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace collNotes.Data.Models
{
    public class Trip : INotifyPropertyChanged
    {
        [Key]
        public int TripID { get; set; }

        private string _PrimaryCollector;
        public string PrimaryCollector
        {
            get { return _PrimaryCollector; }
            set
            {
                _PrimaryCollector = value;
                OnPropertyChanged(nameof(PrimaryCollector));
            }
        }
        public string AdditionalCollectors { get; set; }
        public DateTime CollectionDate { get; set; }
        public int TripNumber { get; set; }
        private string _TripName;
        public string TripName
        {
            get { return _TripName; }
            set
            {
                _TripName = value;
                OnPropertyChanged(nameof(TripName));
            }
        }

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