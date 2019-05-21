using NUnit.Framework;
using System;
using collnotes;
using collnotes.Data;

namespace collnotestests
{
    [TestFixture]
    public class TestProject
    {
        [Test]
        public void ValidateProjectProperties()
        {
            bool foundNewProperty = false;

            foreach (var p in typeof(Project).GetProperties())
            {
                if (!(p.Name.Equals("CreatedDate") ||
                    p.Name.Equals("PrimaryCollector") ||
                    p.Name.Equals("ProjectName") ||
                    p.Name.Equals("RecordNo")))
                {
                    foundNewProperty = true;
                }
            }

            Assert.IsFalse(foundNewProperty);
        }

        [Test]
        public void ValidateTripProperties()
        {
            bool foundNewProperty = false;

            foreach (var p in typeof(Project).GetProperties())
            {
                if (!(p.Name.Equals("AdditionalCollectors") ||
                    p.Name.Equals("CollectionDate") ||
                    p.Name.Equals("ProjectName") ||
                    p.Name.Equals("RecordNo") ||
                    p.Name.Equals("TripName")))
                {
                    foundNewProperty = true;
                }
            }

            Assert.IsFalse(foundNewProperty);
        }

        [Test]
        public void ValidateSiteProperties()
        {
            bool foundNewProperty = false;

            foreach (var p in typeof(Project).GetProperties())
            {
                if (!(p.Name.Equals("AssociatedTaxa") ||
                    p.Name.Equals("GPSCoordinates") ||
                    p.Name.Equals("Habitat") ||
                    p.Name.Equals("Locality") ||
                    p.Name.Equals("LocationNotes") ||
                    p.Name.Equals("RecordNo") ||
                    p.Name.Equals("SiteName") ||
                    p.Name.Equals("TripName")))
                {
                    foundNewProperty = true;
                }
            }

            Assert.IsFalse(foundNewProperty);
        }

        [Test]
        public void ValidateSpecimenProperties()
        {
            bool foundNewProperty = false;

            foreach (var p in typeof(Project).GetProperties())
            {
                if (!(p.Name.Equals("AdditionalInfo") ||
                    p.Name.Equals("Cultivated") ||
                    p.Name.Equals("FieldIdentification") ||
                    p.Name.Equals("GPSCoordinates") ||
                    p.Name.Equals("IndividualCount") ||
                    p.Name.Equals("LifeStage") ||
                    p.Name.Equals("OccurrenceNotes") ||
                    p.Name.Equals("RecordNo") ||
                    p.Name.Equals("SiteName") ||
                    p.Name.Equals("SpecimenName") ||
                    p.Name.Equals("SpecimenNumber") ||
                    p.Name.Equals("Substrate")))
                {
                    foundNewProperty = true;
                }
            }

            Assert.IsFalse(foundNewProperty);
        }
    }
}
