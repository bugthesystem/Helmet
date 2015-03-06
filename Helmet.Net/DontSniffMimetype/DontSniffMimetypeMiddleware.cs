using Microsoft.Owin;
using System.Threading.Tasks;

namespace Helmet.Net
{
    public class DontSniffMimetypeMiddleware : OwinMiddleware
    {
        public DontSniffMimetypeMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", new[] { "nosniff" });
            await Next.Invoke(context);
        }
    }
}