using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.HidePoweredByHeader
{
    public class HidePoweredByHeaderMiddleware : OwinMiddleware
    {
        private readonly HidePoweredOptions _options;

        public HidePoweredByHeaderMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public HidePoweredByHeaderMiddleware(OwinMiddleware next, HidePoweredOptions options)
            : base(next)
        {
            _options = options ?? new HidePoweredOptions();
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (!string.IsNullOrEmpty(_options.SetTo))
            {
                context.Response.Headers.Add("X-Powered-By", new[] { _options.SetTo });
            }
            else
            {
                context.Response.Headers.Remove("X-Powered-By");
            }

            await Next.Invoke(context);
        }
    }
}