using System;

namespace Arci.Networking.Data
{
    /// <summary>
    /// Represents byte stream as a packet
    /// </summary>
    public partial class Packet : ByteBuffer
    {
        /// <summary>
        /// Maximum byte size of stored data
        /// </summary>
        public static readonly int MaxPacketSize = 10000;

        /// <summary>
        /// Opcode number of this Packet
        /// </summary>
        public UInt16 OpcodeNumber { get; private set; }

        /// <summary>
        /// Creates instance of writeable packet
        /// </summary>
        /// <param name="opcodeNumber">Value to be set as opcode number of current packet</param>
        public Packet(UInt16 opcodeNumber)
        {
            Initialize(opcodeNumber);
        }

        /// <summary>
        /// Creates instance of writeable packet
        /// </summary>
        /// <param name="opcode">Value to be set as opcode number of current packet</param>
        public Packet(Enum opcode) : this(Convert.ToUInt16(opcode)) { }

        /// <summary>
        /// Creates instance of readable packet
        /// </summary>
        /// <param name="data">Data to form Packet from</param>
        public Packet(byte[] data) : base(data)
        {
            OpcodeNumber = ReadUInt16();
        }

        /// <summary>
        /// Inicializes data in packet
        /// </summary>
        /// <param name="opcodeNumber">Value to be set as new opcode number of current packet</param>
        public void Initialize(UInt16 opcodeNumber)
        {
            Initialize();

            Write(opcodeNumber);
            OpcodeNumber = opcodeNumber;
        }

        /// <summary>
        /// Inicializes data in packet
        /// </summary>
        /// <param name="opcode">Value to be set as new opcode number of current packet</param>
        public void Initialize(Enum opcode) { Initialize(Convert.ToUInt16(opcode)); }
    }
}
