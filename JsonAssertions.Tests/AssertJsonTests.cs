using NUnit.Framework;

namespace JsonAssertions.Tests
{
    [TestFixture]
    public class AssertJsonTests
    {
        [Test]
        public void EqualJsonStrings_Success()
        {
            AssertJson.AreEquals("{name:'value'}", "{name:'value'}");
        }

        [Test]
        public void AreEquals_UnequalPropertyValues_Fail()
        {
            const string expectedObject = "{name:'value'}";
            const string actualObject = "{name:'value2'}";

            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(assertionException.Message,
                string.Format("Property \"{0}\" does not match\r\n" +
                              "Expected: {1}\r\n" +
                              "But was: {2}", "name", "value", "value2"));
        }

        [Test]
        public void AreEquals_MissingProperty_Fail()
        {
            const string expectedObject = "{name:'value', missing:'yes'}";
            const string actualObject = "{name:'value'}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(assertionException.Message,
                string.Format("Object  properties missing: missing"));
        }

        [Test]
        public void AreEquals_ExcessProperty_Fail()
        {
            const string expectedObject = "{name:'value'}";
            const string actualObject = "{name:'value', excess:'yes'}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(assertionException.Message,
                string.Format("Object  has excess properties: excess"));
        }
    }
}