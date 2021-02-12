using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketStartStopTcpListner
{
    public class RequestObject
    {
        public string OrderNr { get; set; }
        public string Amout { get; set; }
        public string PartnerId { get; set; }
        public string EcrId { get; set; }
        public string RequestType { get; set; }
        public object ResultObject { get; set; }

    }
}
