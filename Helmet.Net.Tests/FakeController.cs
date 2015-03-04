using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Helmet.Net.Tests
{
    [RoutePrefix("api/foo")]
    public class FakeController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "hello", "world" };
        }

        [HttpGet]
        public HttpResponseMessage GetByOp(string op)
        {
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Headers.Add("Content-Disposition", "attachment; filename=somefile.txt");
            return message;
        }
    }
}