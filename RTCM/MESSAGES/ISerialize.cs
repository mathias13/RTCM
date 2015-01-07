using System;

namespace RTCM.MESSAGES
{
    public interface ISerialize
    {
        void GetBytes(ref byte[] buffer);

        int ByteLength
        {
            get;
        }
    }
}
