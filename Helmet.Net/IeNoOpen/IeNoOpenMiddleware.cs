using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.IeNoOpen
{
    public class IeNoOpenMiddleware : OwinMiddleware
    {
        public IeNoOpenMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.Headers.Add("X-Download-Options", new[] {"noopen"});
            await Next.Invoke(context);
        }
    }
}