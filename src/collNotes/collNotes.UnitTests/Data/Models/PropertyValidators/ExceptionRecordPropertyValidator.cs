using collNotes.Data.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class ExceptionRecordPropertyValidator
    {
        [Fact]
        public void ExceptionRecord_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "ExceptionRecordID",
                "Created",
                "DeviceInfo",
                "ExceptionInfo"
            };
            // act
            var isValid = ModelClassValidator.IsValid(typeof(ExceptionRecord), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
