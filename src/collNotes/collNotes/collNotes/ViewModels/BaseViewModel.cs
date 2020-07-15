using collNotes.DeviceServices.Permissions;
using collNotes.Ef.Context;
using collNotes.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using static collNotes.DeviceServices.Permissions.PermissionsService;

namespace collNotes.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IPermissionsService permissionsService =
            DependencyService.Get<IPermissionsService>(DependencyFetchTarget.NewInstance);

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// Checks permission status, if granted returns true.
        /// If not granted, requests permission and returns request result.
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public async Task<bool> CheckOrRequestPermission(PermissionName permissionName)
        {
            return await permissionsService.CheckPermission(permissionName) ?
                true : await permissionsService.RequestPermission(permissionName) ?
                true : false;
                
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
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