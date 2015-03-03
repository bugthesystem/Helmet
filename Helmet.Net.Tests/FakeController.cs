using System.Collections.Generic;
using System.Web.Http;

namespace Helmet.Net.Tests
{
    [RoutePrefix("api/foo")]
    public class FakeController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] {"hello", "world"};
        }
    }
}