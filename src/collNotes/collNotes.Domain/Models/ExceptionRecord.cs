using System;
using System.ComponentModel.DataAnnotations;

namespace collNotes.Domain.Models
{
    public class ExceptionRecord
    {
        [Key]
        public int ExceptionRecordID { get; set; }

        public DateTime Created { get; set; }
        public string DeviceInfo { get; set; }
        public string ExceptionInfo { get; set; }
    }
}