# Concept: Object tracker

An object tracker is an object which tracks the lifetimes of other objects.

An object tracker has two responsibilities:
* Create or retrieve [object IDs](Object-ID) for any [target object](Target-instance) it is asked to track.
* Run the [GC detection thread](GC-detection-thread), which detects when those targets have been garbage collected.

These responsibilities are not handled by the object tracker; they are delegated to the [object ID](Object-ID) and [object ID reference](Object-ID-reference) types:
* Checking if the target is still alive, or retrieving a strong reference to the target.
* [Registering callbacks](Registered-action) that are invoked some time after the target is garbage collected.

If an object tracker is requested to track the same target twice, it will return the same object ID for both of those requests. Because of this, an object tracker can only track [reference-equatable instances](Reference-equatable-instance).

In the Nito.Weakness library, there is only one object tracker per AppDomain: the static Default property of the [ObjectTracker class](ObjectTracker).