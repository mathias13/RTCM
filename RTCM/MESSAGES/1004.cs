using System;
using System.Collections.Generic;

namespace RTCM.MESSAGES
{
    public struct SatteliteData1004
    {
        private uint _satID;

        private bool _l1CodeInd;

        private uint _l1Pseudo;

        private int _l1PhaseMinusPseudo;

        private uint _l1LockTimeInd;

        private uint _l1PseudoModulusAmbiguity;

        private uint _l1CNR;

        private Enums.L2CodeIndicator _l2CodeInd;

        private int _l2_l1_PseudoDifference;

        private int _l2PhaseMinus_l1Pseudo;

        private uint _l2LockTimeInd;

        private uint _l2CNR;

        public SatteliteData1004(byte[] data, int startBit)
        {
            _satID = BitUtil.GetUnsigned(data, startBit + 0, 6);
            _l1CodeInd = BitUtil.GetUnsigned(data, startBit + 6, 1) > 0;
            _l1Pseudo = BitUtil.GetUnsigned(data, startBit + 7, 24);
            _l1PhaseMinusPseudo = BitUtil.GetSigned(data, startBit + 31, 20);
            _l1LockTimeInd = BitUtil.GetUnsigned(data, startBit + 51, 7);
            _l1PseudoModulusAmbiguity = BitUtil.GetUnsigned(data, startBit + 58, 8);
            _l1CNR = BitUtil.GetUnsigned(data, startBit + 66, 8);
            _l2CodeInd = (Enums.L2CodeIndicator)BitUtil.GetUnsigned(data, startBit + 74, 2);
            _l2_l1_PseudoDifference = BitUtil.GetSigned(data, startBit + 76, 14);
            _l2PhaseMinus_l1Pseudo = BitUtil.GetSigned(data, startBit + 90, 20);
            _l2LockTimeInd = BitUtil.GetUnsigned(data, startBit + 110, 7);
            _l2CNR = BitUtil.GetUnsigned(data, startBit + 117, 8);
        }

        public SatteliteData1004(uint satID, bool l1CodeInd, uint l1Pseudo, int l1PhaseMinusPseudo, uint l1LockTimeInd, uint l1PseudoModulusAmbiguity, uint l1CNR, Enums.L2CodeIndicator l2CodeInd, int l2_l1_PseudoDifference, int l2PhaseMinus_l1Pseudo, uint l2LockTimeInd, uint l2CNR)
        {
            _satID = satID;
            _l1CodeInd = l1CodeInd;
            _l1Pseudo = l1Pseudo;
            _l1PhaseMinusPseudo = l1PhaseMinusPseudo;
            _l1LockTimeInd = l1LockTimeInd;
            _l1PseudoModulusAmbiguity = l1PseudoModulusAmbiguity;
            _l1CNR = l1CNR;
            _l2CodeInd = l2CodeInd;
            _l2_l1_PseudoDifference = l2_l1_PseudoDifference;
            _l2PhaseMinus_l1Pseudo = l2PhaseMinus_l1Pseudo;
            _l2LockTimeInd = l2LockTimeInd;
            _l2CNR = l2CNR;
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

        public uint L1PseudorangeModulusAmbiguity
        {
            get { return _l1PseudoModulusAmbiguity; }
        }

        public uint L1CNR
        {
            get { return _l1CNR; }
        }

        public Enums.L2CodeIndicator L2CodeIndicator
        {
            get { return _l2CodeInd; }
        }

        public int L2L1PseudoDifference
        {
            get { return _l2_l1_PseudoDifference; }
        }

        public int L2PhaseMinusL1Pseudo
        {
            get { return _l2PhaseMinus_l1Pseudo; }
        }

        public uint L2LocktimeIndicator
        {
            get { return _l2LockTimeInd; }
        }

        public uint L2CNR
        {
            get { return _l2CNR; }
        }
    }

    public class Message_1004: GpsRtkBase
    {
        private SatteliteData1004[] _satteliteObs;

        public Message_1004(SatteliteData1004[] satteliteObs, uint refStationId, uint tow, bool syncGnssFlag, uint numberGpsSats, bool gpsDivSmooth, uint carrSmoothInterval)
            :base (refStationId, tow, syncGnssFlag, numberGpsSats, gpsDivSmooth, carrSmoothInterval)
        {
            _satteliteObs = satteliteObs;
        }

        public Message_1004(byte[] data)
            :base(data)
        {
            List<SatteliteData1004> satteliteObs = new List<SatteliteData1004>();
            for (int i = 0; i < base._numberGpsSats; i++)
                satteliteObs.Add(new SatteliteData1004(data, 64 + (125 * i)));

            _satteliteObs = satteliteObs.ToArray();
        }

        public override void GetBytes(ref byte[] buffer)
        {
            BitUtil.WriteUnsigned(ref buffer, 1004, 0, 12);
            base.GetBytes(ref buffer);
            for (int i = 0; i < _satteliteObs.Length; i++ )
            {
                int startBit = 64 + (125 * i);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].SatteliteID, startBit + 0, 6);
                BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_satteliteObs[i].L1CodeIndicator), startBit + 6, 1);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1PseudoRange, startBit + 7, 24);
                BitUtil.WriteSigned(ref buffer, _satteliteObs[i].L1PhaserangeMinusPseudorange, startBit + 31, 20);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1LocktimeIndicator, startBit + 51, 7);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1PseudorangeModulusAmbiguity, startBit + 58, 8);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1CNR, startBit + 66, 8);
                BitUtil.WriteUnsigned(ref buffer, Convert.ToUInt32(_satteliteObs[i].L2CodeIndicator), startBit + 74, 2);
                BitUtil.WriteSigned(ref buffer, _satteliteObs[i].L1PseudoRange, startBit + 76, 14);
                BitUtil.WriteSigned(ref buffer, _satteliteObs[i].L1PhaserangeMinusPseudorange, startBit + 90, 20);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1LocktimeIndicator, startBit + 110, 7);
                BitUtil.WriteUnsigned(ref buffer, _satteliteObs[i].L1CNR, startBit + 117, 8);
            }
        }
    
        public new int ByteLength
        {
            get { return (int)Math.Ceiling((double)base.ByteLength + (15.625 * (double)_satteliteObs.Length)); }
        }
    }
}
