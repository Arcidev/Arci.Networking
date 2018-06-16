using System;

namespace Arci.Networking.Object.Attributes
{
    /// <summary>
    /// Packet property attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PacketPropertyAttribute : Attribute
    {
        /// <summary>
        /// Order of the property
        /// </summary>
        public UInt32 Order { get; }

        /// <summary>
        /// Desired type for the property to be parsed as (used for different than property type).
        /// Type provided must be convertible with PropertyType
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Create new instance of packet property attribute
        /// </summary>
        /// <param name="order">Order of the property</param>
        /// <param name="type">Type if desired different than property type</param>
        public PacketPropertyAttribute(UInt32 order, Type type = null)
        {
            Order = order;
            Type = type;
        }
    }
}
