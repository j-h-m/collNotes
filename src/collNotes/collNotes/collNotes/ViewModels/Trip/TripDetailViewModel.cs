using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using System;

namespace collNotes.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        public Trip Trip { get; set; }

        private readonly IExceptionRecordService exceptionRecordService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;

        public readonly TripService tripService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        public TripDetailViewModel(Trip trip)
        {
            tripService = new TripService(Context);
            settingService = new SettingService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            Trip = trip;
            Title = Trip?.TripName;
        }
    }
}