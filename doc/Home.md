# Project Description
The Nito Connected Properties library provides a simple API to attach properties to most .NET objects at runtime. It is developed in C# with code contracts.

# The 2-Minute Intro: Connecting a "Name" to (almost) any Object

The following code shows how to connect a "Name" property to an object:

{code:C#}
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
{code:C#}

Note that the lifetime of the connected property is exactly as if it was a real property of the object, and the lifetime of the object is not changed in any way (even if the property refers to the object). Connected properties are a true [ephemeron implementation](http://en.wikipedia.org/wiki/Ephemeron) for .NET.

See the [documentation tab](http://connectedproperties.codeplex.com/documentation) for all kinds of fun details.