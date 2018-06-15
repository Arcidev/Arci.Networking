
using Arci.Networking.Data;
using System;

namespace Arci.Networking.Object
{
    public abstract partial class PacketObject
    {
        private static object ReadPacketProperty(Packet packet, Type type)
        {
            switch(type)
            {
                
                case Type propertyType when propertyType == typeof(Int16):
                    return packet.ReadInt16();
                
                case Type propertyType when propertyType == typeof(Int32):
                    return packet.ReadInt32();
                
                case Type propertyType when propertyType == typeof(SByte):
                    return packet.ReadSByte();
                
                case Type propertyType when propertyType == typeof(Int64):
                    return packet.ReadInt64();
                
                case Type propertyType when propertyType == typeof(UInt16):
                    return packet.ReadUInt16();
                
                case Type propertyType when propertyType == typeof(UInt32):
                    return packet.ReadUInt32();
                
                case Type propertyType when propertyType == typeof(Byte):
                    return packet.ReadByte();
                
                case Type propertyType when propertyType == typeof(UInt64):
                    return packet.ReadUInt64();
                
                case Type propertyType when propertyType == typeof(String):
                    return packet.ReadString();
                
                case Type propertyType when propertyType == typeof(PacketGuid):
                    return new PacketGuid(packet.ReadUInt64());
                default:
                    return null;
            }
        }

        private static void WritePacketProperty(Packet packet, object value, Type type)
        {
            switch(type)
            {
                
                case Type propertyType when propertyType == typeof(Int16):
                    packet.Write(Convert.ToInt16(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Int32):
                    packet.Write(Convert.ToInt32(value));
                    break;
                
                case Type propertyType when propertyType == typeof(SByte):
                    packet.Write(Convert.ToSByte(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Int64):
                    packet.Write(Convert.ToInt64(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt16):
                    packet.Write(Convert.ToUInt16(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt32):
                    packet.Write(Convert.ToUInt32(value));
                    break;
                
                case Type propertyType when propertyType == typeof(Byte):
                    packet.Write(Convert.ToByte(value));
                    break;
                
                case Type propertyType when propertyType == typeof(UInt64):
                    packet.Write(Convert.ToUInt64(value));
                    break;
                
                case Type propertyType when propertyType == typeof(String):
                    packet.Write(Convert.ToString(value));
                    break;
                
                case Type propertyType when propertyType == typeof(PacketGuid):
                    packet.Write((PacketGuid)value);
                    break;
                default:
                    return;
            }
        }
    }
}
