using Arci.Networking.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;

namespace Arci.Networking.Tests.DataTests
{
    [TestClass]
    public class PacketTests : BaseTestClass
    {
        [TestMethod]
        public void WriteReadByteDataTest()
        {
            using (var writePacket = new Packet(packetOpcode))
            {
                writePacket.Write(uint32Value);
                writePacket.Write(uint16Value);
                writePacket.Write(byteValue);

                writePacket.Write(int32Value);
                writePacket.Write(int16Value);
                writePacket.Write(sbyteValue);

                writePacket.Write(stringValue);
                writePacket.Write(byteArrayValue);

                using (var readPacket = new Packet(writePacket.Data))
                {
                    Assert.AreEqual(writePacket.OpcodeNumber, readPacket.OpcodeNumber);
                    Assert.AreEqual(uint32Value, readPacket.ReadUInt32());
                    Assert.AreEqual(uint16Value, readPacket.ReadUInt16());
                    Assert.AreEqual(byteValue, readPacket.ReadByte());
                    Assert.AreEqual(int32Value, readPacket.ReadInt32());
                    Assert.AreEqual(int16Value, readPacket.ReadInt16());
                    Assert.AreEqual(sbyteValue, readPacket.ReadSByte());
                    Assert.AreEqual(stringValue, readPacket.ReadString());
                    CollectionAssert.AreEqual(byteArrayValue, readPacket.ReadBytes());
                }
            }
        }

        [TestMethod]
        public void WriteReadBitDataTest()
        {
            using (var writePacket = new Packet(packetOpcode))
            {
                writePacket.WriteBit(byteValue & 0x1);
                writePacket.WriteBit(byteValue & 0x2);
                writePacket.WriteBit(byteValue & 0x4);
                writePacket.WriteBit(byteValue & 0x8);
                writePacket.WriteBit(byteValue & 0x10);
                writePacket.WriteBit(byteValue & 0x20);
                writePacket.WriteBit(byteValue & 0x40);
                writePacket.WriteBit(byteValue & 0x80);
                writePacket.WriteBit(true);
                writePacket.WriteBit(false);
                writePacket.FlushBits();

                using (var readPacket = new Packet(writePacket.Data))
                {
                    int bVal = 0;
                    for (int i = 0; i < 8; i++)
                        if (readPacket.ReadBit())
                            bVal |= 1 << i;

                    Assert.AreEqual(byteValue, bVal);
                    Assert.IsTrue(readPacket.ReadBit());
                    Assert.IsFalse(readPacket.ReadBit());
                }
            }
        }

        [TestMethod]
        public void InitPacketTests()
        {
            using (Packet packet1 = new Packet(packetOpcode), packet2 = new Packet(clientEnumOpcode))
            {
                Assert.AreEqual(packetOpcode, packet1.OpcodeNumber);
                Assert.AreEqual(clientEnumOpcode, (ClientPacketTypes)packet2.OpcodeNumber);

                packet1.Initialize(packetOpcode & 0xF);
                packet2.Initialize(serverEnumOpcode);

                Assert.AreEqual(packetOpcode & 0xF, packet1.OpcodeNumber);
                Assert.AreEqual(serverEnumOpcode, (ServerPacketTypes)packet2.OpcodeNumber);
            }
        }
    }
}
