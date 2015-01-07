using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCM
{
    public class Constants
    {
        public static readonly byte RTCM_PREAMBLE = 0xD3;

        public static readonly double PRUNIT_GPS = 299792.458;
        
        public static readonly double CLIGHT = 299792458.0;

        public static readonly double FREQ1 = 1.57542e9;

        public static readonly double LAMBDA1 = CLIGHT / FREQ1;
    }
}
