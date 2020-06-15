using collNotes.Data.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class SpecimenPropertyValidator
    {
        [Fact]
        public void Specimen_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "SpecimenID",
                "FieldIdentification",
                "OccurrenceNotes",
                "Substrate",
                "LifeStage",
                "AdditionalInfo",
                "IndividualCount",
                "Cultivated",
                "AssociatedSiteName",
                "AssociatedSiteNumber",
                "SpecimenNumber",
                "SpecimenName",
                "PhotoAsBase64",
                "LabelString"
            };
            // act
            var isValid = ModelClassValidator.IsValid(typeof(Specimen), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
