using System;

namespace Arci.Networking.Data
{
    public class Guid
    {
        private byte[] byteVal;

        /// <summary>
        /// Creates new Guid instance
        /// </summary>
        public Guid()
        {
            byteVal = new byte[8];
        }

        /// <summary>
        /// Creates new Guid instance from UInt64 value
        /// </summary>
        /// <param name="guid">Value to be stored as Guid</param>
        public Guid(UInt64 guid)
        {
            byteVal = BitConverter.GetBytes(guid);
        }

        /// <summary>
        /// Conversion operator from UInt64 to Guid
        /// </summary>
        /// <param name="guid">Value to be converted to Guid</param>
        public static implicit operator Guid(UInt64 guid)
        {
            return new Guid(guid);
        }

        /// <summary>
        /// Allows to get/set value of any byte value of this Guid
        /// </summary>
        /// <param name="index">Byte index</param>
        /// <returns>Value at index</returns>
        public byte this[int index]
        {
            get { return byteVal[index]; }
            set { byteVal[index] = value; }
        }

        /// <summary>
        /// Conversion operator from Guid to UInt64
        /// </summary>
        /// <param name="guid">Value to be converted to UInt64</param>
        public static implicit operator UInt64(Guid guid)
        {
            return BitConverter.ToUInt64(guid.byteVal, 0);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second guid value</param>
        /// <returns>true if values represents same UIn64 value, otherwise false</returns>
        public static bool operator ==(Guid guid1, Guid guid2)
        {
            if (object.ReferenceEquals(guid1, null))
                return object.ReferenceEquals(guid2, null);

            return guid1.Equals(guid2);
        }

        /// <summary>
        /// Comparsion operator
        /// </summary>
        /// <param name="guid1">First guid value</param>
        /// <param name="guid2">Second guid value</param>
        /// <returns>false if values represents same UIn64 value, otherwise true</returns>
        public static bool operator !=(Guid guid1, Guid guid2)
        {
            if (object.ReferenceEquals(guid1, null))
                return !object.ReferenceEquals(guid2, null);

            return !guid1.Equals(guid2);
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
                return (UInt64)this == (UInt64)obj;

            Guid guid = obj as Guid;
            return guid != null && (UInt64)this == (UInt64)guid;
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
