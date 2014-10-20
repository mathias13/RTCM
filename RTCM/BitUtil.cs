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
            return BitConverter.ToUInt32(GetBits(buffer, startBit, count), 0);
        }

        public static int GetSigned(byte[] buffer, int startBit, int count)
        {
            uint value = BitConverter.ToUInt32(GetBits(buffer, startBit, count - 1), 0);
            byte[] sign = GetBits(buffer, startBit + count - 1, 1);
            if (sign[0] > 0)
            {
                return (int)(Math.Pow(2, (double)count - 1) * -1) + (int)value;
            }
            else
                return (int)value;

            return BitConverter.ToInt32(GetBits(buffer, startBit, count - 1), 0);
        }

        public static void WriteUnsigned(ref byte[] buffer, uint value, int startBit, int count)
        {
            if (count > 32)
                count = 32;

            int maxValue = (int)Math.Pow(2, count) - 1;
            if (value > maxValue)
                throw new ArgumentException("Value is bigger than the number of bits to write", "value");

            WriteBits(ref buffer, BitConverter.GetBytes(value), startBit, count);
        }

        public static void WriteSigned(ref byte[] buffer, int value, int startBit, int count)
        {
            if (count > 32)
                count = 32;

            int maxValue = (int)Math.Pow(2, count) - 1;
            int minValue = (maxValue * -1) - 1;
            if (value > maxValue || value < minValue)
                throw new ArgumentException("Value is smaller or bigger than the number of bits to write", "value");

            WriteBits(ref buffer, BitConverter.GetBytes(value), startBit, count - 1);
            WriteBits(ref buffer, value < 0 ? new byte[1] { 0x1 } : new byte[1] { 0x0 }, startBit + count - 1, 1);
        }

        private static byte[] GetBits(byte[] buffer, int startBit, int count)
        {
            int byteIndex = 0;
            while (byteIndex < (startBit / 8))
                byteIndex++;

            if (byteIndex > buffer.Length)
                throw new ArgumentException("Parameter is outside buffer range", "startBit");

            int bitShift = startBit % 8;
            int bytesNeeded = (int)Math.Ceiling((float)(bitShift + count) / 8.0F);

            if(byteIndex + bytesNeeded > buffer.Length)
                throw new ArgumentException("Parameter is outside buffer range", "count");

            byte[] byteSegment = new byte[bytesNeeded];
            for (int i = 0; i < bytesNeeded; i++)
                byteSegment[i] = buffer[byteIndex + i];

            byte[] correctByte = new byte[4];

            int bitsRead = 0;
            while(bitsRead < count)
            {
                byte maskPreByte = 0x0;
                for(int i = 0; i < bitShift; i++)
                    maskPreByte = (byte)((uint)maskPreByte | (uint)Math.Pow(2.0, (double)i));

                byte maskPostByte = 0x0;

                bool noNeedForPostByte = (float)(bitShift + count - bitsRead) / 8.0F <= 1;

                if (noNeedForPostByte)
                    for (int i = bitShift + count - bitsRead; i < 8; i++)
                        maskPreByte = (byte)((uint)maskPreByte | (uint)Math.Pow(2.0, (double)i));
                else
                    for (int i = 0; i < bitShift && i < count - (bitsRead + 8 - bitShift); i++)
                        maskPostByte = (byte)((uint)maskPostByte | (uint)Math.Pow(2.0, (double)i));
                
                maskPreByte = (byte)~(uint)maskPreByte;

                correctByte[bitsRead / 8] = (byte)((uint)byteSegment[bitsRead / 8] & (uint)maskPreByte);
                correctByte[bitsRead / 8] = (byte)((uint)correctByte[bitsRead / 8] >> bitShift);
                if (!noNeedForPostByte)
                {
                    byte dummyByte = (byte)((uint)byteSegment[(bitsRead / 8) + 1] & (uint)maskPostByte);
                    dummyByte = (byte)((uint)dummyByte << 8 - bitShift);
                    correctByte[bitsRead / 8] = (byte)((uint)correctByte[bitsRead / 8] | dummyByte);
                }

                bitsRead = bitsRead + 8;
            }

            return correctByte;
        }
    
        private static void WriteBits(ref byte[] buffer, byte[] bytes, int startBit, int count)
        {
            int byteIndex = 0;
            while (byteIndex < (startBit / 8))
                byteIndex++;

            if (byteIndex > buffer.Length)
                throw new ArgumentException("Parameter is outside buffer range", "startBit");

            int bitShift = startBit % 8;
            int bytesNeeded = (int)Math.Ceiling((float)(bitShift + count) / 8.0F);

            if (byteIndex + bytesNeeded > buffer.Length)
                throw new ArgumentException("Parameter is outside buffer range", "count");

            int bitsWritten = 0;

            while(bitsWritten < count)
            {
                byte byteToWrite = bytes[bitsWritten / 8];
                byte maskEraser = 0x0;

                for(int i = 0; i < count - bitsWritten && i < 8; i++)
                    maskEraser = (byte)((uint)maskEraser | (uint)Math.Pow(2.0, (double)i));

                byteToWrite = (byte)((uint)byteToWrite & (uint)maskEraser);

                maskEraser = 0x0;
                for (int i = 0; i < 8; i++)
                    if(i < bitShift || i - bitShift > count - bitsWritten)
                        maskEraser = (byte)((uint)maskEraser | (uint)Math.Pow(2.0, (double)i));
                
                buffer[byteIndex + (bitsWritten / 8)] = (byte)(((uint)buffer[byteIndex + bitsWritten / 8] & (uint)maskEraser) | ((uint)byteToWrite << bitShift));

                if (count - bitsWritten > 8 - bitShift && bitShift > 0)
                {
                    maskEraser = 0x0;
                    for (int i = 0; i < 8; i++)
                        if (i >= bitShift || i >= (count - bitsWritten + (8 - bitShift)))
                            maskEraser = (byte)((uint)maskEraser | (uint)Math.Pow(2.0, (double)i));
                    buffer[byteIndex + (bitsWritten / 8) + 1] = (byte)((uint)buffer[byteIndex + (bitsWritten / 8) + 1] & (uint)maskEraser);
                    buffer[byteIndex + (bitsWritten / 8) + 1] = (byte)((uint)buffer[byteIndex + (bitsWritten / 8) + 1] | (uint)byteToWrite >> (8 - bitShift));
                }

                bitsWritten = bitsWritten + 8;
            }

        }

    }
}
