using collNotes.Data.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class SitePropertyValidator
    {
        [Fact]
        public void Site_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "SiteID",
                "Latitude",
                "Longitude",
                "CoordinateUncertaintyInMeters",
                "MinimumElevationInMeters",
                "Locality",
                "Habitat",
                "AssociatedTaxa",
                "LocationNotes",
                "AssociatedTripName",
                "SiteNumber",
                "SiteName",
                "PhotoAsBase64"
            };
            // act
            var isValid = ModelClassValidator.ClassValidator(typeof(Site), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
