
using System;

namespace Arci.Networking.Builder
{
    public partial class PacketBuilder
    {
        
        /// <summary>
        /// Writes Int16 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Int16 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if Int16 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(Int16 bit)
        {
            packet.WriteBit(bit);
            return this;
        }
        
        /// <summary>
        /// Writes Int32 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Int32 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if Int32 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(Int32 bit)
        {
            packet.WriteBit(bit);
            return this;
        }
        
        /// <summary>
        /// Writes SByte value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(SByte val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if SByte value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(SByte bit)
        {
            packet.WriteBit(bit);
            return this;
        }
        
        /// <summary>
        /// Writes UInt16 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(UInt16 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if UInt16 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(UInt16 bit)
        {
            packet.WriteBit(bit);
            return this;
        }
        
        /// <summary>
        /// Writes UInt32 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(UInt32 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if UInt32 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(UInt32 bit)
        {
            packet.WriteBit(bit);
            return this;
        }
        
        /// <summary>
        /// Writes Byte value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Byte val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if Byte value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(Byte bit)
        {
            packet.WriteBit(bit);
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
        /// Writes Byte[] value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Byte[] val)
        {
            packet.Write(val);
            return this;
        }
        
    }
}
