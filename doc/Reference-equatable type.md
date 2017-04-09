# Concept: Reference-equatable type

A reference-equatable type is a type that uses reference equality, not value equality.

Interfaces, pointers, delegates, and of course value types (including enumerations) are not reference-equatable. However, types derived from interfaces may be reference-equatable.

Any base class of a reference-equatable type is also a reference-equatable type. However, a class derived from a reference-equatable type may or may not be reference-equatable.

The concept of a "reference-equatable type" by itself is not very useful; it is used in the definition of the [reference-equatable instance concept](Reference-equatable-instance), which is much more useful.