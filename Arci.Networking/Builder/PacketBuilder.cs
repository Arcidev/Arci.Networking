using Arci.Networking.Data;
using System;
using System.Text;

namespace Arci.Networking.Builder
{
    /// <summary>
    /// Builder for Packet class
    /// </summary>
    public partial class PacketBuilder
    {
        private readonly Packet packet;

        /// <summary>
        /// Creates new instance of packet builder
        /// </summary>
        /// <param name="packet">Packet to build</param>
        /// <exception cref="ArgumentNullException">Thrown if packet is null</exception>
        public PacketBuilder(Packet packet)
        {
            this.packet = packet ?? throw new ArgumentNullException("Param packet cannot be null");
        }

        /// <summary>
        /// Flushes bits in builded packet
        /// </summary>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder FlushBits()
        {
            packet.FlushBits();
            return this;
        }

        /// <summary>
        /// Writes guid byte stream in specified order
        /// </summary>
        /// <param name="guid">Guid to be written</param>
        /// <param name="indexes">Order</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteGuidByteStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            packet.WriteGuidByteStreamInOrder(guid, indexes);
            return this;
        }

        /// <summary>
        /// Writes guid bit stream in specified order
        /// </summary>
        /// <param name="guid">Guid to be written</param>
        /// <param name="indexes">Order</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteGuidBitStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            packet.WriteGuidBitStreamInOrder(guid, indexes);
            return this;
        }

        /// <summary>
        /// Writes string value with ASCII Encoding
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(string val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes string value
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(string val, Encoding encoding)
        {
            packet.Write(val, encoding);
            return this;
        }

        /// <summary>
        /// Writes bytebuffer to packet
        /// </summary>
        /// <param name="buffer">Buffer to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(ByteBuffer buffer)
        {
            packet.Write(buffer);
            return this;
        }

        /// <summary>
        /// Writes bit value to stream
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(bool bit)
        {
            packet.WriteBit(bit);
            return this;
        }

        /// <summary>
        /// Builds packet
        /// </summary>
        /// <returns>Builded packet</returns>
        public Packet Build()
        {
            return packet;
        }
    }
}
