using Arci.Networking.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arci.Networking.Tests.DataTests
{
    [TestClass]
    public class ByteBufferTests : BaseTestClass
    {
        [TestMethod]
        public void AppendByteBufferTest()
        {
            using (var writePacket = new Packet(packetOpcode))
            {
                ByteBuffer buffer1 = new ByteBuffer();
                ByteBuffer buffer2 = new ByteBuffer();

                for (int i = 0; i < byteArrayValue.Length; i++)
                {
                    writePacket.Write(byteArrayValue[i]);
                    buffer1.Write(byteArrayValue[i]);
                    buffer2.Write(byteArrayValue[byteArrayValue.Length - (i + 1)]);
                }

                writePacket.Write(buffer1);
                writePacket.Write(buffer2);

                using (var readPacket = new Packet(writePacket.Data))
                {
                    for (int i = 0; i < byteArrayValue.Length; i++)
                        Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());

                    for (int i = 0; i < byteArrayValue.Length; i++)
                        Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());

                    for (int i = byteArrayValue.Length - 1; i >= 0; i--)
                        Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());
                }
            }
        }

        [TestMethod]
        public void TestWriteBits()
        {
            byte val = 0x1 + 0x4 + 0x10;
            var packet1 = new Packet(packetOpcode);
            packet1.WriteBits(val, 5);
            packet1.FlushBits();

            var packet2 = new Packet(packet1.Data);
            Assert.AreEqual(val, packet2.ReadBits(5));

            packet1.Dispose();
            packet1.Dispose();
        }

        [TestMethod]
        public void TestWriteBits2()
        {
            var val = uint.MaxValue;
            var packet1 = new Packet(packetOpcode);
            packet1.WriteBits(val, 16);
            packet1.FlushBits();

            var packet2 = new Packet(packet1.Data);
            Assert.AreEqual(ushort.MaxValue, packet2.ReadBits(16));

            packet1.Dispose();
            packet1.Dispose();
        }
    }
}
