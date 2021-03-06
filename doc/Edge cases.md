# Edge cases

## Resurrection

Resurrection is a .NET technique for an object's finalizer to "resurrect" that object. The object - which has already been marked for garbage collection - is then "unmarked" and made alive again.

The [object tracker](Object-tracker) monitors when an instance becomes garbage. If a [target instance](Target-instance) is resurrected after it becomes garbage, the object tracker will ignore its resurrection. It only "sees" the first transition from non-garbage to garbage.

## AppDomain unloading

As soon as an AppDomain begins unloading, no further [registered actions](Registered-actions) are invoked. For this reason, registered actions may not be used to "attach" a finalizer to a target.

The [GC detection thread](GC-detection-thread) which executes the actions is aborted during AppDomain unloading, so an asynchronous {{ThreadAbortException}} will be raised in any actions that are executing when the AppDomain is unloaded.

All resources are properly cleaned up when unloading an AppDomain.