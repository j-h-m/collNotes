using NUnit.Framework;
using System;
using collnotes;

namespace collnotestests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestCase()
        {
            bool result = false;
            Assert.IsTrue(result, "this test will fail");
        }
    }
}
