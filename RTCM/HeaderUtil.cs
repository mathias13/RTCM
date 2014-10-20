using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCM.MESSAGES;

namespace RTCM
{
    public class HeaderUtil
    {
        public static readonly byte RTCM_PREAMBLE = 0xD3;

        public static byte[] AddRTCMHeader(byte[] message)
        {
            byte[] frame = new byte[3];
            BitUtil.WriteUnsigned(ref frame, RTCM_PREAMBLE, 0, 8);
            BitUtil.WriteUnsigned(ref frame, (uint)message.Length, 14, 10);
            List<byte> completeMessage = new List<byte>(frame);
            completeMessage.AddRange(message);
            byte[] crcBytes = BitConverter.GetBytes(CRC24Q.crc24q(completeMessage.ToArray(), completeMessage.Count, 0));
            completeMessage.Add(crcBytes[0]);
            completeMessage.Add(crcBytes[1]);
            completeMessage.Add(crcBytes[2]);
            return completeMessage.ToArray();
        }

        public static bool CheckAndRemoveRTCMHeader(ref List<byte> buffer)
        {
            byte[] crcBytes = new byte[3];
            for (int i = 2; i > -1; i--)
            {
                crcBytes[i] = buffer[buffer.Count - 1];
                buffer.RemoveAt(buffer.Count - 1);
            }
            if (BitConverter.ToUInt32(crcBytes, 0) == CRC24Q.crc24q(buffer.ToArray(), buffer.Count, 0))
            {
                buffer.RemoveAt(0);
                buffer.RemoveAt(0);
                buffer.RemoveAt(0);
                return true;
            }
            else
                return false;

        }
    }
}
