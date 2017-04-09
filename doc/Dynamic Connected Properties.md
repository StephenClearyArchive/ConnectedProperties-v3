Connected properties are dynamic by nature, and it's sometimes useful to have connected properties of type **dynamic**.

Named properties are always **dynamic**.

Implicit connectors have a "default TValue type" of **dynamic**, as demonstrated by this code:

{code:C#}
internal struct DynamicPropertyTag {}

public static void Test(object obj)
{
  // Set the property to be an integer and overwrite it with a string.
  obj.GetConnectedProperty<DynamicPropertyTag>().Set(13);
  obj.GetConnectedProperty<DynamicPropertyTag>().Set("Bob");

  // Even more dynamic fun: set the property to be an ExpandoObject.
  obj.GetConnectedProperty<DynamicPropertyTag>().Set(new ExpandoObject());
  obj.GetConnectedProperty<DynamicPropertyTag>().Get().NewProperty = 13;
  obj.GetConnectedProperty<DynamicPropertyTag>().Get().Name = "Bob";
}
{code:C#}

The code example above sets the value to an instance of ExpandoObject, which is commonly used with the **dynamic** type. However, note that thread safety of the _value_ is your responsibility.

:**Reminder:** The Connected Properties API (e.g., {{Get}}) is thread-safe, but the thread-safety of the _values_ is your responsibility. In the example above, the {{ExpandoObject}} instance is not thread-safe.

Explicit connectors do not have a "default TValue type", but you may specify **dynamic** for {{TValue}}.
