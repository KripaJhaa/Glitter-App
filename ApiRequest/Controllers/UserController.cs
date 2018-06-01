using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ApiRequest.Controllers
{
    public class UserController : ApiController
    {
        [AllowCrossSiteJson]
        [Route("api/User/GetAllTweets")]
        [HttpGet]
        public IHttpActionResult GetAllTweets()
        {
            var session = HttpContext.Current.Session;

            if (session["EmailId"] == null)
            {
                return NotFound();
            }

            else
            {
                Console.WriteLine("Yes Session Exist" + session["EmailId"]);
                return Ok();
            }
        }

    }
}
         