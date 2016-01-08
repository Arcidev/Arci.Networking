using Arci.Networking.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Guid = Arci.Networking.Data.Guid;

namespace Arci.Networking.Tests.DataTests
{
    [TestClass]
    public class GuidTests : BaseTestClass
    {
        [TestMethod]
        public void WriteGuidTests()
        {
            Packet writePacket = new Packet(packetOpcode);
            writePacket.WriteGuidBitStreamInOrder(guidValues[0], 0, 1, 2, 3, 4, 5, 6, 7);
            writePacket.WriteGuidBitStreamInOrder(guidValues[1], 7, 6, 5);
            writePacket.WriteGuidBitStreamInOrder(guidValues[2], 0, 1, 2, 3);
            writePacket.WriteGuidBitStreamInOrder(guidValues[1], 4, 3, 2);
            writePacket.WriteGuidBitStreamInOrder(guidValues[2], 7, 6, 5, 4);
            writePacket.WriteGuidBitStreamInOrder(guidValues[1], 1, 0);
            writePacket.FlushBits(); // Not required as we've written the whole bytes (3 * 8 bits == 3 bytes)

            writePacket.WriteGuidByteStreamInOrder(guidValues[1], 0, 1, 2, 3, 4, 5, 6, 7);
            writePacket.WriteGuidByteStreamInOrder(guidValues[2], 7, 6);
            writePacket.WriteGuidByteStreamInOrder(guidValues[0], 0, 1, 2, 3, 7, 6, 5, 4);
            writePacket.WriteGuidByteStreamInOrder(guidValues[2], 5, 4, 3, 2, 1, 0);

            var guids = new Guid[3];
            for (int i = 0; i < guids.Length; i++)
                guids[i] = new Guid();

            Packet readPacket = new Packet(writePacket.Data);
            readPacket.ReadGuidBitStreamInOrder(guids[0], 0, 1, 2, 3, 4, 5, 6, 7);
            readPacket.ReadGuidBitStreamInOrder(guids[1], 7, 6, 5);
            readPacket.ReadGuidBitStreamInOrder(guids[2], 0, 1, 2, 3);
            readPacket.ReadGuidBitStreamInOrder(guids[1], 4, 3, 2);
            readPacket.ReadGuidBitStreamInOrder(guids[2], 7, 6, 5, 4);
            readPacket.ReadGuidBitStreamInOrder(guids[1], 1, 0);

            readPacket.ReadGuidByteStreamInOrder(guids[1], 0, 1, 2, 3, 4, 5, 6, 7);
            readPacket.ReadGuidByteStreamInOrder(guids[2], 7, 6);
            readPacket.ReadGuidByteStreamInOrder(guids[0], 0, 1, 2, 3, 7, 6, 5, 4);
            readPacket.ReadGuidByteStreamInOrder(guids[2], 5, 4, 3, 2, 1, 0);

            Assert.AreEqual(guidValues[0], guids[0]);
            Assert.AreEqual(guidValues[1], guids[1]);
            Assert.AreEqual(guidValues[2], guids[2]);

            readPacket.Dispose();
            writePacket.Dispose();
        }

        [TestMethod]
        public void MatchTests()
        {
            var guid1 = guidValues[0];
            var guid2 = guidValues[1];
            Assert.IsTrue(guidValues[0] == guid1); // Testing against different adress
            Assert.IsTrue(guid2 != guid1);
            Assert.IsFalse(guid1 == null);
            Assert.IsFalse(null == guid2);
            Assert.IsTrue(guid1 != null);
            Assert.IsTrue(null != guid2);
            Assert.IsFalse(guid1.Equals(guid2));
            Assert.AreNotEqual(guid1.GetHashCode(), guid2.GetHashCode());
        }

        [TestMethod]
        public void ConversionOperatorsTests()
        {
            var guid1 = new Guid(uint64Value);
            var guid2 = (Guid)uint64Value;
            var uint64Val = (UInt64)guid1;

            Assert.AreEqual(guid1, guid2);
            Assert.AreEqual(uint64Value, uint64Val);
        }

        [TestMethod]
        public void IndexTests()
        {
            var guid1 = guidValues[0];
            var guid2 = new Guid();

            for (int i = 0; i < 8; i++)
                guid2[i] = guid1[i];

            Assert.AreEqual(guid1, guid2);
        }
    }
}
