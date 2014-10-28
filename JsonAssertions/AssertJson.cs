using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonAssertions
{
    public static class AssertJson
    {
        public static void AreEquals(string expectedJson, string actualJson)
        {
            var expectedJObject = JObject.Parse(expectedJson);
            var actualJObject = JObject.Parse(actualJson);
            AreEquals(expectedJObject, actualJObject);
        }

        public static void AreEquals(JObject expectedJObject, JObject actualJObject)
        {
            if (JToken.DeepEquals(expectedJObject, actualJObject))
                return;

            EqualNumberOfProperties(expectedJObject, actualJObject);

            foreach (var property in expectedJObject)
            {
                if (actualJObject[property.Key] != expectedJObject[property.Key])
                    Assert.Fail("Property \"{0}\" does not match\r\n" +
                                "Expected: {1}\r\n" +
                                "But was: {2}", property.Key, property.Value, actualJObject[property.Key]);
            }
        }

        public static void EqualNumberOfProperties(JObject expectedJObject, JObject actualJObject)
        {
            var expectedProperties = expectedJObject.Children<JProperty>().Select(property => property.Name).ToList();
            var actualProperties = actualJObject.Children<JProperty>().Select(property => property.Name).ToList();

            var missingProperties = expectedProperties.Except(actualProperties).ToList();
            var excessProperties = actualProperties.Except(expectedProperties).ToList();

            if (missingProperties.Any())
                Assert.Fail("Object {0} properties missing: {1}",
                    expectedJObject.Path,
                    string.Join(", ", missingProperties));

            if (excessProperties.Any())
                Assert.Fail("Object {0} has excess properties: {1}", expectedJObject.Path,
                    string.Join(", ", excessProperties));
        }
    }
}