There is a single default property connector for the application domain, {{PropertyConnector.Default}}. You should use this one unless you need to create one of your own.

Every property connector is completely independent from every other property connector. Each connector manages its own set of connected properties.

Property connectors expose the following API:
{code:C#}
// A collection of connectible properties that can be connected to carrier objects.
public sealed class PropertyConnector
{
    /// <summary>
    /// Gets the default collection of connectible properties.
    /// </summary>
    public static PropertyConnector Default { get; }

    /// <summary>
    /// Gets a connectible property with the specified name. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> Get(object carrier, string name, bool bypassValidation = false);

    /// <summary>
    /// Gets a connectible property with the specified name. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> TryGet(object carrier, string name, bool bypassValidation = false);

    /// <summary>
    /// Gets a connectible property for the specified tag type. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <typeparam name="TTag">The tag type of the property.</typeparam>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> Get<TTag>(object carrier, bool bypassValidation = false);

    /// <summary>
    /// Gets a connectible property for the specified tag type. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <typeparam name="TTag">The tag type of the property.</typeparam>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, bool bypassValidation = false);

    /// <summary>
    /// Gets a connectible property for the specified tag type with the specified name. Throws <see cref="InvalidOperationException"/> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <typeparam name="TTag">The tag type of the property.</typeparam>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> Get<TTag>(object carrier, string name, bool bypassValidation = false);

    /// <summary>
    /// Gets a connectible property for the specified tag type with the specified name. Returns <c>null</c> if <paramref name="carrier"/> is not a valid carrier object.
    /// </summary>
    /// <typeparam name="TTag">The tag type of the property.</typeparam>
    /// <param name="carrier">The carrier object for this property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public ConnectibleProperty<dynamic> TryGet<TTag>(object carrier, string name, bool bypassValidation = false);

    /// <summary>
    /// Copies all connectible properties in this collection from one carrier object to another. Throws <see cref="InvalidOperationException"/> if either <paramref name="carrierFrom"/> or <paramref name="carrierTo"/> is not a valid carrier object.
    /// </summary>
    /// <param name="carrierFrom">The carrier object acting as the source of connectible properties.</param>
    /// <param name="carrierTo">The carrier object acting as the destination of connectible properties.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public void CopyAll(object carrierFrom, object carrierTo, bool bypassValidation = false);

    /// <summary>
    /// Copies all connectible properties in this collection from one carrier object to another. Returns <c>false</c> if either <paramref name="carrierFrom"/> or <paramref name="carrierTo"/> is not a valid carrier object.
    /// </summary>
    /// <param name="carrierFrom">The carrier object acting as the source of connectible properties.</param>
    /// <param name="carrierTo">The carrier object acting as the destination of connectible properties.</param>
    /// <param name="bypassValidation">An optional value indicating whether to bypass carrier object validation. The default is <c>false</c>.</param>
    public bool TryCopyAll(object carrierFrom, object carrierTo, bool bypassValidation = false);
}
{code:C#}