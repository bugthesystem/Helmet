using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Helmet.Net.Tests
{
    public class FooController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "hello", "world" };
        }

        [HttpGet]
        public HttpResponseMessage GetByOp(string op)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);

            switch (op)
            {
                case "etag":
                    {
                        message.Headers.TryAddWithoutValidation("ETag", "abc123");
                        break;
                    }
            }

            message.Content = new StringContent("Hello world!");

            return message;
        }
    }
}