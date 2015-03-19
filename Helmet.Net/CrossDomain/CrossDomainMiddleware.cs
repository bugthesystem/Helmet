using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.CrossDomain
{
    public class CrossDomainMiddleware : OwinMiddleware
    {
        private const string DATA = "<?xml version=1.0?>" +
     "<!DOCTYPE cross-domain-policy SYSTEM http://www.adobe.com/xml/dtds/cross-domain-policy.dtd>" +
     "<cross-domain-policy>" +
     "<site-control permitted-cross-domain-policies=none/>" +
     "</cross-domain-policy>";

        private readonly CrossDomainOptions _crossDomainOptions;


        public CrossDomainMiddleware(OwinMiddleware next, CrossDomainOptions crossDomainOptions)
            : base(next)
        {
            _crossDomainOptions = crossDomainOptions ?? new CrossDomainOptions();
        }


        public async override Task Invoke(IOwinContext context)
        {
            var pathName = context.Request.Uri.ToString();
            var uri = _crossDomainOptions.CaseSensitive ? pathName : pathName.ToLower();

            if ("/crossdomain.xml" == uri)
            {
                context.Response.Headers.Add("Content-Type", new[] { "text/x-cross-domain-policy" });
                context.Response.Write(DATA);
            }
            await Next.Invoke(context);
        }
    }
}
