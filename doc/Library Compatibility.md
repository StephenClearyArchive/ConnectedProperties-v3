The ConnectedProperties library:
* includes code contracts. The release version includes code contracts as a separate dll (only checking public surface contracts); the debug version includes full internal code contract checking.
* uses [semantic versioning](http://semver.org/), and has used it from day one.
* is available as a NuGet package for installation from within Visual Studio.

The library does use concurrent dictionaries internally. On modern platforms (.NET 4.0 and Windows Store), the BCL ConcurrentDictionary type is used; on older platforms, a locking Dictionary is used.