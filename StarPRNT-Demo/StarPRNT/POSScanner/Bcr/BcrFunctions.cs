using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POPScanner.Bcr
{
    public class BcrFunctions
    {
        public static byte[] CreateClearBuffer()
        {
            // Barcode reader command builder sample is in "BcrBuilder" class.
            BcrBuilder builder = new BcrBuilder();

            builder.AppendClearBuffer();

            return builder.Commands;
        }
    }
}
