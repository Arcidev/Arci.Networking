using System;

namespace Arci.Networking.Object.Attributes
{
    /// <summary>
    /// Packet class attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PacketClassAttribute : Attribute
    {
        /// <summary>
        /// Packet opcode
        /// </summary>
        public UInt16 PacketOpcode { get; }

        /// <summary>
        /// Create new instance of packet object attribute
        /// </summary>
        /// <param name="packetOpcode">Order of the property</param>
        public PacketClassAttribute(UInt16 packetOpcode)
        {
            PacketOpcode = packetOpcode;
        }
    }
}
