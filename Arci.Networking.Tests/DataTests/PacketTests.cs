﻿using Arci.Networking.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System;

namespace Arci.Networking.Tests.DataTests
{
    [TestClass]
    public class PacketTests : BaseTestClass
    {
        [TestMethod]
        public void WriteReadByteDataTest()
        {
            Packet writePacket = new Packet(packetOpcode);
            writePacket.Write(uint32Value);
            writePacket.Write(uint16Value);
            writePacket.Write(byteValue);

            writePacket.Write(int32Value);
            writePacket.Write(int16Value);
            writePacket.Write(sbyteValue);

            writePacket.Write(stringValue);
            writePacket.Write(byteArrayValue);

            Packet readPacket = new Packet(writePacket.Data);

            Assert.AreEqual(writePacket.OpcodeNumber, readPacket.OpcodeNumber);
            Assert.AreEqual(uint32Value, readPacket.ReadUInt32());
            Assert.AreEqual(uint16Value, readPacket.ReadUInt16());
            Assert.AreEqual(byteValue, readPacket.ReadByte());
            Assert.AreEqual(int32Value, readPacket.ReadInt32());
            Assert.AreEqual(int16Value, readPacket.ReadInt16());
            Assert.AreEqual(sbyteValue, readPacket.ReadSByte());
            Assert.AreEqual(stringValue, readPacket.ReadString());
            var packetBytes = readPacket.ReadBytes();
            for (int i = 0; i < byteArrayValue.Length; i++)
                Assert.AreEqual(byteArrayValue[i], packetBytes[i]);

            readPacket.Dispose();
            writePacket.Dispose();
        }

        [TestMethod]
        public void WriteReadBitDataTest()
        {
            Packet writePacket = new Packet(packetOpcode);
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

            int bVal = 0;
            Packet readPacket = new Packet(writePacket.Data);
            for (int i = 0; i < 8; i++)
                if (readPacket.ReadBit())
                    bVal |= 1 << i;

            Assert.AreEqual(byteValue, bVal);
            Assert.IsTrue(readPacket.ReadBit());
            Assert.IsFalse(readPacket.ReadBit());

            readPacket.Dispose();
            writePacket.Dispose();
        }

        [TestMethod]
        public void InitPacketTests()
        {
            Packet packet1 = new Packet(packetOpcode);
            Packet packet2 = new Packet(clientEnumOpcode);

            Assert.AreEqual(packetOpcode, packet1.OpcodeNumber);
            Assert.AreEqual(clientEnumOpcode, (ClientPacketTypes)packet2.OpcodeNumber);

            packet1.Initialize(packetOpcode & 0xF);
            packet2.Initialize(serverEnumOpcode);

            Assert.AreEqual(packetOpcode & 0xF, packet1.OpcodeNumber);
            Assert.AreEqual(serverEnumOpcode, (ServerPacketTypes)packet2.OpcodeNumber);

            packet1.Dispose();
            packet2.Dispose();
        }
    }
}