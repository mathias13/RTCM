using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCM.MESSAGES
{
    public struct SatteliteData1001
    {
        private uint _satID;

        private bool _l1CodeInd;

        private uint _l1Pseudo;

        private int _l1PhaseMinusPseudo;

        private uint _l1LockTimeInd;

        public SatteliteData1001(byte[] data, int startBit)
        {
            _satID = BitUtil.GetUnsigned(data, startBit + 0, 6);
            _l1CodeInd = BitUtil.GetUnsigned(data, startBit + 6, 1) > 0;
            _l1Pseudo = BitUtil.GetUnsigned(data, startBit + 7, 24);
            _l1PhaseMinusPseudo = BitUtil.GetSigned(data, startBit + 31, 20);
            _l1LockTimeInd = BitUtil.GetUnsigned(data, startBit + 51, 7);
        }

        public SatteliteData1001(uint satID, bool l1CodeInd, uint l1Pseudo, int l1PhaseMinusPseudo, uint l1LockTimeInd)
        {
            _satID = satID;
            _l1CodeInd = l1CodeInd;
            _l1Pseudo = l1Pseudo;
            _l1PhaseMinusPseudo = l1PhaseMinusPseudo;
            _l1LockTimeInd = l1LockTimeInd;
        }

        public uint SatteliteID
        {
            get { return _satID; }
        }

        public bool L1CodeIndicator
        {
            get { return _l1CodeInd; }
        }

        public uint L1PseudoRange
        {
            get { return _l1Pseudo; }
        }

        public int L1PhaserangeMinusPseudorange
        {
            get { return _l1PhaseMinusPseudo; }
        }

        public uint L1LocktimeIndicator
        {
            get { return _l1LockTimeInd; }
        }
    }

    public class Message_1001: GpsRtkBase
    {
        private SatteliteData1001[] _satteliteObs;

        public Message_1001(SatteliteData1001[] satteliteObs, uint refStationId, uint tow, bool syncGnssFlag, uint numberGpsSats, bool gpsDivSmooth, uint carrSmoothInterval)
            :base (refStationId, tow, syncGnssFlag, numberGpsSats, gpsDivSmooth, carrSmoothInterval)
        {
            _satteliteObs = satteliteObs;
        }

        public Message_1001(byte[] data)
            :base(data)
        {
            List<SatteliteData1001> satteliteObs = new List<SatteliteData1001>();
            for (int i = 0; i < base._numberGpsSats; i++)
                satteliteObs.Add(new SatteliteData1001(data, 64 + (74 * i)));

            _satteliteObs = satteliteObs.ToArray();
        }

        public override void GetBytes(ref byte[] buffer)
        {
            BitUtil.WriteUnsigned(ref buffer, 1002, 0, 12);
            base.GetBytes(ref buffer);
            for (int i = 0; i < _satteliteObs.Length; i++ )
            {
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].SatteliteID, 64 + (74 * i), 6);
                BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_satteliteObs[i].L1CodeIndicator), 70, 1);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1PseudoRange, 71, 24);
                BitUtil.WriteSigned(ref buffer, _satteliteObs[i].L1PhaserangeMinusPseudorange, 95, 20);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1LocktimeIndicator, 115, 7);
            }
        }
    
        public new int ByteLength
        {
            get { return (int)Math.Ceiling(8.0 + (9.25 * (double)_satteliteObs.Length)); }
        }
    }
}
