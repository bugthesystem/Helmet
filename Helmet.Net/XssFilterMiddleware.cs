using System;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace Helmet.Net
{
    public class XssFilterMiddleware : OwinMiddleware
    {
        public XssFilterMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            Console.WriteLine("Begin Request");
            await Next.Invoke(context);
            Console.WriteLine("End Request");
        }
    }
}