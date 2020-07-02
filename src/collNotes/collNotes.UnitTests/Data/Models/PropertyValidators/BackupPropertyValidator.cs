using collNotes.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class BackupPropertyValidator
    {
        [Fact]
        public void Backup_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "Trips",
                "Sites",
                "Specimen",
                "Settings",
                "ExceptionRecords"
            };
            // act
            var isValid = ModelClassValidator.IsValid(typeof(Backup), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
