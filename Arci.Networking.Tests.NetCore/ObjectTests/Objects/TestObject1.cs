using Arci.Networking.Data;
using Arci.Networking.Object.Attributes;
using System;

namespace Arci.Networking.Tests.NetCore.ObjectTests.Objects
{
    [PacketClass(1)]
    public class TestObject1
    {
        [PacketProperty(1)]
        public SByte SByte { get; set; }

        [PacketProperty(2)]
        public UInt16 UInt16 { get; set; }

        [PacketProperty(3)]
        public Int32 Int32 { get; set; }

        [PacketProperty(4)]
        public UInt64 UInt64 { get; set; }

        [PacketProperty(5)]
        public string String { get; set; }

        [PacketProperty(6)]
        public PacketGuid Guid { get; set; }
    }
}
