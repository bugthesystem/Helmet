using System.Collections.Generic;
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
    }
}