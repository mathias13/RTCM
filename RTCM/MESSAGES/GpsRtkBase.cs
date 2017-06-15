using System;

namespace RTCM.MESSAGES
{
    public abstract class GpsRtkBase : ISerialize
    {
        protected uint _refStationId;

        protected uint _tow;

        protected bool _syncGnssFlag;

        protected uint _numberGpsSats;

        protected bool _gpsDivSmooth;

        protected uint _carrSmoothInterval;

        protected GpsRtkBase(uint refStationId, uint tow, bool syncGnssFlag, uint numberGpsSats, bool gpsDivSmooth, uint carrSmoothInterval)
        {
            _refStationId = refStationId;
            _tow = tow;
            _syncGnssFlag = syncGnssFlag;
            _numberGpsSats = numberGpsSats;
            _gpsDivSmooth = gpsDivSmooth;
            _carrSmoothInterval = carrSmoothInterval;
        }

        protected GpsRtkBase(byte[] data)
        {
            _refStationId = BitUtil.GetUnsigned(data, 12, 12);
            _tow = BitUtil.GetUnsigned(data, 24, 30);
            _syncGnssFlag = BitUtil.GetUnsigned(data, 54, 1) > 0 ? true : false;
            _numberGpsSats = BitUtil.GetUnsigned(data, 55, 5);
            _gpsDivSmooth = BitUtil.GetUnsigned(data, 60, 1) > 0 ? true : false;
            _carrSmoothInterval = BitUtil.GetUnsigned(data, 61, 3);
        }

        public virtual void GetBytes(ref byte[] buffer)
        {
            BitUtil.WriteUnsigned(ref buffer, _refStationId, 12, 12);
            BitUtil.WriteUnsigned(ref buffer, _tow, 24, 30);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_syncGnssFlag), 54, 1);
            BitUtil.WriteUnsigned(ref buffer, _numberGpsSats, 55, 5);
            BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_gpsDivSmooth), 60, 1);
            BitUtil.WriteUnsigned(ref buffer, _carrSmoothInterval, 61, 3);
        }
    
        public virtual int ByteLength
        {
            get { return 8; }
        }

        public uint RefStationID
        {
            get { return _refStationId; }
        }

        public uint TimeOfWeek
        {
            get { return _tow; }
        }

        public bool SyncGNSSFlag
        {
            get { return _syncGnssFlag; }
        }

        public uint NumberOfGPSSats
        {
            get { return _numberGpsSats; }
        }

        public bool GPSDivergenceSmoothing
        {
            get { return _gpsDivSmooth; }
        }

        public uint GPSSmoothingInterval
        {
            get { return _carrSmoothInterval; }
        }
    }
}
