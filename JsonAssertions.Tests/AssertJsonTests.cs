using NUnit.Framework;

namespace JsonAssertions.Tests
{
    [TestFixture]
    public class AssertJsonTests
    {
        [Test]
        public void AreEquals_EqualJsonStrings_Success()
        {
            AssertJson.AreEquals("{name:'value'}", "{name:'value'}");
        }

        [Test]
        public void AreEquals_UnequalPropertyValues_Fail()
        {
            const string expectedObject = "{prop1:'value1', name:'value'}";
            const string actualObject = "{prop1:'value1', name:'value2'}";

            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(string.Format("Property \"{0}\" does not match. " +
                                          "Expected: {1}. " +
                                          "But was: {2}", "name", "value", "value2"), assertionException.Message);
        }

        [Test]
        public void AreEquals_UnequalIntPropertyValues_Fail()
        {
            const string expectedObject = "{prop1:'value1', int:15}";
            const string actualObject = "{prop1:'value1', int:16}";

            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(string.Format("Property \"{0}\" does not match. " +
                                          "Expected: {1}. " +
                                          "But was: {2}", "int", "15", "16"), assertionException.Message);
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

        [Test]
        public void AreEquals_NestedObjects_Fail()
        {
            const string expectedObject = "{prop1:'value1', nested0:{}, nested: {key:'value'}}";
            const string actualObject = "{prop1:'value1', nested0:{}, nested: {key:'value2'}}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));

            Assert.AreEqual(string.Format("Property \"{0}\" does not match. " +
                                          "Expected: {1}. " +
                                          "But was: {2}", "nested.key", "value", "value2"), assertionException.Message);
        }

        [Test]
        public void AreEquals_NestedArraysDifferentLength_Fail()
        {
            const string expectedObject = "{prop1:'value1', arr:[1, '2', {key: 'value'}, 5]}";
            const string actualObject = "{prop1:'value1', arr:[1, '3', {key: 'value'}]}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));
            Assert.AreEqual(
                string.Format("Different arrays length at {0}. Expected: {1}, but was: {2}", "arr", "4", "3"),
                assertionException.Message);
        }

        [Test]
        public void AreEquals_NestedArrays_Fail()
        {
            const string expectedObject = "{prop1:'value1', arr:[1, '2', {key: 'value'}]}";
            const string actualObject = "{prop1:'value1', arr:[1, '3', {key: 'value'}]}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));
            Assert.AreEqual(string.Format("Property \"{0}\" does not match. " +
                                          "Expected: {1}. " +
                                          "But was: {2}", "arr[1]", "2", "3"), assertionException.Message);
        }

        [Test]
        public void AreEquals_NestedArraysDiffObjects_Fail()
        {
            const string expectedObject = "{prop1:'value1', arr:[1, '2', {key: 'value2'}]}";
            const string actualObject = "{prop1:'value1', arr:[1, '2', {key: 'value'}]}";
            var assertionException =
                Assert.Throws<AssertionException>(() => AssertJson.AreEquals(expectedObject, actualObject));
            Assert.AreEqual(string.Format("Property \"{0}\" does not match. " +
                                          "Expected: {1}. " +
                                          "But was: {2}", "arr[2].key", "value2", "value"), assertionException.Message);
        }
    }
}