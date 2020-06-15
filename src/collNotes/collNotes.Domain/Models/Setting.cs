using System;
using System.ComponentModel.DataAnnotations;

namespace collNotes.Domain.Models
{
    public class Setting
    {
        [Key]
        public int SettingID { get; set; }

        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public DateTime LastSaved { get; set; }
    }
}