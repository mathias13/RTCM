using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCM
{
    public class BitUtil
    {

        public static uint GetUnsigned(byte[] buffer, int startBit, int count)
        {
            return (uint)GetBits(buffer, startBit, count);
        }

        public static int GetSigned(byte[] buffer, int startBit, int count)
        {
            if (count > 32)
                throw new ArgumentException("Can't read more than 32 bits, count");
            ulong value = GetBits(buffer, startBit, count - 1);
            ulong sign = GetBits(buffer, startBit + count - 1, 1);
            if ((sign & 0x1) > 0)
            {
                ulong fillBits = 0x0;
                for (int i = 0; i < 32 - count + 1; i++)
                    fillBits = fillBits >> 1 | 0x80000000;
                return BitConverter.ToInt32(BitConverter.GetBytes((value | fillBits)), 0);
            }
            else
                return BitConverter.ToInt32(BitConverter.GetBytes((value)), 0);
        }

        public static long GetSignedLong(byte[] buffer, int startBit, int count)
        {
            if (count > 64)
                throw new ArgumentException("Can't read more than 64 bits, count");
            ulong value = GetBits(buffer, startBit, count - 1);
            ulong sign = GetBits(buffer, startBit + count - 1, 1);
            if ((sign & 0x1) > 0)
            {
                ulong fillBits = 0x0;
                for (int i = 0; i < 64 - count + 1; i++)
                    fillBits = fillBits >> 1 | 0x8000000000000000;
                return BitConverter.ToInt64(BitConverter.GetBytes((value | fillBits)), 0);
            }
            else
                return BitConverter.ToInt64(BitConverter.GetBytes((value)), 0);
        }

        public static void WriteUnsigned(ref byte[] buffer, ulong value, int startBit, int count)
        {
            if (count > 32)
                count = 32;

            ulong maxValue = (ulong)Math.Pow(2, count) - 1;
            if (value > maxValue)
                throw new ArgumentException(String.Format("Value:{0} is bigger than:{1} the number of bits to write", value.ToString(), maxValue.ToString()), "value");

            WriteBits(ref buffer, value, startBit, count);
        }

        public static void WriteSigned(ref byte[] buffer, int value, int startBit, int count)
        {
            if (count > 32)
                count = 32;

            int maxValue = (int)Math.Pow(2, count) - 1;
            int minValue = (maxValue * -1) - 1;
            if (value > maxValue || value < minValue)
                throw new ArgumentException(String.Format("Value:{0} is smaller:{2} or bigger:{1} than the number of bits to write", value.ToString(), maxValue.ToString(), minValue.ToString()), "value");

            WriteBits(ref buffer, (ulong)value, startBit, count - 1);
            WriteBits(ref buffer, value < 0 ? (uint)0x1 : (uint)0x0, startBit + count - 1, 1);
        }

        public static void WriteSigned(ref byte[] buffer, long value, int startBit, int count)
        {
            if (count > 64)
                count = 64;

            long maxValue = (long)Math.Pow(2, count) - 1;
            long minValue = (maxValue * -1) - 1;
            if (value > maxValue || value < minValue)
                throw new ArgumentException(String.Format("Value:{0} is smaller:{2} or bigger:{1} than the number of bits to write", value.ToString(), maxValue.ToString(), minValue.ToString()), "value");

            WriteBits(ref buffer, (ulong)value, startBit, count - 1);
            WriteBits(ref buffer, value < 0 ? (uint)0x1 : (uint)0x0, startBit + count - 1, 1);
        }

        private static ulong GetBits(byte[] buffer, int startBit, int count)
        {
            ulong bits = 0;

            for (int i = startBit; i < startBit + count; i++)
            {
                bits |= (ulong)((buffer[i / 8] >> (i % 8)) & 1) << i - startBit;
            }

            return bits;
        }
    
        private static void WriteBits(ref byte[] buffer, ulong data, int startBit, int count)
        {
            ulong mask = 0x01;

            if (count <= 0 || 64 < count)
                return;

            for (int i = startBit; i < startBit + count; i++, mask <<= 1)
            {
                if ((data & mask) > 0)
                    buffer[i / 8] |= (byte)(((int)1) << (i % 8));
                else
                    buffer[i / 8] &= (byte)~(((int)1) << (i % 8));
            }
        }

    }
}
