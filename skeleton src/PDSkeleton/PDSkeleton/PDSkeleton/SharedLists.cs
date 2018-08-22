using System;
using System.Collections.Generic;
using System.Text;

namespace PDSkeleton
{
    public static class SharedLists
    {
        public static List<Project> Projects { get; set; }
        public static List<Trip> Trips { get; set; }
        public static List<Site> Sites { get; set; }
        public static List<Specimen> Specimens { get; set; }
    }
}
