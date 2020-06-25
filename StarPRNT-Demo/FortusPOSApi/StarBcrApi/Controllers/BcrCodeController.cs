using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public void Post([FromBody] string value)
        {
            //TODO: implemet some logic using value
        }

    }
}
