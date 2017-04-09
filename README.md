# ConnectedProperties-v3

This is a source archive for v3 (and earlier) of Nito.ConnectedProperties.

For the current (v4 and later) source, see [here](https://github.com/StephenCleary/ConnectedProperties).

# ConnectedProperties

The Nito Connected Properties library provides a simple API to attach properties to most .NET objects at runtime.

## The 2-Minute Intro: Connecting a "Name" to (almost) any Object

The following code shows how to connect a "Name" property to an object:

````C#
// Use the Connected Properties library.
using Nito.ConnectedProperties;

class Program
{
  // Display the Name of any object passed into this method.
  public static void DisplayName(object obj)
  {
    // Look up a connected property called "Name".
    var nameProperty = PropertyConnector.Default.Get(obj, "Name");
    Console.WriteLine("Name: " + nameProperty.Get());
  }

  static void Main()
  {
    // Create an object to name.
    var obj = new object();

    // I dub this object "Bob".
    var nameProperty = PropertyConnector.Default.Get(obj, "Name");
    nameProperty.Set("Bob");

    // Pass the object to the DisplayName method, which is able to retrieve the connected property.
    DisplayName(obj);
  }
}
````

Note that the lifetime of the connected property is exactly as if it was a real property of the object, and the lifetime of the object is not changed in any way (even if the property refers to the object). Connected properties are a true [ephemeron implementation](http://en.wikipedia.org/wiki/Ephemeron) for .NET.

See the [documentation](doc/Documentation.md) for all kinds of fun details.
