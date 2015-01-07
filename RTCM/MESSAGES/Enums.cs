using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCM.MESSAGES
{
    public class Enums
    {
        public enum Messages
        {
            MSG_1001 = 1001,
            MSG_1002 = 1002,
            MSG_1003 = 1003,
            MSG_1004 = 1004,
            MSG_1005 = 1005,
            MSG_1006 = 1006,
            Unknown
        }

        public enum L2CodeIndicator
        {
            C_A_code = 0,
            P_code = 1,
            Reserved = 2
        }
    }
}
