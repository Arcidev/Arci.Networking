using Arci.Networking.Data;
using Arci.Networking.Object.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arci.Networking.Object
{
    /// <summary>
    /// Packet object container for converting objects to/from Packet
    /// </summary>
    public abstract partial class PacketObject
    {
        private static IDictionary<UInt16, Type> types;

        private static IDictionary<UInt16, Type> Types
        {
            get
            {
                if (types == null)
                    types = LoadTypes();

                return types;
            }
        }

        /// <summary>
        /// Converts packet object (class with PacketClass attribute) to Packet
        /// </summary>
        /// <param name="packetObject">Object to be converted</param>
        /// <returns>Packet</returns>
        public static Packet ToPacket(object packetObject)
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
        public static object FromPacket(Packet packet)
        {
            if (!Types.TryGetValue(packet.OpcodeNumber, out var type))
                return null;

            return ReadPacketProperties(packet, type);
        }

        private static void WritePacketProperties(Packet packet, object instance)
        {
            foreach (var property in GetSortedProperties(instance.GetType()))
                WritePacketProperty(packet, property.Item1.GetValue(instance), property.Item2.Type ?? property.Item1.PropertyType);
        }

        private static object ReadPacketProperties(Packet packet, Type type)
        {
            var instance = Activator.CreateInstance(type);
            foreach (var property in GetSortedProperties(type))
            {
                var value = ReadPacketProperty(packet, property.Item2.Type ?? property.Item1.PropertyType);
                if (value == null)
                    continue;

                property.Item1.SetValue(instance, Convert.ChangeType(value, property.Item1.PropertyType));
            }

            return instance;
        }

        private static PacketClassAttribute GetPacketClassAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(PacketClassAttribute), true).FirstOrDefault() as PacketClassAttribute;
        }

        private static IDictionary<UInt16, Type> LoadTypes()
        {
            var dictionary = new Dictionary<UInt16, Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attribute = GetPacketClassAttribute(type);
                    if (attribute == null)
                        continue;

                    dictionary.Add(attribute.PacketOpcode, type);
                }
            }

            return dictionary;
        }

        private static IOrderedEnumerable<Tuple<PropertyInfo, PacketPropertyAttribute>> GetSortedProperties(Type type)
        {
            return type.GetProperties()
              .Where(x => x.GetCustomAttributes(typeof(PacketPropertyAttribute), true).Length > 0)
              .Select(x => Tuple.Create(x, (PacketPropertyAttribute)x.GetCustomAttributes(typeof(PacketPropertyAttribute), false)[0]))
              .OrderBy(x => x.Item2.Order);
        }
    }
}
