using Arci.Networking.Data;
using Arci.Networking.Object;
using Arci.Networking.Tests.ObjectTests.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arci.Networking.Tests.ObjectTests
{
    [TestClass]
    public class PacketObjectTests
    {
        [TestMethod]
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
            Assert.AreEqual(1, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.AreEqual(obj.SByte, readPacket.ReadSByte());
            Assert.AreEqual(obj.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.UInt16As32, readPacket.ReadUInt32());
            Assert.AreEqual(obj.String, readPacket.ReadString());
            Assert.AreEqual(obj.Guid, readPacket.ReadPacketGuid());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = (TestObject1)PacketObject.FromPacket(readPacket);
            Assert.AreEqual(obj.SByte, deserializedObject.SByte);
            Assert.AreEqual(obj.UInt16, deserializedObject.UInt16);
            Assert.AreEqual(obj.Int32, deserializedObject.Int32);
            Assert.AreEqual(obj.UInt64, deserializedObject.UInt64);
            Assert.AreEqual(obj.UInt16As32, deserializedObject.UInt16As32);
            Assert.AreEqual(obj.String, deserializedObject.String);
            Assert.AreEqual(obj.Guid, deserializedObject.Guid);

            readPacket.Dispose();
            packet.Dispose();
        }

        [TestMethod]
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
            Assert.AreEqual(2, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.AreEqual(obj.Guid, readPacket.ReadPacketGuid());
            Assert.AreEqual(obj.String, readPacket.ReadString());
            Assert.AreEqual(obj.UInt16As32, readPacket.ReadUInt32());
            Assert.AreEqual(obj.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = (TestObject2)PacketObject.FromPacket(readPacket);
            Assert.AreEqual(obj.SByte, deserializedObject.SByte);
            Assert.AreEqual(obj.UInt16, deserializedObject.UInt16);
            Assert.AreEqual(obj.Int32, deserializedObject.Int32);
            Assert.AreEqual(obj.UInt64, deserializedObject.UInt64);
            Assert.AreEqual(obj.UInt16As32, deserializedObject.UInt16As32);
            Assert.AreEqual(obj.String, deserializedObject.String);
            Assert.AreEqual(obj.Guid, deserializedObject.Guid);
            Assert.AreNotEqual(obj.NotSerialized, deserializedObject.NotSerialized);

            readPacket.Dispose();
            packet.Dispose();
        }

        [TestMethod]
        public void TestUnserializableObject()
        {
            var packet = PacketObject.ToPacket(new object());
            Assert.IsNull(packet);

            var obj = PacketObject.FromPacket(new Packet(0));
            Assert.IsNull(obj);
        }
    }
}
