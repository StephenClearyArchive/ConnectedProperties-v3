# Concept: Garbage collection detection thread

A garbage collection detection thread is a thread that runs in the background to detect garbage collection.

This thread periodically checks any [targets](Target-instance) tracked by an [object tracker](Object-tracker), determining whether they have been marked as garbage or not. When the GC detection thread detects that a target is eligible for garbage collection, it will invoke any [registered actions](Registered-action) for that target.