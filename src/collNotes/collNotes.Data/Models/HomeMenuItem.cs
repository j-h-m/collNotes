using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace collNotes.Data.Models
{
    public enum MenuItemType
    {
        About,
        Settings,
        ExportImport,
        Trips,
        Sites,
        Specimen
    }

    public class HomeMenuItem : INotifyPropertyChanged
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        private Color _BackgroundColor;
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set 
            {
                _BackgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor)); 
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