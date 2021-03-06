# Type: ObjectId

The {{ObjectId}} type is an [object identifier](Object-ID). All of its members are threadsafe.

The {{ObjectId}} type has three facets:
* It is a unique identifier for a [tracked object](Target-instance).
* It acts as a [weak reference](Weak-reference) for that tracked object.
* It supports [registering callbacks](Registered-action) which are invoked some time after the tracked object is garbage collected.

## Members
* {{bool IsAlive { get; } }} - Gets a value indicating whether the target is still alive (has not been garbage collected).
* {{object Target { get; } }} - Gets the target object. Will return null if the object has been garbage collected.
* {{T TargetAs<T>()}} - Gets the target object, cast to {{T}}. Note that this does an _explicit_ cast, not an "as-cast".
* {{void Register(Action action)}} and {{void Register(Action<ObjectId> action)}} - Registers the action as a callback.

## Creating an ObjectId

{{ObjectId}} instances may not be created directly. They are only returned from the [ObjectTracker.TrackObject](ObjectTracker) method.

For example:
{{
var target = ...;
var id = ObjectTracker.Default.TrackObject(target);
// "id" is now the object identifier for "target"
}}

## Using an ObjectId as a unique identifier

{{ObjectId}} instances use reference equality, so they can be compared using either the {{Equals}} method or {{operator==}}. Every tracked object has exactly one {{ObjectId}} instance.

For example:
{{
static object target = ...;

// In thread 1:
var id1 = ObjectTracker.Default.TrackObject(target);

// In thread 2:
var id2 = ObjectTracker.Default.TrackObject(target);

Assert.IsTrue(id1 == id2);
}}

Even though the two {{ObjectId}} variables are assigned on different threads, they are the same instance.

## Using an ObjectId as a weak reference

Code that has an {{ObjectId}} may use the {{IsAlive}} and {{Target}} properties and the {{TargetAs}} method. {{IsAlive}} is not commonly used, but {{TargetAs}} is useful for getting a strong reference:

{{
ObjectId id = ...;
MyTargetType target = id.TargetAs<MyTargetType>();
if (target == null)
{
  // Target has been garbage collected.
}
else
{
  // Target is still alive. Use it.
}
}}

## Using an ObjectId to register callbacks

It's possible to register a callback that will (in most cases) be invoked some time after the target is garbage collected. See the [registered action documentation](Registered-action) for all the details.

{{
ObjectId id = ...;
id.Register(_ =>
{
  // Be careful in here! See the Registered action documentation...
});
}}