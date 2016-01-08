using System;

namespace Arci.Networking.Data
{
    public partial class Packet : ByteBuffer
    {
        public static readonly int MaxPacketSize = 10000;

        public UInt16 OpcodeNumber { get; private set; }

        // Creates instance of writeable packet
        public Packet(UInt16 opcodeNumber)
        {
            Initialize(opcodeNumber);
        }

        // Creates instance of writeable packet
        public Packet(Enum opcode) : this(Convert.ToUInt16(opcode)) { }

        // Creates instance of readable packet
        public Packet(byte[] data) : base(data)
        {
            OpcodeNumber = ReadUInt16();
        }

        // Inicializes data in packet
        public void Initialize(UInt16 opcodeNumber)
        {
            Initialize();

            Write(opcodeNumber);
            OpcodeNumber = opcodeNumber;
        }

        // Inicializes data in packet
        public void Initialize(Enum opcode) { Initialize(Convert.ToUInt16(opcode)); }
    }
}
