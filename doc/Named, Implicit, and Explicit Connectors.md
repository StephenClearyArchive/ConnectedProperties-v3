A _connector_ is responsible for connecting a property to a carrier object. It acts like a property definition.

# Named Connectors

A named connector is identified by a (string) name. There are extension methods in {{Nito.ConnectedProperties.Named}} which allow accessing named properties on any carrier object.

{code:C#}
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Named;

[TestMethod](TestMethod)
public void ConnectedPropertyMayBeRead()
{
  // Create a carrier object to connect our property to.
  object carrier = new object();

  // Set the "Line Number" property on that carrier object (the property is connected to the carrier as a side-effect).
  carrier.GetConnectedProperty("Line Number").Set(13);

  // Read the "Line Number" property from that carrier object.
  Assert.AreEqual(13, carrier.GetConnectedProperty("Line Number").Get());
}
{code:C#}

Since names are global, you should always have some kind of namespace, e.g., your company name and/or product: "Nito.CSV Reader.Line Number".

Alternatively, you can use a "tag type" to apply a namespace, like this:

{code:C#}
private struct CsvReaderProperties { };

[TestMethod](TestMethod)
public void PropertiesInNamespacesAreIndependentFromPropertiesWithoutNamespaces()
{
  object carrier = new object();
  carrier.GetConnectedProperty<CsvReaderProperties>("Line Number").Set(17);
  carrier.GetConnectedProperty("Line Number").Set(13);
  Assert.AreEqual(17, carrier.GetConnectedProperty<CsvReaderProperties>("Line Number").Get());
  Assert.AreEqual(13, carrier.GetConnectedProperty("Line Number").Get());
}
{code:C#}

# Implicit Connectors

Implicit connectors exist as a _type,_ and there are extension methods in {{Nito.ConnectedProperties.Implicit}} which allow accessing those properties on any carrier object. Part of an implicit connector's definition is a _tag type_, which is used to distinguish between different connected properties with the same value type.

The easiest way to demonstrate implicit connectors is by an example:

{code:C#}
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Implicit;

// Define a "tag" for our property definition.
private struct MyIntegerProperty { }

[TestMethod](TestMethod)
public void ConnectedPropertyMayBeRead()
{
  // Create a carrier object to connect our property to.
  object carrier = new object();

  // Set the "integer (MyIntegerProperty)" property on that carrier object (the property is connected to the carrier as a side-effect).
  carrier.GetConnectedProperty<int, MyIntegerProperty>().Set(13);

  // Read the "integer (MyIntegerProperty)" property from that carrier object.
  Assert.AreEqual(13, carrier.GetConnectedProperty<int, MyIntegerProperty>().Get());
}
{code:C#}

The {{MyIntegerProperty}} acts as a property definition "tag"; we could define another tag {{OtherIntegerProperty}} which is also an integer type, and that would be a different connected property.

# Explicit Connectors

An explicit connector exists as an instance of {{Nito.ConnectedProperties.Explicit.PropertyConnector<TCarrier, TValue>}}, where {{TCarrier}} is the type of carrier object and {{TValue}} is the type of the property value. Explicit connectors may choose to use any type of carrier object (by specifying {{TCarrier}} as {{Object}}), or they may choose to only connect to objects of a certain type or interface.

:: Advanced tip: If you're using explicit connectors and restricting {{TCarrier}} types, there is an interface for explicit connectors ({{Nito.ConnectedProperties.Explicit.IPropertyConnector<in TCarrier, TValue>}}), which is contravariant for the {{TCarrier}} type.

You must manage the lifetime of explicit connectors. Here's an example of a test case using an explicit connector:

{code:C#}
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Explicit;

[TestMethod](TestMethod)
public void ConnectedPropertyMayBeRead()
{
  // Create the property connector: it can connect to any type of object, and its values are integers.
  var propertyDefinition = new PropertyConnector<object, int>();

  // Create a carrier object to connect our property to.
  object carrier = new object();

  // Set the property on that carrier object (the property is connected to the carrier as a side-effect).
  propertyDefinition.GetProperty(carrier).Set(13);

  // Read the property from that carrier object.
  Assert.AreEqual(13, propertyDefinition.GetProperty(carrier).Get());
}
{code:C#}

# Choosing a Connector Type

Named connectors have the following advantages:
* Smallest code and design impact.
* Easiest to share connected properties with other parts of your code. Actually, a bit **too** easy; this is why namespace tags are recommended.

Implicit connectors have the following advantages:
* There is generally less code and less of a design impact, since the connector object itself doesn't have to be created and stored somewhere.
* It's easier to share your connected properties with other parts of the code (you only have to expose your tag type, as opposed to passing around an actual connector object).

Explicit connectors have the following advantages:
* It's possible to restrict to a certain category of carrier objects by specifying {{TCarrier}}.
* It's possible to dynamically create (and destroy) property connectors.
* They are probably easier for most .NET programmers to understand.

Personally, I (Stephen Cleary) would choose named or implicit connectors unless I needed the dynamic creation of explicit connectors.

The Connected Properties library does provide full support for all types, and all connectors may connect to the same carrier object without any problems.