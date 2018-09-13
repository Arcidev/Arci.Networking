
using Arci.Networking.Data;
using System;

namespace Arci.Networking.Serialization
{
    public static partial class PacketSerializer
    {
        private static object ReadPacketProperty(ByteBuffer byteBuffer, Type type)
        {
            switch(Type.GetTypeCode(type))
            {
                
                case TypeCode.Int16:
                    return byteBuffer.ReadInt16();
                
                case TypeCode.Int32:
                    return byteBuffer.ReadInt32();
                
                case TypeCode.SByte:
                    return byteBuffer.ReadSByte();
                
                case TypeCode.Int64:
                    return byteBuffer.ReadInt64();
                
                case TypeCode.UInt16:
                    return byteBuffer.ReadUInt16();
                
                case TypeCode.UInt32:
                    return byteBuffer.ReadUInt32();
                
                case TypeCode.Byte:
                    return byteBuffer.ReadByte();
                
                case TypeCode.UInt64:
                    return byteBuffer.ReadUInt64();
                
                case TypeCode.String:
                    return byteBuffer.ReadString();
                
                case TypeCode.Single:
                    return byteBuffer.ReadSingle();
                
                case TypeCode.Double:
                    return byteBuffer.ReadDouble();
                
                case TypeCode.Decimal:
                    return byteBuffer.ReadDecimal();
                
                case TypeCode.DateTime:
                    return byteBuffer.ReadDateTime();
                
                default:
                    return ReadPacketObject(byteBuffer, type);
            }
        }

        private static void WritePacketProperty(ByteBuffer byteBuffer, object value, Type type)
        {
            switch(Type.GetTypeCode(type))
            {
                
                case TypeCode.Int16:
                    byteBuffer.Write((Int16)value);
                    break;
                
                case TypeCode.Int32:
                    byteBuffer.Write((Int32)value);
                    break;
                
                case TypeCode.SByte:
                    byteBuffer.Write((SByte)value);
                    break;
                
                case TypeCode.Int64:
                    byteBuffer.Write((Int64)value);
                    break;
                
                case TypeCode.UInt16:
                    byteBuffer.Write((UInt16)value);
                    break;
                
                case TypeCode.UInt32:
                    byteBuffer.Write((UInt32)value);
                    break;
                
                case TypeCode.Byte:
                    byteBuffer.Write((Byte)value);
                    break;
                
                case TypeCode.UInt64:
                    byteBuffer.Write((UInt64)value);
                    break;
                
                case TypeCode.String:
                    byteBuffer.Write((String)value);
                    break;
                
                case TypeCode.Single:
                    byteBuffer.Write((Single)value);
                    break;
                
                case TypeCode.Double:
                    byteBuffer.Write((Double)value);
                    break;
                
                case TypeCode.Decimal:
                    byteBuffer.Write((Decimal)value);
                    break;
                
                case TypeCode.DateTime:
                    byteBuffer.Write((DateTime)value);
                    break;
                
                default:
                    WritePacketObject(byteBuffer, value, type);
                    break;
            }
        }
    }
}
