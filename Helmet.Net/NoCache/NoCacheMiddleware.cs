using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.NoCache
{
    public class NoCacheMiddleware : OwinMiddleware
    {
        private INoCacheOptions _options;

        public NoCacheMiddleware(OwinMiddleware next, INoCacheOptions options)
            : base(next)
        {
            _options = options;
        }

        public NoCacheMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            _options = _options ?? new NoCacheOptions { NoEtag = false };

            context.Response.Headers.Add("Cache-Control",
                new[] { "no-store, no-cache, must-revalidate, proxy-revalidate" });
            context.Response.Headers.Add("Pragma", new[] { "no-cache" });
            context.Response.Headers.Add("Expires", new[] { "0" });

            await Next.Invoke(context);

            if (_options.NoEtag)
            {
                if (context.Response.Headers.ContainsKey("ETag"))
                    context.Response.Headers.Remove("ETag");
            }
        }
    }
}