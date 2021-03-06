﻿using Arci.Networking.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Arci.Networking.Tests.BuilderTests
{
    [TestClass]
    public class PacketBuilderTests
    {
        [TestMethod]
        public void TestPacketBuild()
        {
            var buildedPacket = new Packet(0).Builder()
                .WriteBit(false).WriteBit(true).WriteBit(9).FlushBits()
                .WriteGuidBitStreamInOrder(123456, 1, 2, 3, 4, 5, 6, 7, 0)
                .Write((byte)1).Write((UInt16)2).Write((UInt32)3).Write((sbyte)4).Write((Int16)5).Write(6)
                .Write(new byte[] { 7, 8, 9 })
                .WriteGuidByteStreamInOrder(123456, 1, 2, 3, 4, 5, 6, 7, 0)
                .Build();

            var packet = new Packet(0);
            packet.WriteBit(false);
            packet.WriteBit(true);
            packet.WriteBit(9);
            packet.FlushBits();
            packet.WriteGuidBitStreamInOrder(123456, 1, 2, 3, 4, 5, 6, 7, 0);
            packet.Write((byte)1);
            packet.Write((UInt16)2);
            packet.Write((UInt32)3);
            packet.Write((sbyte)4);
            packet.Write((Int16)5);
            packet.Write(6);
            packet.Write(new byte[] { 7, 8, 9 });
            packet.WriteGuidByteStreamInOrder(123456, 1, 2, 3, 4, 5, 6, 7, 0);

            CollectionAssert.AreEqual(packet.Data, buildedPacket.Data);
            buildedPacket.Dispose();
            packet.Dispose();
        }
    }
}
