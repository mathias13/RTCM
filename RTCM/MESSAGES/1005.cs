using System;

namespace RTCM.MESSAGES
{
    public class Message_1005 : ISerialize
    {
        private uint _refId;

        private uint _irtf;

        private bool _gps;

        private bool _glonass;

        private bool _galileo;

        private bool _refStationIndicator;

        private long _refX;

        private bool _singleReceiverIndicator;

        private bool _reserved = false;

        private long _refY;

        private bool _reserved1 = false;

        private bool _reserved2 = false;

        private long _refZ;

        public Message_1005(uint refId, uint irtf, bool gps, bool glonass, bool galileo, bool refStationIndicator, long refX, bool singleReceiverIndicator, long refY, long refZ)
        {
            _refId = refId;
            _irtf = irtf;
            _gps = gps;
            _glonass = glonass;
            _galileo = galileo;
            _refStationIndicator = refStationIndicator;
            _refX = refX;
            _singleReceiverIndicator = singleReceiverIndicator;
            _refY = refY;
            _refZ = refZ;
        }

        public Message_1005(uint refId, uint irtf, bool gps, bool glonass, bool galileo, bool refStationIndicator, double refX, bool singleReceiverIndicator, double refY, double refZ)
        {
            _refId = refId;
            _irtf = irtf;
            _gps = gps;
            _glonass = glonass;
            _galileo = galileo;
            _refStationIndicator = refStationIndicator;
            _refX = (long)(refX * 10000);
            _singleReceiverIndicator = singleReceiverIndicator;
            _refY = (long)(refY * 10000);
            _refZ = (long)(refZ * 10000);
        }

        public Message_1005(byte[] data)
        {
            _refId = BitUtil.GetUnsigned(data, 12, 12);
            _irtf = BitUtil.GetUnsigned(data, 24, 6);
            _gps = BitUtil.GetUnsigned(data, 30, 1) > 0 ? true : false;
            _glonass = BitUtil.GetUnsigned(data, 31, 1) > 0 ? true : false;
            _galileo = BitUtil.GetUnsigned(data, 32, 1) > 0 ? true : false;
            _refStationIndicator = BitUtil.GetUnsigned(data, 33, 1) > 0 ? true : false;
            _refX = BitUtil.GetSignedLong(data, 34, 38);
            _singleReceiverIndicator = BitUtil.GetUnsigned(data, 72, 1) > 0 ? true : false; ;
            _refY = BitUtil.GetSignedLong(data, 74, 38);
            _refZ = BitUtil.GetSignedLong(data, 114, 38);
        }

        public void GetBytes(ref byte[] buffer)
        {
            BitUtil.WriteUnsigned(ref buffer, 1005, 0, 12);
            BitUtil.WriteUnsigned(ref buffer, _refId, 12, 12);
            BitUtil.WriteUnsigned(ref buffer, _irtf, 24, 6);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_gps), 30, 1);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_glonass), 31, 1);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_galileo), 32, 1);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_refStationIndicator), 33, 1);
            BitUtil.WriteSigned(ref buffer, _refX, 34, 38);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_singleReceiverIndicator), 72, 1);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_reserved), 73, 1);
            BitUtil.WriteSigned(ref buffer, _refY, 74, 38);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_reserved1), 112, 1);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_reserved2), 113, 1);
            BitUtil.WriteSigned(ref buffer, _refZ, 114, 38);
        }
    
        public int ByteLength
        {
            get { return 19; }
        }
    
        public uint ReferenceID
        {
            get { return _refId; }
        }

        public uint IRTFRealizationYear
        {
            get { return _irtf; }
        }

        public bool GPSIndicator
        {
            get { return _gps; }
        }

        public bool GLONASSIndicator
        {
            get { return _glonass; }
        }

        public bool GALILEOIndicator
        {
            get { return _galileo; }
        }

        public bool ReferenceStationIndicator
        {
            get { return _refStationIndicator; }
        }
        
        public bool SingleReceiverOscillatorIndicator
        {
            get { return _singleReceiverIndicator; }
        }

        public long AntennaReferenceECEF_X
        {
            get { return _refX; }
        }

        public double AntennaReferenceECEF_X_Meters
        {
            get { return (double)_refX / 10000; }
        }

        public long AntennaReferenceECEF_Y
        {
            get { return _refY; }
        }

        public double AntennaReferenceECEF_Y_Meters
        {
            get { return (double)_refY / 10000; }
        }

        public long AntennaReferenceECEF_Z
        {
            get { return _refZ; }
        }

        public double AntennaReferenceECEF_Z_Meters
        {
            get { return (double)_refZ / 10000; }
        }
    }
}
