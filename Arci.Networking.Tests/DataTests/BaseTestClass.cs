using Arci.Networking.Data;
using Shared;
using System;

namespace Arci.Networking.Tests.DataTests
{
    public class BaseTestClass
    {
        protected const UInt16 packetOpcode = 255;
        protected const UInt64 uint64Value = 0xAABBCCDDEEFF1199;
        protected const UInt32 uint32Value = 1;
        protected const UInt16 uint16Value = 2;
        protected const byte byteValue = 3;
        protected const Int32 int32Value = 4;
        protected const Int16 int16Value = 5;
        protected const sbyte sbyteValue = 6;
        protected const string stringValue = "Hello World";
        protected const ClientPacketTypes clientEnumOpcode = ClientPacketTypes.CMSG_INIT_ENCRYPTED_AES;
        protected const ServerPacketTypes serverEnumOpcode = ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_RSA;

        protected static readonly PacketGuid[] guidValues = new PacketGuid[] { new PacketGuid(uint64Value), new PacketGuid(0xDDEEFF1199AABBCC), new PacketGuid(0xAA1199BBCCDDEEFF) };
        protected static readonly byte[] byteArrayValue = new byte[] { 1, 2, 3, 4 };
    }
}
