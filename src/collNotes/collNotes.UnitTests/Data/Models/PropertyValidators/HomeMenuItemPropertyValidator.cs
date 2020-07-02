using collNotes.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class HomeMenuItemPropertyValidator
    {
        [Fact]
        public void HomeMenuItem_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "Id",
                "Title"
            };
            // act
            var isValid = ModelClassValidator.IsValid(typeof(HomeMenuItem), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}
