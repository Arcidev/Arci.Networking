
using Arci.Networking.Data;
using System;

namespace Arci.Networking.Object
{
    public abstract partial class PacketObject
    {
        private static object ReadPacketProperty(ByteBuffer byteBuffer, Type type)
        {
            switch(type)
            {
                
                case Type propertyType when propertyType == typeof(Int16):
                    return byteBuffer.ReadInt16();
                
                case Type propertyType when propertyType == typeof(Int32):
                    return byteBuffer.ReadInt32();
                
                case Type propertyType when propertyType == typeof(SByte):
                    return byteBuffer.ReadSByte();
                
                case Type propertyType when propertyType == typeof(Int64):
                    return byteBuffer.ReadInt64();
                
                case Type propertyType when propertyType == typeof(UInt16):
                    return byteBuffer.ReadUInt16();
                
                case Type propertyType when propertyType == typeof(UInt32):
                    return byteBuffer.ReadUInt32();
                
                case Type propertyType when propertyType == typeof(Byte):
                    return byteBuffer.ReadByte();
                
                case Type propertyType when propertyType == typeof(UInt64):
                    return byteBuffer.ReadUInt64();
                
                case Type propertyType when propertyType == typeof(String):
                    return byteBuffer.ReadString();
                
                case Type propertyType when propertyType == typeof(Single):
                    return byteBuffer.ReadSingle();
                
                case Type propertyType when propertyType == typeof(Double):
                    return byteBuffer.ReadDouble();
                
                case Type propertyType when propertyType == typeof(Decimal):
                    return byteBuffer.ReadDecimal();
                
                case Type propertyType when propertyType == typeof(Guid):
                    return byteBuffer.ReadGuid();
                case Type propertyType when propertyType == typeof(PacketGuid):
                    return byteBuffer.ReadPacketGuid();
                default:
                    if (type.IsClass)
                    {
                        var instance = Activator.CreateInstance(type);
                        ReadPacketProperties(byteBuffer, instance);
                        return instance;
                    }
                    return null;
            }
        }

        private static void WritePacketProperty(ByteBuffer byteBuffer, object value, Type type)
        {
            switch(type)
            {
                
                case Type propertyType when propertyType == typeof(Int16):
                    byteBuffer.Write(Convert.ToInt16(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Int32):
                    byteBuffer.Write(Convert.ToInt32(value));
                    break;
                
                case Type propertyType when propertyType == typeof(SByte):
                    byteBuffer.Write(Convert.ToSByte(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Int64):
                    byteBuffer.Write(Convert.ToInt64(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt16):
                    byteBuffer.Write(Convert.ToUInt16(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt32):
                    byteBuffer.Write(Convert.ToUInt32(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Byte):
                    byteBuffer.Write(Convert.ToByte(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt64):
                    byteBuffer.Write(Convert.ToUInt64(value));
                    break;
                
                case Type propertyType when propertyType == typeof(String):
                    byteBuffer.Write(Convert.ToString(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Single):
                    byteBuffer.Write(Convert.ToSingle(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Double):
                    byteBuffer.Write(Convert.ToDouble(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Decimal):
                    byteBuffer.Write(Convert.ToDecimal(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Guid):
                    byteBuffer.Write((Guid)value);
                    break;
                case Type propertyType when propertyType == typeof(PacketGuid):
                    byteBuffer.Write((PacketGuid)value);
                    break;
                default:
                    if (type.IsClass)
                        WritePacketProperties(byteBuffer, value);
                    break;
            }
        }
    }
}
