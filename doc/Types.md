## Commonly-used types

* [WeakReference](WeakReference) - A strongly-typed, disposable replacement for MSDN's [WeakReference type](http://msdn.microsoft.com/en-us/library/system.weakreference.aspx).
* [WeakCollection](WeakCollection) - A simple collection of weak references to objects.

## Types used by implementation (exposed for convenience)

* [ObjectId](ObjectId) - A unique identifier and weak reference for an object instance.
* [IObjectIdReference](IObjectIdReference) - A strongly-typed wrapper around [ObjectId](ObjectId).
* [ObjectTracker](ObjectTracker) - The component that creates [ObjectIds](ObjectId) and tracks the lifetimes of objects.