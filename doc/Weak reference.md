# Concept: Weak reference

A weak reference is a logical reference to an object instance that does not prevent garbage collection for that instance.

Weak references have two properties:
* {{IsAlive}} - Determines if the object instance has been garbage collected. This is seldomly used.
* {{Target}} - Converts the weak reference into a strong reference, resulting in {{null}} if the object instance has been garbage collected.

Weak references may be strongly-typed or weakly-typed. The only difference is in the type of {{Target}}: a strongly-typed weak reference has a specific type for the {{Target}} property, while a weakly-typed weak reference just uses {{object}} for the type of the {{Target}} property.

The [CLR WeakReference type](http://msdn.microsoft.com/en-us/library/system.weakreference.aspx) is a weak reference, but is not used by Nito.Weakness for various reasons.

The Nito.Weakness library includes several types that are weak references:
* [WeakReference](WeakReference) (strongly-typed weak reference)
* [ObjectId](ObjectId) (threadsafe, weakly-typed weak reference)
* [IObjectIdReference](IObjectIdReference) (threadsafe, strongly-typed weak reference)