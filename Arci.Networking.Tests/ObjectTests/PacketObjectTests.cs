using Arci.Networking.Data;
using Arci.Networking.Object;
using Arci.Networking.Tests.ObjectTests.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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
                String = "1234",
                Guid = new PacketGuid(9999),
                DateTime = DateTime.Now
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.AreEqual(1, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.IsTrue(readPacket.ReadBit()); // String value present
            Assert.IsTrue(readPacket.ReadBit()); // Guid value present
            readPacket.ClearUnflushedBits();
            Assert.AreEqual(obj.SByte, readPacket.ReadSByte());
            Assert.AreEqual(obj.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.String, readPacket.ReadString());
            Assert.AreEqual(obj.Guid, readPacket.ReadPacketGuid());
            Assert.AreEqual(obj.DateTime, readPacket.ReadDateTime());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject1>(readPacket);
            Assert.AreEqual(obj.SByte, deserializedObject.SByte);
            Assert.AreEqual(obj.UInt16, deserializedObject.UInt16);
            Assert.AreEqual(obj.Int32, deserializedObject.Int32);
            Assert.AreEqual(obj.UInt64, deserializedObject.UInt64);
            Assert.AreEqual(obj.String, deserializedObject.String);
            Assert.AreEqual(obj.Guid, deserializedObject.Guid);
            Assert.AreEqual(obj.DateTime, deserializedObject.DateTime);

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
                String = "1234",
                Guid = new PacketGuid(9999),
                NotSerialized = 10
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.AreEqual(2, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);

            Assert.IsTrue(readPacket.ReadBit()); // Guid value present
            Assert.IsTrue(readPacket.ReadBit()); // String value present
            readPacket.ClearUnflushedBits();
            Assert.AreEqual(obj.Guid, readPacket.ReadPacketGuid());
            Assert.AreEqual(obj.String, readPacket.ReadString());
            Assert.AreEqual(obj.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject2>(readPacket);
            Assert.AreEqual(obj.SByte, deserializedObject.SByte);
            Assert.AreEqual(obj.UInt16, deserializedObject.UInt16);
            Assert.AreEqual(obj.Int32, deserializedObject.Int32);
            Assert.AreEqual(obj.UInt64, deserializedObject.UInt64);
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
        }

        [TestMethod]
        public void TestPropertyClassSerialization()
        {
            var obj = new TestObject3
            {
                Object1 = new TestObject1()
                {
                    SByte = 1,
                    UInt16 = 2,
                    Int32 = 3,
                    UInt64 = 4,
                    String = "1234",
                    Guid = new PacketGuid(9999),
                    DateTime = DateTime.Now
                },
                Object2 = new TestObject2()
                {
                    SByte = 6,
                    UInt16 = 7,
                    Int32 = 8,
                    UInt64 = 9,
                    String = "5678",
                    Guid = new PacketGuid(1111),
                    NotSerialized = 11
                }
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.AreEqual(3, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.IsTrue(readPacket.ReadBit()); // Object1 value present
            Assert.IsTrue(readPacket.ReadBit()); // Object2 value present
            readPacket.ClearUnflushedBits();

            Assert.IsTrue(readPacket.ReadBit()); // String value present
            Assert.IsTrue(readPacket.ReadBit()); // Guid value present
            readPacket.ClearUnflushedBits();
            Assert.AreEqual(obj.Object1.SByte, readPacket.ReadSByte());
            Assert.AreEqual(obj.Object1.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.Object1.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.Object1.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.Object1.String, readPacket.ReadString());
            Assert.AreEqual(obj.Object1.Guid, readPacket.ReadPacketGuid());
            Assert.AreEqual(obj.Object1.DateTime, readPacket.ReadDateTime());

            Assert.IsTrue(readPacket.ReadBit()); // Guid value present
            Assert.IsTrue(readPacket.ReadBit()); // String value present
            readPacket.ClearUnflushedBits();
            Assert.AreEqual(obj.Object2.Guid, readPacket.ReadPacketGuid());
            Assert.AreEqual(obj.Object2.String, readPacket.ReadString());
            Assert.AreEqual(obj.Object2.UInt64, readPacket.ReadUInt64());
            Assert.AreEqual(obj.Object2.Int32, readPacket.ReadInt32());
            Assert.AreEqual(obj.Object2.UInt16, readPacket.ReadUInt16());
            Assert.AreEqual(obj.Object2.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject3>(readPacket);
            Assert.AreEqual(obj.Object1.SByte, deserializedObject.Object1.SByte);
            Assert.AreEqual(obj.Object1.UInt16, deserializedObject.Object1.UInt16);
            Assert.AreEqual(obj.Object1.Int32, deserializedObject.Object1.Int32);
            Assert.AreEqual(obj.Object1.UInt64, deserializedObject.Object1.UInt64);
            Assert.AreEqual(obj.Object1.String, deserializedObject.Object1.String);
            Assert.AreEqual(obj.Object1.Guid, deserializedObject.Object1.Guid);
            Assert.AreEqual(obj.Object1.DateTime, deserializedObject.Object1.DateTime);
            Assert.AreEqual(obj.Object2.SByte, deserializedObject.Object2.SByte);
            Assert.AreEqual(obj.Object2.UInt16, deserializedObject.Object2.UInt16);
            Assert.AreEqual(obj.Object2.Int32, deserializedObject.Object2.Int32);
            Assert.AreEqual(obj.Object2.UInt64, deserializedObject.Object2.UInt64);
            Assert.AreEqual(obj.Object2.String, deserializedObject.Object2.String);
            Assert.AreEqual(obj.Object2.Guid, deserializedObject.Object2.Guid);
            Assert.AreNotEqual(obj.Object2.NotSerialized, deserializedObject.Object2.NotSerialized);

            readPacket.Dispose();
            packet.Dispose();
        }

        [TestMethod]
        public void TestNullPropertySerialization()
        {
            var obj = new TestObject4
            {
                Object1 = new TestObject1()
                {
                    SByte = 1,
                    UInt16 = 2,
                    Int32 = 3,
                    UInt64 = 4,
                    String = "1234",
                    Guid = new PacketGuid(9999)
                },
                NotSerializedObject2 = new TestObject2(),
                Object3 = new TestObject3(),
                NullableInt = 5,
                NullableInt2 = null,
                NotSerializedString = "6789",
                String = "test_string1"
            };

            var packet = PacketObject.ToPacket(obj);
            var readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject4>(readPacket);
            packet.Dispose();
            readPacket.Dispose();

            Assert.AreEqual(obj.Object1.SByte, deserializedObject.Object1.SByte);
            Assert.AreEqual(obj.Object1.UInt16, deserializedObject.Object1.UInt16);
            Assert.AreEqual(obj.Object1.Int32, deserializedObject.Object1.Int32);
            Assert.AreEqual(obj.Object1.UInt64, deserializedObject.Object1.UInt64);
            Assert.AreEqual(obj.Object1.String, deserializedObject.Object1.String);
            Assert.AreEqual(obj.Object1.Guid, deserializedObject.Object1.Guid);
            Assert.IsNull(deserializedObject.NotSerializedObject2);
            Assert.IsNotNull(deserializedObject.Object3);
            Assert.IsNull(deserializedObject.Object3.Object1);
            Assert.IsNull(deserializedObject.Object3.Object2);
            Assert.AreEqual(obj.NullableInt, deserializedObject.NullableInt);
            Assert.AreEqual(obj.NullableInt2, deserializedObject.NullableInt2);
            Assert.IsNull(deserializedObject.NotSerializedString);
            Assert.AreEqual(obj.String, deserializedObject.String);
        }

        [TestMethod]
        public void TestCollectionSerialization()
        {
            var obj = new TestObject5()
            {
                ArrayOfString = new string[] { "123", "456", "789" },
                ListOfInt = new List<int>() { 1, 2, 3 },
                ListOfObject = new List<TestObject3>() { new TestObject3(), new TestObject3(), new TestObject3() }
            };

            var packet = PacketObject.ToPacket(obj);
            var readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject5>(readPacket);
            packet.Dispose();
            readPacket.Dispose();

            Assert.AreEqual(obj.ArrayOfString.Length, deserializedObject.ArrayOfString.Length);
            Assert.AreEqual(obj.ListOfInt.Count, deserializedObject.ListOfInt.Count);
            Assert.AreEqual(obj.ListOfObject.Count, deserializedObject.ListOfObject.Count);
            Assert.IsTrue(deserializedObject.ArrayOfString.SequenceEqual(obj.ArrayOfString));
            Assert.IsTrue(deserializedObject.ListOfInt.SequenceEqual(obj.ListOfInt));
        }
    }
}
