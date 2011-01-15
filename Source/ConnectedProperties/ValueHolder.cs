// <copyright file="ValueHolder.cs" company="Nito Programs">
//     Copyright (c) 2011 Nito Programs.
// </copyright>

namespace Nito.ConnectedProperties
{
    /// <summary>
    /// A reference type that holds a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    internal sealed class ValueHolder<TValue>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public TValue Value { get; set; }
    }
}
