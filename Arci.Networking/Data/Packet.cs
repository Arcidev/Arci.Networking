using System;
using System.IO;

namespace Arci.Networking.Data
{
    public partial class Packet : IDisposable
    {
        private BinaryReader readData;
        private BinaryWriter writeData;
        private MemoryStream memoryStream;

        private int bitPos;
        private byte curBitVal;

        public static readonly int MaxPacketSize = 10000;
        
        // Creates instance of writeable packet
        public Packet(UInt16 opcodeNumber)
        {
            Initialize(opcodeNumber);
        }

        // Creates instance of writeable packet
        public Packet(Enum opcode) : this(Convert.ToUInt16(opcode)) { }

        // Creates instance of readable packet
        public Packet(byte[] data)
        {
            inicialize();
            readData = new BinaryReader(new MemoryStream(data));
            OpcodeNumber = ReadUInt16();
        }

        // Inicializes data in packet
        public void Initialize(UInt16 opcodeNumber)
        {
            inicialize();
            Dispose();

            memoryStream = new MemoryStream();
            writeData = new BinaryWriter(memoryStream);
            writeData.Write(opcodeNumber);
        }

        // Inicializes data in packet
        public void Initialize(Enum opcode) { Initialize(Convert.ToUInt16(opcode)); }

        // Writes 1 bit
        public void WriteBit(UInt32 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
            {
                bitPos = 8;
                Write(curBitVal);
                curBitVal = 0;
            }
        }

        // Writes guid byte stream in specified order
        public void WriteGuidByteStreamInOrder(Guid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                if (guid[index] != 0)
                    Write(guid[index]);
        }

        // Writes guid bit stream in specified order
        public void WriteGuidBitStreamInOrder(Guid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                WriteBit(guid[index]);
        }

        // Flushes bits from memory to stream
        public void FlushBits()
        {
            if (bitPos == 8)
                return;

            Write(curBitVal);
            curBitVal = 0;
            bitPos = 8;

        }

        // Reads 1 bit
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

        // Writes string value
        public void Write (string val)
		{
			writeData.Write((UInt16)val.Length);
            writeData.Write(System.Text.Encoding.ASCII.GetBytes(val));
		}

        // Reads Int16
        public Int16 ReadInt16() { return readData.ReadInt16(); }

        // Reads Int32
        public Int32 ReadInt32() { return readData.ReadInt32(); }

        // Reads UInt16
        public UInt16 ReadUInt16() { return readData.ReadUInt16(); }

        // Reads UInt32
        public UInt32 ReadUInt32() { return readData.ReadUInt32(); }

        // Reads byte
        public byte ReadByte() { return readData.ReadByte(); }

        // Reads sbyte
        public sbyte ReadSByte() { return readData.ReadSByte(); }

        // Reads multiple bytes
        public byte[] ReadBytes()
        {
            var length = ReadUInt16();
            return readData.ReadBytes(length);
        }

        // Reads multiple bytes
        public byte[] ReadBytes(int length)
        {
            return readData.ReadBytes(length);
        }

        // Reads string
        public string ReadString()
        {
            return System.Text.Encoding.ASCII.GetString(ReadBytes());
        }

        // Reads guid byte stream in specified order
        public void ReadGuidByteStreamInOrder(Guid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                if (guid[index] != 0)
                    guid[index] = ReadByte();
        }

        // Reads guid bit stream in specified order
        public void ReadGuidBitStreamInOrder(Guid guid, params int[] indexes)
        {
            foreach (var index in indexes)
                guid[index] = (byte) (ReadBit() ? 1 : 0);
        }

        public byte[] Data { get { return memoryStream.ToArray(); } }

        public UInt16 OpcodeNumber { get; private set; }

        // Disposing object
        public void Dispose()
        {
            if (writeData != null)
                writeData.Close();
            if (memoryStream != null)
                memoryStream.Close();
            if (readData != null)
                readData.Close();
        }

        private void inicialize()
        {
            bitPos = 8;
            curBitVal = 0;
        }
    }
}
