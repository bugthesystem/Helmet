using System.Web.Http;
using System.Collections.Generic;

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
    }
}
