# Concept: Object ID reference

An object ID reference is a strongly-typed wrapper for an [object ID](Object-ID). Multiple object ID references may refer to the same object ID.

As such, an object ID reference is a [strongly-typed weak reference](Weak-reference) for the underlying [target](Target-instance). It also supports [registering actions](Registered-action). In these ways, it is conceptually similar to an object ID (with a strongly-typed weak reference instead of a weakly-typed weak reference).

However, object ID references are different than object IDs when the concept of "equality" is considered. An object ID is a unique identifying instance for its [target object](Target-instance). In contrast, multiple object ID references may refer to the same object ID (and therefore the same target object).

Furthermore, object ID references are strongly-typed, and it is possible to have two object ID references of different types that refer to the same object ID (and the same target object). One example of this is an object ID reference for a {{Base}} type and another object ID reference for a {{Derived}} type; both references may refer to the same object ID and target object, even though the object ID references are of different types.

Object ID references do support value equality; two object ID references are considered equal iff they both refer to the same object ID (and therefore the same target object). Note that an unusual consequence of this is that object ID references for different types may be equal.

In the Nito.Weakness library, object ID references are represented by the [IObjectIdReference interface](IObjectIdReference).