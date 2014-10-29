Example:

```cs
string expectedObject = "{prop1:'value1', name:'value'}";
string actualObject = "{prop1:'value1', name:'value2'}";

// Assertion will throw AssertionException with following message:
// Property "name" does not match. Expected: value. But was: value2
AssertJson.AreEquals(expectedObject, actualObject);
```
