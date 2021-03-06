A connectible property is either _connected_ or _disconnected_.

When a property is connected to a carrier object, it has a value related to that object.

**Note:** A disconnected property is different than a connected property with a value of {{null}}.

Connected properties expose the following API:

{code:C#}
/// <summary>
/// Sets the value of the property, overwriting any existing value.
/// </summary>
/// <param name="value">The value to set.</param>
void Set(T value);

/// <summary>
/// Gets the value of the property, if it is connected. Returns <c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.
/// </summary>
/// <param name="value">Receives the value of the property, if this method returns <c>true</c>.</param>
/// <returns><c>true</c> if the property was returned in <paramref name="value"/>; <c>false</c> if the property was disconnected.</returns>
bool TryGet(out T value);

/// <summary>
/// Gets the value of the property, throwing an exception if the property was disconnected.
/// </summary>
/// <returns>The value of the property.</returns>
T Get();

/// <summary>
/// Sets the value of the property, if it is disconnected. Otherwise, does nothing. Returns <c>true</c> if the property value was set; <c>false</c> if the property was already connected.
/// </summary>
/// <param name="value">The value to set.</param>
/// <returns><c>true</c> if the property value was set; <c>false</c> if the property was already connected.</returns>
bool TryConnect(T value);

/// <summary>
/// Sets the value of the property, throwing an exception if the property was already connected.
/// </summary>
/// <param name="value">The value to set.</param>
void Connect(T value);

/// <summary>
/// Updates the value of the property, if the existing value matches a comparision value.
/// Otherwise, does nothing. Returns <c>true</c> if the property value was updated; <c>false</c> if the comparision failed.
/// The comparision is done using the default object equality comparer.
/// </summary>
/// <param name="newValue">The value to set.</param>
/// <param name="comparisonValue">The value to compare to the existing value.</param>
bool TryUpdate(T newValue, T comparisonValue);

/// <summary>
/// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
/// </summary>
/// <param name="connectValue">The new value of the property, if it is disconnected.</param>
/// <returns>The value of the property.</returns>
T GetOrConnect(T connectValue);

/// <summary>
/// Gets the value of the property, if it is connected; otherwise, sets the value of the property and returns the new value.
/// </summary>
/// <param name="createCallback">The delegate invoked to create the value of the property, if it is disconnected. May not be <c>null</c>. If there is a multithreaded race condition, each thread's delegate may be invoked, but all values except one will be discarded.</param>
/// <returns>The value of the property.</returns>
T GetOrCreate(Func<T> createCallback);

/// <summary>
/// Sets the value of a property, connecting it if necessary. <paramref name="createCallback"/> and <paramref name="updateCallback"/> may be invoked multiple times. Returns the new value of the property.
/// </summary>
/// <param name="createCallback">The delegate invoked to create the value if the property is not connected.</param>
/// <param name="updateCallback">The delegate invoked to update the value if the property is connected.</param>
T CreateOrUpdate(Func<T> createCallback, Func<T, T> updateCallback);

/// <summary>
/// Connects a new value or updates the existing value of the property. <paramref name="updateCallback"/> may be invoked multiple times. Returns the new value of the property.
/// </summary>
/// <param name="connectValue">The value to set if the property is not connected.</param>
/// <param name="updateCallback">The delegate invoked to update the value if the property is connected.</param>
T ConnectOrUpdate(T connectValue, Func<T, T> updateCallback);

/// <summary>
/// Attempts to disconnect the property. Returns <c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.
/// </summary>
/// <returns><c>true</c> if the property was disconnected by this method; <c>false</c> if the property was already disconnected.</returns>
bool TryDisconnect();

/// <summary>
/// Disconnects the property, throwing an exception if the property was already disconnected.
/// </summary>
void Disconnect();

/// <summary>
/// Creates a new instance that casts the property value to a specified type.
/// </summary>
/// <typeparam name="TResult">The type of the property value.</typeparam>
ConnectibleProperty<TResult> Cast<TResult>();
{code:C#}