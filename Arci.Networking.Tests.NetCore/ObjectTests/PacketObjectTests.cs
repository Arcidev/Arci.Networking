using Arci.Networking.Data;
using Arci.Networking.Object;
using Arci.Networking.Tests.NetCore.ObjectTests.Objects;
using Xunit;

namespace Arci.Networking.Tests.NetCore.ObjectTests
{
    public class PacketObjectTests
    {
        [Fact]
        public void TestObjectSerialization()
        {
            var obj = new TestObject1()
            {
                SByte = 1,
                UInt16 = 2,
                Int32 = 3,
                UInt64 = 4,
                UInt16As32 = 5,
                String = "1234",
                Guid = new PacketGuid(9999)
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.Equal(1, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.Equal(obj.SByte, readPacket.ReadSByte());
            Assert.Equal(obj.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.UInt16As32, readPacket.ReadUInt32());
            Assert.Equal(obj.String, readPacket.ReadString());
            Assert.Equal(obj.Guid, readPacket.ReadPacketGuid());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = (TestObject1)PacketObject.FromPacket(readPacket);
            Assert.Equal(obj.SByte, deserializedObject.SByte);
            Assert.Equal(obj.UInt16, deserializedObject.UInt16);
            Assert.Equal(obj.Int32, deserializedObject.Int32);
            Assert.Equal(obj.UInt64, deserializedObject.UInt64);
            Assert.Equal(obj.UInt16As32, deserializedObject.UInt16As32);
            Assert.Equal(obj.String, deserializedObject.String);
            Assert.Equal(obj.Guid, deserializedObject.Guid);

            readPacket.Dispose();
            packet.Dispose();
        }

        [Fact]
        public void TestObjectSerializationOrder()
        {
            var obj = new TestObject2()
            {
                SByte = 1,
                UInt16 = 2,
                Int32 = 3,
                UInt64 = 4,
                UInt16As32 = 5,
                String = "1234",
                Guid = new PacketGuid(9999),
                NotSerialized = 10
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.Equal(2, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.Equal(obj.Guid, readPacket.ReadPacketGuid());
            Assert.Equal(obj.String, readPacket.ReadString());
            Assert.Equal(obj.UInt16As32, readPacket.ReadUInt32());
            Assert.Equal(obj.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = (TestObject2)PacketObject.FromPacket(readPacket);
            Assert.Equal(obj.SByte, deserializedObject.SByte);
            Assert.Equal(obj.UInt16, deserializedObject.UInt16);
            Assert.Equal(obj.Int32, deserializedObject.Int32);
            Assert.Equal(obj.UInt64, deserializedObject.UInt64);
            Assert.Equal(obj.UInt16As32, deserializedObject.UInt16As32);
            Assert.Equal(obj.String, deserializedObject.String);
            Assert.Equal(obj.Guid, deserializedObject.Guid);
            Assert.NotEqual(obj.NotSerialized, deserializedObject.NotSerialized);

            readPacket.Dispose();
            packet.Dispose();
        }

        [Fact]
        public void TestUnserializableObject()
        {
            var packet = PacketObject.ToPacket(new object());
            Assert.Null(packet);

            var obj = PacketObject.FromPacket(new Packet(0));
            Assert.Null(obj);
        }
    }
}
