using Arci.Networking.Data;
using Arci.Networking.Object.Attributes;
using System;

namespace Arci.Networking.Tests.ObjectTests.Objects
{
    [PacketClass(2)]
    public class TestObject2
    {
        [PacketProperty(6)]
        public SByte SByte { get; set; }

        [PacketProperty(5)]
        public UInt16 UInt16 { get; set; }

        [PacketProperty(4)]
        public Int32 Int32 { get; set; }

        [PacketProperty(3)]
        public UInt64 UInt64 { get; set; }

        [PacketProperty(2)]
        public string String { get; set; }

        [PacketProperty(1)]
        public PacketGuid Guid { get; set; }

        public int NotSerialized { get; set; }
    }
}
