using Arci.Networking.Data;
using Arci.Networking.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arci.Networking.Serialization
{
    /// <summary>
    /// Packet object container for converting objects to/from Packet
    /// </summary>
    public static partial class PacketSerializer
    {
        /// <summary>
        /// Converts packet object (class with PacketClass attribute) to Packet
        /// </summary>
        /// <param name="packetObject">Object to be converted</param>
        /// <returns>Packet</returns>
        public static Packet ToPacket<T>(this T packetObject) where T : class, new()
        {
            if (packetObject == null)
                return null;

            var attribute = GetPacketClassAttribute(packetObject.GetType());
            if (attribute == null)
                return null;

            var packet = new Packet(attribute.PacketOpcode);
            WritePacketProperties(packet, packetObject);

            return packet;
        }

        /// <summary>
        /// Creates packet object from Packet
        /// </summary>
        /// <param name="packet">Packet to be converted</param>
        /// <returns>Packet object</returns>
        public static T FromPacket<T>(this Packet packet) where T : class, new()
        {
            var instance = new T();
            ReadPacketProperties(packet, instance);

            return instance;
        }

        private static void WritePacketProperties(ByteBuffer packet, object instance)
        {
            var byteBuffer = new ByteBuffer();
            foreach (var property in GetSortedProperties(instance.GetType()))
            {
                // Write information about nullability only for nullable types
                if (!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    var value = property.GetValue(instance);
                    if (value == null)
                    {
                        packet.WriteBit(false);
                        continue;
                    }

                    packet.WriteBit(true);
                }
               
                WritePacketProperty(byteBuffer, property.GetValue(instance), Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            packet.FlushBits();
            packet.Write(byteBuffer);
        }

        private static void ReadPacketProperties(ByteBuffer packet, object instance)
        {
            var properties = GetSortedProperties(instance.GetType());
            var packedProperties = new List<bool>();

            foreach (var property in properties)
            {
                // Read information about nullability only for nullable types
                if (!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null)
                    packedProperties.Add(packet.ReadBit());
            }

            packet.ClearUnflushedBits();
            var index = 0;

            foreach (var property in properties)
            {
                // Check if property is nullable and if so, check if property is present or should be skipped
                if ((!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null) && !packedProperties[index++])
                    continue;

                var value = ReadPacketProperty(packet, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                if (value == null)
                    continue;

                property.SetValue(instance, value);
            }
        }

        private static PacketClassAttribute GetPacketClassAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(PacketClassAttribute), true).FirstOrDefault() as PacketClassAttribute;
        }

        private static IOrderedEnumerable<PropertyInfo> GetSortedProperties(Type type)
        {
            return type.GetProperties()
              .Where(x => x.GetCustomAttributes(typeof(PacketPropertyAttribute), true).Length > 0)
              .OrderBy(x => ((PacketPropertyAttribute)x.GetCustomAttributes(typeof(PacketPropertyAttribute), false)[0]).Order);
        }

        private static object ReadPacketObject(ByteBuffer byteBuffer, Type type)
        {
            if (type == typeof(Guid))
            {
                return byteBuffer.ReadGuid();
            }
            else if (type == typeof(PacketGuid))
            {
                return byteBuffer.ReadPacketGuid();
            }
            else if (type.IsGenericType && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                var instance = Activator.CreateInstance(type);
                var count = byteBuffer.ReadUInt16();
                for (var i = 0; i < count; i++)
                {
                    var value = ReadPacketProperty(byteBuffer, type.GetGenericArguments()[0]);
                    type.GetMethod("Add").Invoke(instance, new[] { value });
                }
                return instance;
            }
            else if (type.IsArray)
            {
                var count = byteBuffer.ReadUInt16();
                var arr = Array.CreateInstance(type.GetElementType(), count);
                for (var i = 0; i < count; i++)
                    arr.SetValue(ReadPacketProperty(byteBuffer, type.GetElementType()), i);
                return arr;
            }
            else if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
            {
                var instance = Activator.CreateInstance(type);
                ReadPacketProperties(byteBuffer, instance);
                return instance;
            }
            return null;
        }

        private static void WritePacketObject(ByteBuffer byteBuffer, object value, Type type)
        {
            if (type == typeof(Guid))
            {
                byteBuffer.Write((Guid)value);
            }
            else if (type == typeof(PacketGuid))
            {
                byteBuffer.Write((PacketGuid)value);
            }
            else if (type.IsArray || (type.IsGenericType && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(ICollection<>))))
            {
                UInt16 count = 0;
                var itemsBuffer = new ByteBuffer();
                foreach (var item in (IEnumerable)value)
                {
                    WritePacketProperty(itemsBuffer, item, type.GetElementType() ?? type.GetGenericArguments()[0]);
                    count++;
                }

                byteBuffer.Write(count);
                byteBuffer.Write(itemsBuffer);
            }
            else if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
                WritePacketProperties(byteBuffer, value);
        }
    }
}
