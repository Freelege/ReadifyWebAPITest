using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ReadifyWebAPITest.Controllers
{
    public class TokenController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult Get()
        {
            return Ok<string>("7f2154a6-503d-4a2b-86f2-3c926086bc86");
        }
    }
}
