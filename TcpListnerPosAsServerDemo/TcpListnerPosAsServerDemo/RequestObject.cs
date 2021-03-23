using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpListnerPosAsServerDemo
{
    public class RequestObject
    {
        public string OrderNr { get; set; } = "12345";
        public string Amout { get; set; } = "1";
        public string PartnerId { get; set; } = "1";
        public string EcrId { get; set; } = "EcrA";
        public string RequestType { get; set; } = string.Empty;
        public string ResultObject { get; set; } = string.Empty;
        public string ApprovalCode { get; internal set; }
        public bool Refound { get; set; } = false;

    }
}
