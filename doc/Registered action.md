# Concept: Registered action

Callback actions may be registered for a [target instance](Target-instance) that is being tracked by an [object tracker](Object-tracker). These actions are invoked some time after the target has been garbage collected (unless the AppDomain is unloaded).

Actions are registered by the target's [object ID](Object-ID). Normally, to register an action for a target, the target is first passed to the object tracker, which returns the object ID; the action is then passed to the object ID's {{Register}} method, which registers it.

## Restrictions on registered actions
* Registered actions should not directly or indirectly reference the target (but referencing the target's object ID is OK). If a registered action references the target, then the target will never become eligible for garbage collection, and the registered action will never be invoked.
* Registered actions may not throw exceptions. Any exceptions thrown from a registered action will crash the process (except {{ThreadAbortException}}, which is normal during AppDomain unloading).
* Registered actions must be threadsafe. In particular:
	* Registered actions may be directly invoked from the {{Register}} method. This is due to a race condition where the target was garbage collected immediately before {{Register}} was called.
	* Registered actions may be invoked from the [GC detection thread](GC-detection-thread) if they are not directly invoked from the {{Register}} method.
	* Registered actions may run concurrently with other registered actions and/or the finalizer of the target.

## Additional considerations
* Registered actions are not always invoked _immediately_ after the target is garbage collected. It may take some time for the garbage collection to be detected.
* If the AppDomain is unloaded, then no more registered actions are invoked. Therefore, registered actions cannot be used to "attach" a finalizer to a target object.

## Guarantees for registered actions
* No locks are held while a registered action is invoked.
* At some point after the target is garbage collected, the registered action will be invoked (unless the AppDomain is unloaded). This is true as long as the GC detection thread is running.