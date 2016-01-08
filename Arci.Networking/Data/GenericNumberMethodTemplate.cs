
using System;

namespace Arci.Networking.Data
{
    public partial class ByteBuffer : IDisposable
    {
        
        public void Write(Int16 val)
        {
            writeData.Write(val);
        }

        public void WriteBit(Int16 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(Int32 val)
        {
            writeData.Write(val);
        }

        public void WriteBit(Int32 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(SByte val)
        {
            writeData.Write(val);
        }

        public void WriteBit(SByte bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(UInt16 val)
        {
            writeData.Write(val);
        }

        public void WriteBit(UInt16 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(UInt32 val)
        {
            writeData.Write(val);
        }

        public void WriteBit(UInt32 bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(Byte val)
        {
            writeData.Write(val);
        }

        public void WriteBit(Byte bit)
        {
            --bitPos;
            if (bit != 0)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void WriteBit(bool bit)
        {
            --bitPos;
            if (bit)
                curBitVal |= (byte)(1 << bitPos);

            if (bitPos == 0)
                WriteCurBitVal();
        }
        
        public void Write(Byte[] val)
        {
            writeData.Write((UInt16)val.Length);
            writeData.Write(val);
        }
        
        private void WriteCurBitVal()
        {
            bitPos = 8;
            Write(curBitVal);
            curBitVal = 0;
        }
    }
}
