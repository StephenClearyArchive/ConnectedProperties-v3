Care must be taken when connecting properties to carrier objects through interfaces or base classes.

Interfaces may be implemented by reference types or value types, and value types cannot be carrier objects. Similarly, a reference-equatable base class may have a derived class that uses value equality, and an instance of that derived class cannot be a carrier object.

The {{TryGet}} methods may be used to access properties only if the carrier instance is a valid carrier:

{code:C#}
public void DisplayName(object obj)
{
  var nameProperty = PropertyConnector.Default.TryGet(obj, "Name");
  if (nameProperty != null)
  {
    Console.WriteLine("Name: " + nameProperty.Get());
  }
  else
  {
    Console.WriteLine("No name is connected.");
  }
}
{code:C#}
