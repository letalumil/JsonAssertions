using Newtonsoft.Json.Linq;

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
        }
    }
}