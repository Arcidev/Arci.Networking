
using System;

namespace Arci.Networking.Data
{
    public partial class ByteBuffer : IDisposable
    {
        
        /// <summary>
        /// Reads Int16 value from stream
        /// </summary>
        /// <returns>Int16 value from stream</returns>
        public Int16 ReadInt16()
        {
            return readData.ReadInt16();
        }

        /// <summary>
        /// Writes Int16 value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(Int16 val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if Int16 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(Int16 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(Int16 value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((Int16)1 << i) & value);
        }
        
        /// <summary>
        /// Reads Int32 value from stream
        /// </summary>
        /// <returns>Int32 value from stream</returns>
        public Int32 ReadInt32()
        {
            return readData.ReadInt32();
        }

        /// <summary>
        /// Writes Int32 value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(Int32 val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if Int32 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(Int32 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(Int32 value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((Int32)1 << i) & value);
        }
        
        /// <summary>
        /// Reads SByte value from stream
        /// </summary>
        /// <returns>SByte value from stream</returns>
        public SByte ReadSByte()
        {
            return readData.ReadSByte();
        }

        /// <summary>
        /// Writes SByte value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(SByte val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if SByte value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(SByte bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(SByte value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((SByte)1 << i) & value);
        }
        
        /// <summary>
        /// Reads UInt16 value from stream
        /// </summary>
        /// <returns>UInt16 value from stream</returns>
        public UInt16 ReadUInt16()
        {
            return readData.ReadUInt16();
        }

        /// <summary>
        /// Writes UInt16 value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(UInt16 val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if UInt16 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(UInt16 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(UInt16 value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((UInt16)1 << i) & value);
        }
        
        /// <summary>
        /// Reads UInt32 value from stream
        /// </summary>
        /// <returns>UInt32 value from stream</returns>
        public UInt32 ReadUInt32()
        {
            return readData.ReadUInt32();
        }

        /// <summary>
        /// Writes UInt32 value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(UInt32 val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if UInt32 value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(UInt32 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(UInt32 value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((UInt32)1 << i) & value);
        }
        
        /// <summary>
        /// Reads Byte value from stream
        /// </summary>
        /// <returns>Byte value from stream</returns>
        public Byte ReadByte()
        {
            return readData.ReadByte();
        }

        /// <summary>
        /// Writes Byte value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(Byte val)
        {
            writeData.Write(val);
        }

        /// <summary>
        /// Writes 1 if Byte value is different from 0, otherwise writes 0
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(Byte bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }

        /// <summary>
        /// Writes value with specified number of bits
        /// </summary>
        /// <param name="value">Value to be written</param>
        /// <param name="bitsCount">Number of bits that value should written with</param>
        public void WriteBits(Byte value, byte bitsCount)
        {
            for (int i = bitsCount - 1; i >= 0; i--)
                WriteBit(((Byte)1 << i) & value);
        }
        
        /// <summary>
        /// Writes bit value to stream
        /// </summary>
        /// <param name="bit">Value to be written</param>
        public void WriteBit(bool bit)
        {
            --bitPos;
            if (bit)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        /// <summary>
        /// Writes Byte[] value to stream
        /// </summary>
        /// <param name="val">Value to be written</param>
        public void Write(Byte[] val)
        {
            writeData.Write((UInt16)val.Length);
            writeData.Write(val);
        }
        
        /// <summary>
        /// Writes current bit values to stream
        /// </summary>
        private void WriteCurBitVal()
        {
            Write(curBitVal);
            bitPos = 8;
            curBitVal = 0;
        }
    }
}
