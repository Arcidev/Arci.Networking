using System;

namespace Arci.Networking.Serialization.Attributes
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
        /// Create new instance of packet property attribute
        /// </summary>
        /// <param name="order">Order of the property</param>
        public PacketPropertyAttribute(UInt32 order)
        {
            Order = order;
        }
    }
}
