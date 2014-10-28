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
                var expectedJToken = expectedJObject[property.Key];
                var actualJToken = actualJObject[property.Key];
                AreEquals(expectedJToken, actualJToken);
            }
        }

        public static void AreEquals(JToken expectedJToken, JToken actualJToken)
        {
            switch (expectedJToken.Type)
            {
                case JTokenType.Array:
                    var expectedJArray = expectedJToken.Value<JArray>();
                    var actualJArray = actualJToken.Value<JArray>();
                    if (expectedJArray.Count != actualJArray.Count)
                        Assert.Fail("Different arrays length at {0}. Expected: {1}, but was: {2}",
                            expectedJToken.Path,
                            expectedJArray.Count, actualJArray.Count);
                    for (var i = 0; i < expectedJArray.Count; i++)
                    {
                        AreEquals(expectedJArray[i], actualJArray[i]);
                    }
                    break;
                case JTokenType.Object:
                    AreEquals(expectedJToken.Value<JObject>(), actualJToken.Value<JObject>());
                    break;
                default:
                    var expectedString = expectedJToken.Value<string>();
                    var actualString = actualJToken.Value<string>();
                    if (expectedString != actualString)
                        Assert.Fail("Property \"{0}\" does not match. " +
                                    "Expected: {1}. " +
                                    "But was: {2}", expectedJToken.Path, expectedString, actualString);
                    break;
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