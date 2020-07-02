using System;
using System.Collections.Generic;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public static class ModelClassValidator
    {
        public static bool IsValid(Type t, List<string> propertyNames)
        {
            bool result = false;

            foreach (var name in propertyNames)
            {
                if (t.GetProperty(name) is null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
