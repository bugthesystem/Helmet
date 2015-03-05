using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net
{
    public class IeNoOpenMiddleware : OwinMiddleware
    {
        private const string IenoOpenHeaderValue = "noopen";

        public IeNoOpenMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.Headers.Add("X-Download-Options", new[] { IenoOpenHeaderValue });
            await Next.Invoke(context);
        }
    }
}
