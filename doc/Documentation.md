# Overview

Nito.ConnectedProperties provides an API for attaching properties to most objects at runtime.

A _connectible property_ is a piece of data that may be connected to a _carrier object_ at runtime. A connectible property is either _connected_ or _disconnected_. Only properties that are connected have values.

: Note: In the documentation and IntelliSense, the term _connected property_ is often used as a synonym for _connectible property_. The term _connected property_ does not imply that the connectible property is actually _connected_. If there is any possibility of confusion, the term _connectible property_ is used instead of _connected property_.

A _carrier object_ is any instance capable of having properties connected to it - specifically, a reference type that uses reference equality.

A _connector_ manages the connections between connectible properties and carrier objects. There is a default connector that is sufficient for most people, but you can also create your own connector instance.

All accesses to connectible property values are thread-safe. However, the actual _value_ of the connected property is only thread-safe if the connected property type is thread-safe (e.g., an immutable type).

# Table of Contents

[Connected Property Values](Connected-Property-Values)

[Carrier Objects](Carrier-Objects)

[Property Connectors](Property-Connectors)

[Object Lifetimes](Object-Lifetimes)

[Limitations](Limitations)

Techniques
: [Extension Method Wrappers](Extension-Method-Wrappers)
: [Interfaces and Inheritance](Interfaces-and-Inheritance)
: [Connected Methods](Connected-Methods)

Details
: [AppDomains](AppDomains)
: [Notes on Naming](Notes-on-Naming)
: [Library Compatibility](Library-Compatibility)
: [How It Works](How-It-Works)

Obsolete documentation
: [Named, Implicit, and Explicit Connectors](Named,-Implicit,-and-Explicit-Connectors)
: [Visibility of Implicit Connectors](Visibility-of-Implicit-Connectors)
: [Dynamic Connected Properties](Dynamic-Connected-Properties)
