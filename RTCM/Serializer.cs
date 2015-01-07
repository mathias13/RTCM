using System;
using System.Collections.Generic;
using RTCM.MESSAGES;

namespace RTCM
{
    public class Serializer
    {
        public static byte[] Serialize(ISerialize message)
        {
            byte[] frame = new byte[3];
            BitUtil.WriteUnsigned(ref frame, Constants.RTCM_PREAMBLE, 0, 8);
            BitUtil.WriteUnsigned(ref frame, (uint)message.ByteLength, 14, 10);

            List<byte> completeMessage = new List<byte>(frame);

            byte[] messageBytes = new byte[message.ByteLength];
            message.GetBytes(ref messageBytes);
            completeMessage.AddRange(messageBytes);

            byte[] crcBytes = BitConverter.GetBytes(CRC24Q.crc24q(completeMessage.ToArray(), completeMessage.Count, 0));
            completeMessage.Add(crcBytes[0]);
            completeMessage.Add(crcBytes[1]);
            completeMessage.Add(crcBytes[2]);

            return completeMessage.ToArray();
        }

    }
}
