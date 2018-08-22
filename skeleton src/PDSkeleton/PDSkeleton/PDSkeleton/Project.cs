using System;
using System.Collections.Generic;
using System.Text;

namespace PDSkeleton
{
    public class Project
    {
        public string ProjectName { get; set; }
        public string PrimaryCollector { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
