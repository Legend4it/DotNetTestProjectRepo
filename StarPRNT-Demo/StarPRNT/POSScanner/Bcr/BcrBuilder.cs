using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POPScanner
{
    class BcrBuilder
    {
        private List<byte> commandsList;

        public BcrBuilder()
        {
            commandsList = new List<byte>();
        }

        public byte[] Commands
        {
            get
            {
                return commandsList.ToArray();
            }
        }

        public void Append(byte data)
        {
            commandsList.Add(data);
        }

        public void Append(byte[] data)
        {
            commandsList.AddRange(data);
        }

        public void AppendClearBuffer()
        {
            Append(new byte[] { 0x1b, 0x1d, (byte)'B', 0x33 });
        }
    }
}
