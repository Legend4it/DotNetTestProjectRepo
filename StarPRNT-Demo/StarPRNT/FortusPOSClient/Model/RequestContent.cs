using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortusPOSClient.Model
{
    [Serializable]
    public class RequestContent
    {
        public int RelPartnerId { get; set; }
        public string TerminalGuid { get; set; }
        public string TerminalName { get; set; }
        public string BcrCode { get; set; }
        public RequestContent()
        {

        }
    }
}
