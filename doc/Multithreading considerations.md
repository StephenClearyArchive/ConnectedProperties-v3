# Multithreading considerations

Not all types in Nito.Weakness are threadsafe (e.g., [WeakReference](WeakReference) and [WeakCollection](WeakCollection)). However, [ObjectTracker](ObjectTracker) and [ObjectId](ObjectId) are fully threadsafe.

## Lock contention
* Multiple threads may attempt to retrieve [ObjectIds](ObjectId) at the same time. This contention is mitigated within the [ObjectTracker](ObjectTracker) by defining two tiers of concurrent lookups: the instance's [exact type](Exact-type) and its hash code. This reduces multithreaded contention in this scenario to a much smaller scope: the [target objects](Target-instance) must be of the same type and have the same hash code in order to cause contention.
* One thread may attempt to retrieve an [ObjectId](ObjectId) and contend with the [GC detection thread](GC-detection-thread) if it is traversing the same part of the lookup tree.
* Multiple threads may [register actions](Registered-actions) for the same [ObjectId](ObjectId) at the same time.

## Race conditions
* One thread may [register an action](Registered-actions) for an [ObjectId](ObjectId) whose [target](Target-instance) is being garbage collected. In this case, a race condition "winner" is determined:
	* If the user thread wins, then the target is kept alive until the action is registered. The target may then be garbage collected as normal, and the registered action will eventually be invoked by the [GC detection thread](GC-detection-thread).
	* If the garbage collector wins, then the action is not registered, and is invoked immediately.
* When an AppDomain begins unloading, no more [registered actions](Registered-actions) are invoked. This is a race condition that normally happens when an application exits.

## Race conditions (handled by Nito.Weakness)
* One thread may attempt to retrieve an [ObjectId](ObjectId)(ObjectId) for a [target](Target-instance) being garbage collected. The [ObjectTracker](ObjectTracker) will keep the target alive until the [ObjectId](ObjectId)(ObjectId) is created and returned.