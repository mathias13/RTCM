using System;

namespace RTCM.MESSAGES
{
    public interface ISendMessage
    {
        void GetBytes(ref byte[] buffer);

        int ByteLength
        {
            get;
        }
    }
}
