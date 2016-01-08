using System;

namespace Arci.Networking.Data
{
    public class Guid
    {
        private byte[] byteVal;

        public Guid()
        {
            byteVal = new byte[8];
        }

        public Guid(UInt64 guid)
        {
            byteVal = BitConverter.GetBytes(guid);
        }

        public static implicit operator Guid(UInt64 guid)
        {
            return new Guid(guid);
        }

        public byte this[int index]
        {
            get { return byteVal[index]; }
            set { byteVal[index] = value; }
        }

        public static implicit operator UInt64(Guid guid)
        {
            return BitConverter.ToUInt64(guid.byteVal, 0);
        }

        public static bool operator ==(Guid guid1, Guid guid2)
        {
            if (object.ReferenceEquals(guid1, null))
                return object.ReferenceEquals(guid2, null);

            return guid1.Equals(guid2);
        }

        public static bool operator !=(Guid guid1, Guid guid2)
        {
            if (object.ReferenceEquals(guid1, null))
                return !object.ReferenceEquals(guid2, null);

            return !guid1.Equals(guid2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is UInt64)
                return (UInt64)this == (UInt64)obj;

            Guid guid = obj as Guid;
            return guid != null && (UInt64)this == (UInt64)guid;
        }

        public override int GetHashCode()
        {
            return ((UInt64)this).GetHashCode();
        }
    }
}
