using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static collNotes.Settings.CollNotesSettings;

namespace collNotes.DeviceServices.AppTheme
{
    public interface IAppThemeService
    {
        Task<ColorTheme> GetSavedTheme();
        Task<bool> SetAppTheme(ColorTheme colorTheme);
        Task<bool> SaveAppTheme(ColorTheme colorTheme);
    }
}
