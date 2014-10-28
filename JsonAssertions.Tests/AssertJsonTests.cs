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
    }
}