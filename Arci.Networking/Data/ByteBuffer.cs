﻿using System;
using System.IO;
using System.Text;

namespace Arci.Networking.Data
{
    /// <summary>
    /// Byffer to store byte stream
    /// </summary>
    public partial class ByteBuffer : IDisposable
    {
        private BinaryReader readData;
        private BinaryWriter writeData;
        private MemoryStream memoryStream;

        private int bitPos;
        private byte curBitVal;

        /// <summary>
        /// Data in packet
        /// </summary>
        public byte[] Data => memoryStream.ToArray();

        /// <summary>
        /// Creates new instance of readable ByteBuffer
        /// </summary>
        /// <param name="data">Data to form ByteBuffer from</param>
        protected ByteBuffer(byte[] data)
        {
            InitBitData();
            memoryStream = new MemoryStream(data);
            readData = new BinaryReader(memoryStream);
        }

        /// <summary>
        /// Creates new instance of writable ByteBuffer
        /// </summary>
        public ByteBuffer()
        {
            Initialize();
        }

        /// <summary>
        /// Writes guid byte stream in specified order
        /// </summary>
        /// <param name="guid">Guid to be written</param>
        /// <param name="indexes">Order</param>
        public void WriteGuidByteStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                if (guid[index] != 0)
                    Write(guid[index]);
        }

        /// <summary>
        /// Writes guid bit stream in specified order
        /// </summary>
        /// <param name="guid">Guid to be written</param>
        /// <param name="indexes">Order</param>
        public void WriteGuidBitStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                WriteBit(guid[index]);
        }

        /// <summary>
        /// Flushes bits from memory to stream
        /// </summary>
        public void FlushBits()
        {
            if (bitPos == 8)
                return;

            WriteCurBitVal();
        }

        /// <summary>
        /// Clears unflushed bits
        /// </summary>
        public void ClearUnflushedBits()
        {
            InitBitData();
        }

        /// <summary>
        /// Reads 1 bit
        /// </summary>
        /// <returns>1 bit from stream</returns>
        public bool ReadBit()
        {
            ++bitPos;
            if (bitPos > 7)
            {
                bitPos = 0;
                curBitVal = ReadByte();
            }

            return ((curBitVal >> (7 - bitPos)) & 1) != 0;
        }

        /// <summary>
        /// Reads exact number of bits and return result as a value
        /// </summary>
        /// <param name="bitsCount">Bits to be read</param>
        /// <returns>Value represented by bits that have been read</returns>
        public UInt32 ReadBits(byte bitsCount)
        {
            UInt32 value = 0;
            for (int i = bitsCount - 1; i >= 0; i--)
            {
                if (ReadBit())
                    value |= (UInt32)1 << i;
            }
            return value;
        }

        /// <summary>
        /// Writes string value
        /// </summary>
        /// <param name="val">Value to be written</param>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        public void Write(string val, Encoding encoding = null)
        {
            writeData.Write((UInt16)val.Length);
            writeData.Write((encoding ?? Encoding.ASCII).GetBytes(val));
        }

        /// <summary>
        /// Appends the whole storage of bytebuffer to this one
        /// </summary>
        /// <param name="buffer">Buffer to be written</param>
        public void Write(ByteBuffer buffer)
        {
            writeData.Write(buffer.Data);
        }

        /// <summary>
        /// Reads multiple bytes based on length written in stream
        /// </summary>
        /// <returns>Byte array from stream</returns>
        public byte[] ReadBytes()
        {
            var length = ReadUInt16();
            return readData.ReadBytes(length);
        }

        /// <summary>
        /// Reads multiple bytes
        /// </summary>
        /// <param name="length">Bytes to be read</param>
        /// <returns>Byte array from stream</returns>
        public byte[] ReadBytes(int length)
        {
            return readData.ReadBytes(length);
        }

        /// <summary>
        /// Reads string
        /// </summary>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        /// <returns>String in ASCII format</returns>
        public string ReadString(Encoding encoding = null)
        {
            return (encoding ?? Encoding.ASCII).GetString(ReadBytes());
        }

        /// <summary>
        /// Reads guid byte stream in specified order
        /// </summary>
        /// <param name="guid">Guid to store value from stream</param>
        /// <param name="indexes">Order</param>
        public void ReadGuidByteStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                if (guid[index] != 0)
                    guid[index] = ReadByte();
        }

        /// <summary>
        /// Reads guid bit stream in specified order
        /// </summary>
        /// <param name="guid">Guid to store value from stream</param>
        /// <param name="indexes">Order</param>
        public void ReadGuidBitStreamInOrder(PacketGuid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                guid[index] = (byte) (ReadBit() ? 1 : 0);
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            if (writeData != null)
                writeData.Dispose();
            if (memoryStream != null)
                memoryStream.Dispose();
            if (readData != null)
                readData.Dispose();
        }

        /// <summary>
        /// Inicializes data in ByteBuffer
        /// </summary>
        protected void Initialize()
        {
            InitBitData();
            Dispose();

            memoryStream = new MemoryStream();
            writeData = new BinaryWriter(memoryStream);
        }

        /// <summary>
        /// Inicializes inner data
        /// </summary>
        private void InitBitData()
        {
            bitPos = 8;
            curBitVal = 0;
        }
    }
}
