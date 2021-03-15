using StarMicronics.StarIO;
using static POPScanner.EnumSource;

namespace POPScanner
{
    public class CommunicationResult
    {
        public Result Result { get; set; }

        public int Code { get; set; }

        public CommunicationResult()
        {
            Result = Result.ErrorUnknown;
            Code = StarResultCode.ErrorFailed;
        }
    }
}
