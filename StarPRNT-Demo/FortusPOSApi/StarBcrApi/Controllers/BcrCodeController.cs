using Newtonsoft.Json;
using StarBcrApi.Models;
using System.Web.Http;

namespace StarBcrApi.Controllers
{
    public class BcrCodeController : ApiController
    {
        // GET api/bcrcode/
        [HttpGet]
        public string Get(string bcrcode)
        {
            return bcrcode;
        }

        // POST api/bcrcode
        [HttpPost]
        public void Post(object value)
        {
            //TODO: implemet some logic using value
            var data = JsonConvert.DeserializeObject<RequestContent>(value.ToString());
        }

    }
}
