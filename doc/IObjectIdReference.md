# Type: IObjectIdReference<T>

The {{IObjectIdReference<T>}} interface is a strongly-typed wrapper around an [ObjectId](ObjectId). It is an implementation of the [object ID reference concept](Object-ID-reference). Like {{ObjectId}}, {{IObjectIdReference<T>}} is fully threadsafe.

End-user code should not implement this interface.

The {{IObjectIdReference<T>}} type has two facets:
* It acts as a [strongly-typed weak reference](Weak-reference) for that tracked object.
* It supports [registering callbacks](Registered-action) which are invoked some time after the tracked object is garbage collected.

## Members
* {{bool IsAlive { get; } }} - Gets a value indicating whether the target is still alive (has not been garbage collected).
* {{T Target { get; } }} - Gets the target object. Will return null if the object has been garbage collected.
* {{void Register(Action action)}} and {{void Register(Action<ObjectId> action)}} - Registers the action as a callback.

## Creating an IObjectIdReference<T>

{{IObjectIdReference<T>}} instances may not be created directly. They are only returned from the [ObjectTracker.Track](ObjectTracker) method.

For example:
{{
var target = ...;
var id = ObjectTracker.Default.Track(target);
// "id" is now an object identifier reference for "target"
}}

## Comparing IObjectIdReference<T> instances

Types derived from {{IObjectIdReference<T>}} use value equality. Any two {{IObjectIdReference<T>}}-derived types are equal if they refer to the same [ObjectId](ObjectId) (and therefore the same target). They must be compared using the {{Equals}} method, and not {{operator==}}. Note that {{EqualityComparer<IObjectIdReference<T>>.Default}} does use the {{Equals}} method, so the default equality comparer for these types is correct.

For example:
{{
MyDerivedClass target = ...;
MyBaseClass targetAsBase = target;

IObjectIdReference<MyDerivedClass> id1 = ObjectTracker.Default.Track(target);
IObjectIdReference<MyBaseClass> id2 = ObjectTracker.Default.Track(targetAsBase);

// These are equal, even though they're different types
Assert.IsTrue(id1.Equals(id2));

// Assignment is also allowed
id2 = id1;
}}

## Using an IObjectIdReference<T> as a weak reference

Code that has an {{IObjectIdReference<T>}} may use the {{IsAlive}} and {{Target}} properties. {{IsAlive}} is not commonly used, but {{Target}} is useful for getting a strong reference:

{{
IObjectIdReference<MyTagetType> id = ...;
MyTargetType target = id.Target;
if (target == null)
{
  // Target has been garbage collected.
}
else
{
  // Target is still alive. Use it.
}
}}

## Using an IObjectIdReference<T> to register callbacks

It's possible to register a callback that will (in most cases) be invoked some time after the target is garbage collected. See the [registered action documentation](Registered-action) for all the details.

{{
IObjectIdReference<MyTargetType> id = ...;
id.Register(_ =>
{
  // Be careful in here! See the Registered action documentation...
});
}}