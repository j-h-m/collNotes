using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.MenuPage;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace collNotes.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        private IAppThemeService appThemeService;
        private MenuPageService menuPageService;

        public MenuPageViewModel()
        {
            menuPageService = new MenuPageService();
            ISettingService settingService = new SettingService(Context);
            IExceptionRecordService exceptionRecordService = new ExceptionRecordService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);

            var savedTheme = appThemeService.GetSavedTheme().Result;
            var result = appThemeService.SetAppTheme(savedTheme).Result;
        }

        public List<HomeMenuItem> GetMenuItems()
        {
            return menuPageService.GetMenuItems();
        }
    }
}
