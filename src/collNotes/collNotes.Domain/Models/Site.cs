using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace collNotes.Domain.Models
{
    public class Site : INotifyPropertyChanged
    {
        [Key]
        public int SiteID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CoordinateUncertaintyInMeters { get; set; }
        public string MinimumElevationInMeters { get; set; }
        public string Locality { get; set; }
        public string Habitat { get; set; }
        public string AssociatedTaxa { get; set; }
        public string LocationNotes { get; set; }
        public int AssociatedTripNumber { get; set; }
        private string _AssociatedTripName;
        public string AssociatedTripName
        {
            get { return _AssociatedTripName; }
            set
            {
                _AssociatedTripName = value;
                OnPropertyChanged(nameof(AssociatedTripName));
            }
        }
        public int SiteNumber { get; set; }
        private string _SiteName;
        public string SiteName
        {
            get { return _SiteName; }
            set
            {
                _SiteName = value;
                OnPropertyChanged(nameof(SiteName));
            }
        }
        public string PhotoAsBase64 { get; set; }

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