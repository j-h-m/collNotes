using System;
using System.Collections.Generic;
using System.Text;

namespace PDSkeleton
{
    class Project
    {
        private string projectName = "";
        public string ProjectName { get; set; }
        private List<Trip> trips = new List<Trip>();
        public List<Trip> Trips { get; set; }
    }
}
