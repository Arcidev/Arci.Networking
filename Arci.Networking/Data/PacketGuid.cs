﻿using System;

namespace Arci.Networking.Data
{
    /// <summary>
    /// Represents 64bit integer as 8 byte values
    /// </summary>
    public class PacketGuid : IEquatable<PacketGuid>, IEquatable<UInt64>
    {
        private readonly byte[] byteVal;

        /// <summary>
        /// Creates new Guid instance
        /// </summary>
        public PacketGuid() => byteVal = new byte[8];

        /// <summary>
        /// Creates new Guid instance from UInt64 value
        /// </summary>
        /// <param name="guid">Value to be stored as Guid</param>
        public PacketGuid(UInt64 guid)
        {
            byteVal = BitConverter.GetBytes(guid);
        }

        /// <summary>
        /// Conversion operator from UInt64 to Guid
        /// </summary>
        /// <param name="guid">Value to be converted to Guid</param>
        public static implicit operator PacketGuid(UInt64 guid)
        {
            return new PacketGuid(guid);
        }

        /// <summary>
        /// Allows to get/set value of any byte value of this Guid
        /// </summary>
        /// <param name="index">Byte index</param>
        /// <returns>Value at index</returns>
        public byte this[int index]
        {
            get => byteVal[index];
            set => byteVal[index] = value;
        }

        /// <summary>
        /// Conversion operator from Guid to UInt64
        /// </summary>
        /// <param name="guid">Value to be converted to UInt64</param>
        public static implicit operator UInt64(PacketGuid guid)
        {
            return BitConverter.ToUInt64(guid.byteVal, 0);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second guid value</param>
        /// <returns>true if values represents same UIn64 value, otherwise false</returns>
        public static bool operator ==(PacketGuid guid1, PacketGuid guid2)
        {
            return Equals(guid1, guid2);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second UInt64 value</param>
        /// <returns>true if values represents same UIn64 value, otherwise false</returns>
        public static bool operator ==(PacketGuid guid1, UInt64 guid2)
        {
            return Equals(guid1, guid2);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second guid value</param>
        /// <returns>false if values represents same UIn64 value, otherwise true</returns>
        public static bool operator !=(PacketGuid guid1, PacketGuid guid2)
        {
            return !Equals(guid1, guid2);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second UInt64 value</param>
        /// <returns>false if values represents same UIn64 value, otherwise true</returns>
        public static bool operator !=(PacketGuid guid1, UInt64 guid2)
        {
            return !Equals(guid1, guid2);
        }

        /// <summary>
        /// Compares with other Guid value
        /// </summary>
        /// <param name="other">Value to be compared with</param>
        /// <returns>true if value represents same UIn64 value as this Guid value, otherwise false</returns>
        public bool Equals(PacketGuid other)
        {
            if (other == null)
                return false;

            return Equals((UInt64)other);
        }

        /// <summary>
        /// Compares with other UInt64 value
        /// </summary>
        /// <param name="other">Value to be compared with</param>
        /// <returns>true if value represents same UIn64 value as this Guid value, otherwise false</returns>
        public bool Equals(UInt64 other)
        {
            return (UInt64)this == other;
        }

        /// <summary>
        /// Compares with other Guid or UInt64 value
        /// </summary>
        /// <param name="obj">Value to be compared with</param>
        /// <returns>true if value represents same UIn64 value as this Guid value, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is UInt64)
                return Equals((UInt64)obj);

            return Equals(obj as PacketGuid);
        }

        /// <summary>
        /// Generates hashcode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return ((UInt64)this).GetHashCode();
        }
    }
}
