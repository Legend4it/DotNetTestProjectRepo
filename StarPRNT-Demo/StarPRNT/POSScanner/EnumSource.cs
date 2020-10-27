using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POPScanner
{
    public static class EnumSource
    {
        public enum Result
        {
            Success,
            ErrorUnknown,
            ErrorOpenPort,
            ErrorBeginCheckedBlock,
            ErrorEndCheckedBlock,
            ErrorWritePort,
            ErrorReadPort,
        }

        public enum PeripheralStatus
        {
            Invalid,
            Impossible,
            Connect,
            Disconnect
        }

    }
}
