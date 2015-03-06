using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.DontSniffMimetype
{
    public class DontSniffMimetypeMiddleware : OwinMiddleware
    {
        public DontSniffMimetypeMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options", new[] {"nosniff"});
            await Next.Invoke(context);
        }
    }
}