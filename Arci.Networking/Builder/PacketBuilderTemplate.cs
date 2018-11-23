
using System;

namespace Arci.Networking.Builder
{
    public partial class PacketBuilder
    {
        
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(SByte value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
            return this;
        }
        
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(Int16 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(Int32 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
            return this;
        }
        
        /// <summary>
        /// Writes Int64 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Int64 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if Int64 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(Int64 bit)
        {
            packet.WriteBit(bit);
            return this;
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(Int64 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(Byte value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(UInt16 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
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
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(UInt32 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
            return this;
        }
        
        /// <summary>
        /// Writes UInt64 value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(UInt64 val)
        {
            packet.Write(val);
            return this;
        }

        /// <summary>
        /// Writes 1 if UInt64 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBit(UInt64 bit)
        {
            packet.WriteBit(bit);
            return this;
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder WriteBits(UInt64 value, byte bitsCount)
        {
            packet.WriteBits(value, bitsCount);
            return this;
        }
                
        /// <summary>
        /// Writes Byte[] value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Byte[] val)
        {
            packet.Write(val);
            return this;
        }
        
        /// <summary>
        /// Writes Single value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Single val)
        {
            packet.Write(val);
            return this;
        }
        
        /// <summary>
        /// Writes Double value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Double val)
        {
            packet.Write(val);
            return this;
        }
        
        /// <summary>
        /// Writes Decimal value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Decimal val)
        {
            packet.Write(val);
            return this;
        }
        
        /// <summary>
        /// Writes String value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(String val)
        {
            packet.Write(val);
            return this;
        }
        
        /// <summary>
        /// Writes Guid value to packet
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <returns>This PacketBuilder</returns>
        public PacketBuilder Write(Guid val)
        {
            packet.Write(val);
            return this;
        }
        
    }
}
