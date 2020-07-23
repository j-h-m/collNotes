using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace collNotes.Domain.Models
{
    public class ImportRecord
    {
        [Key]
        public int ImportID { get; set; }
        public string FileName { get; set; }
        public DateTime Created { get; set; }
    }
}
