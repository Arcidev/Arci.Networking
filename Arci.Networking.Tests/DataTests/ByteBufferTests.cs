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
            Packet writePacket = new Packet(packetOpcode);
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

            Packet readPacket = new Packet(writePacket.Data);
            for (int i = 0; i < byteArrayValue.Length; i++)
                Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());

            for (int i = 0; i < byteArrayValue.Length; i++)
                Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());

            for (int i = byteArrayValue.Length - 1; i >= 0; i--)
                Assert.AreEqual(byteArrayValue[i], readPacket.ReadByte());

            readPacket.Dispose();
            writePacket.Dispose();
        }
    }
}
