using Arci.Networking.Data;
using Arci.Networking.Object;
using Arci.Networking.Tests.ObjectTests.Objects;
using System.Collections.Generic;
using System.Linq;
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
                String = "1234",
                Guid = new PacketGuid(9999)
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.Equal(1, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.True(readPacket.ReadBit()); // String value present
            Assert.True(readPacket.ReadBit()); // Guid value present
            readPacket.ClearUnflushedBits();
            Assert.Equal(obj.SByte, readPacket.ReadSByte());
            Assert.Equal(obj.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.String, readPacket.ReadString());
            Assert.Equal(obj.Guid, readPacket.ReadPacketGuid());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject1>(readPacket);
            Assert.Equal(obj.SByte, deserializedObject.SByte);
            Assert.Equal(obj.UInt16, deserializedObject.UInt16);
            Assert.Equal(obj.Int32, deserializedObject.Int32);
            Assert.Equal(obj.UInt64, deserializedObject.UInt64);
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
                String = "1234",
                Guid = new PacketGuid(9999),
                NotSerialized = 10
            };

            var packet = PacketObject.ToPacket(obj);
            Assert.Equal(2, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);

            Assert.True(readPacket.ReadBit()); // Guid value present
            Assert.True(readPacket.ReadBit()); // String value present
            readPacket.ClearUnflushedBits();
            Assert.Equal(obj.Guid, readPacket.ReadPacketGuid());
            Assert.Equal(obj.String, readPacket.ReadString());
            Assert.Equal(obj.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject2>(readPacket);
            Assert.Equal(obj.SByte, deserializedObject.SByte);
            Assert.Equal(obj.UInt16, deserializedObject.UInt16);
            Assert.Equal(obj.Int32, deserializedObject.Int32);
            Assert.Equal(obj.UInt64, deserializedObject.UInt64);
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
        }

        [Fact]
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
                    Guid = new PacketGuid(9999)
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
            Assert.Equal(3, packet.OpcodeNumber);

            var readPacket = new Packet(packet.Data);
            Assert.True(readPacket.ReadBit()); // Object1 value present
            Assert.True(readPacket.ReadBit()); // Object2 value present
            readPacket.ClearUnflushedBits();

            Assert.True(readPacket.ReadBit()); // String value present
            Assert.True(readPacket.ReadBit()); // Guid value present
            readPacket.ClearUnflushedBits();
            Assert.Equal(obj.Object1.SByte, readPacket.ReadSByte());
            Assert.Equal(obj.Object1.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.Object1.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.Object1.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.Object1.String, readPacket.ReadString());
            Assert.Equal(obj.Object1.Guid, readPacket.ReadPacketGuid());

            Assert.True(readPacket.ReadBit()); // Guid value present
            Assert.True(readPacket.ReadBit()); // String value present
            readPacket.ClearUnflushedBits();
            Assert.Equal(obj.Object2.Guid, readPacket.ReadPacketGuid());
            Assert.Equal(obj.Object2.String, readPacket.ReadString());
            Assert.Equal(obj.Object2.UInt64, readPacket.ReadUInt64());
            Assert.Equal(obj.Object2.Int32, readPacket.ReadInt32());
            Assert.Equal(obj.Object2.UInt16, readPacket.ReadUInt16());
            Assert.Equal(obj.Object2.SByte, readPacket.ReadSByte());
            readPacket.Dispose();

            readPacket = new Packet(packet.Data);
            var deserializedObject = PacketObject.FromPacket<TestObject3>(readPacket);
            Assert.Equal(obj.Object1.SByte, deserializedObject.Object1.SByte);
            Assert.Equal(obj.Object1.UInt16, deserializedObject.Object1.UInt16);
            Assert.Equal(obj.Object1.Int32, deserializedObject.Object1.Int32);
            Assert.Equal(obj.Object1.UInt64, deserializedObject.Object1.UInt64);
            Assert.Equal(obj.Object1.String, deserializedObject.Object1.String);
            Assert.Equal(obj.Object1.Guid, deserializedObject.Object1.Guid);
            Assert.Equal(obj.Object2.SByte, deserializedObject.Object2.SByte);
            Assert.Equal(obj.Object2.UInt16, deserializedObject.Object2.UInt16);
            Assert.Equal(obj.Object2.Int32, deserializedObject.Object2.Int32);
            Assert.Equal(obj.Object2.UInt64, deserializedObject.Object2.UInt64);
            Assert.Equal(obj.Object2.String, deserializedObject.Object2.String);
            Assert.Equal(obj.Object2.Guid, deserializedObject.Object2.Guid);
            Assert.NotEqual(obj.Object2.NotSerialized, deserializedObject.Object2.NotSerialized);

            readPacket.Dispose();
            packet.Dispose();
        }

        [Fact]
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

            Assert.Equal(obj.Object1.SByte, deserializedObject.Object1.SByte);
            Assert.Equal(obj.Object1.UInt16, deserializedObject.Object1.UInt16);
            Assert.Equal(obj.Object1.Int32, deserializedObject.Object1.Int32);
            Assert.Equal(obj.Object1.UInt64, deserializedObject.Object1.UInt64);
            Assert.Equal(obj.Object1.String, deserializedObject.Object1.String);
            Assert.Equal(obj.Object1.Guid, deserializedObject.Object1.Guid);
            Assert.Null(deserializedObject.NotSerializedObject2);
            Assert.NotNull(deserializedObject.Object3);
            Assert.Null(deserializedObject.Object3.Object1);
            Assert.Null(deserializedObject.Object3.Object2);
            Assert.Equal(obj.NullableInt, deserializedObject.NullableInt);
            Assert.Equal(obj.NullableInt2, deserializedObject.NullableInt2);
            Assert.Null(deserializedObject.NotSerializedString);
            Assert.Equal(obj.String, deserializedObject.String);
        }

        [Fact]
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

            Assert.Equal(obj.ArrayOfString.Length, deserializedObject.ArrayOfString.Length);
            Assert.Equal(obj.ListOfInt.Count, deserializedObject.ListOfInt.Count);
            Assert.Equal(obj.ListOfObject.Count, deserializedObject.ListOfObject.Count);
            Assert.True(deserializedObject.ArrayOfString.SequenceEqual(obj.ArrayOfString));
            Assert.True(deserializedObject.ListOfInt.SequenceEqual(obj.ListOfInt));
        }
    }
}
