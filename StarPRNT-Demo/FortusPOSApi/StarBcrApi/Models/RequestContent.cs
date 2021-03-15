namespace StarBcrApi.Models
{
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