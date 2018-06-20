
using Arci.Networking.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arci.Networking.Object
{
    public abstract partial class PacketObject
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
                
                default:
                    if (type == typeof(Guid))
                    {
                        return byteBuffer.ReadGuid();
                    } else if (type == typeof(PacketGuid))
                    {
                        return byteBuffer.ReadPacketGuid();
                    } else if (type.IsGenericType && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    {
                        var instance = Activator.CreateInstance(type);
                        var count = byteBuffer.ReadUInt16();
                        for (var i = 0; i < count; i++)
                        {
                            var value = ReadPacketProperty(byteBuffer, type.GetGenericArguments()[0]);
                            type.GetMethod("Add").Invoke(instance, new[] { value });
                        }
                        return instance;
                    } else if (type.IsArray)
                    {
                        var count = byteBuffer.ReadUInt16();
                        var arr = Array.CreateInstance(type.GetElementType(), count);
                        for (var i = 0; i < count; i++)
                            arr.SetValue(ReadPacketProperty(byteBuffer, type.GetElementType()), i);
                        return arr;
                    } else if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
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
            switch(Type.GetTypeCode(type))
            {
                
                case TypeCode.Int16:
                    byteBuffer.Write(Convert.ToInt16(value));
                    break;
                
                case TypeCode.Int32:
                    byteBuffer.Write(Convert.ToInt32(value));
                    break;
                
                case TypeCode.SByte:
                    byteBuffer.Write(Convert.ToSByte(value));
                    break;
                
                case TypeCode.Int64:
                    byteBuffer.Write(Convert.ToInt64(value));
                    break;
                
                case TypeCode.UInt16:
                    byteBuffer.Write(Convert.ToUInt16(value));
                    break;
                
                case TypeCode.UInt32:
                    byteBuffer.Write(Convert.ToUInt32(value));
                    break;
                
                case TypeCode.Byte:
                    byteBuffer.Write(Convert.ToByte(value));
                    break;
                
                case TypeCode.UInt64:
                    byteBuffer.Write(Convert.ToUInt64(value));
                    break;
                
                case TypeCode.String:
                    byteBuffer.Write(Convert.ToString(value));
                    break;
                
                case TypeCode.Single:
                    byteBuffer.Write(Convert.ToSingle(value));
                    break;
                
                case TypeCode.Double:
                    byteBuffer.Write(Convert.ToDouble(value));
                    break;
                
                case TypeCode.Decimal:
                    byteBuffer.Write(Convert.ToDecimal(value));
                    break;
                
                default:
                    if (type == typeof(Guid))
                    {
                        byteBuffer.Write((Guid)value);
                    } else if (type == typeof(PacketGuid))
                    {
                        byteBuffer.Write((PacketGuid)value);
                    } else if (type.IsArray || (type.IsGenericType && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>))))
                    {
                        if (!(value is IEnumerable collection))
                        {
                            byteBuffer.Write((UInt16)0);
                            return;
                        }

                        UInt16 count = 0;
                        var itemsBuffer = new ByteBuffer();
                        foreach (var item in collection)
                        {
                            WritePacketProperty(itemsBuffer, item, type.GetElementType() ?? type.GetGenericArguments()[0]);
                            count++;
                        }

                        byteBuffer.Write(count);
                        byteBuffer.Write(itemsBuffer);
                    } else if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
                        WritePacketProperties(byteBuffer, value);
                    break;
            }
        }
    }
}
