# Type: ObjectTracker

The {{ObjectTracker}} tracks object instances (see the [object tracker conceptual documentation](Object-tracker) for more details).

There is only one {{ObjectTracker}} instance, available through the static {{Default}} property. All members are fully threadsafe.

The {{ObjectTracker}} is only responsible for two things:
* Adding [target objects](Target-instance) to track.
* Running the [GC detection thread](GC-detection-thread), which detects when tracked objects are garbage collected.

All other object tracking operations (retrieving a strong reference, registering a callback, etc) are handled by [ObjectId](ObjectId) or [IObjectIdReference<T>](IObjectIdReference).

## Members

* {{static ObjectTracker Default { get; } }} - Gets the only instance of {{ObjectTracker}} for this {{AppDomain}}.
* {{int GCDetectionWaitTimeInMilliseconds { get; set; } }} - Gets or sets the amount of time for the GC detection thread to wait in-between each scan of all tracked objects, in milliseconds. This may be set to {{Timeout.Infinite}} to pause the GC detection thread.
* {{ObjectId TrackObject(object target)}} - Adds {{target}} as a tracked instance. Returns the [ObjectId](ObjectId) for that {{target}}. If the {{target}} is already tracked, {{TrackObject}} will return its existing {{ObjectId}}.
* {{IObjectIdReference<T> Track<T>(T target)}} - Same as {{TrackObject}}, but returns a strongly-typed [IObjectIdReference<T>](IObjectIdReference) wrapping the target's {{ObjectId}}.