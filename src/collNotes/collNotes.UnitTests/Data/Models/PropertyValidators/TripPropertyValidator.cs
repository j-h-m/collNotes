using collNotes.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class TripPropertyValidator
    {
        [Fact]
        public void Trip_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "TripID",
                "PrimaryCollector",
                "AdditionalCollectors",
                "CollectionDate",
                "TripNumber",
                "TripName"
            };
            // act
            var isValid = ModelClassValidator.IsValid(typeof(Trip), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
