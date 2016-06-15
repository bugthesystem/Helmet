using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.PermanenetRedirect
{
    public class PermanentRedirectMiddleware : OwinMiddleware
    {
        private readonly PermanentRedirectOptions _options;

        public PermanentRedirectMiddleware(OwinMiddleware next) 
            : this(next, null)
        {
        }

        public PermanentRedirectMiddleware(OwinMiddleware next, PermanentRedirectOptions options)
            : base(next)
        {
            _options = options ?? new PermanentRedirectOptions();
        }

        public override async Task Invoke(IOwinContext context)
        {
            Uri url = context.Request.Uri;

            RedirectRule rule = GetRedirectRuleForUrl(url.AbsoluteUri);

            if (rule == null)
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 301;
                context.Response.Headers.Set("Location", rule.Destination);
            }
        }

        private RedirectRule GetRedirectRuleForUrl(string absoluteUri)
        {
            RedirectRule rule = _options.RedirectRules.FirstOrDefault(r => String.Equals(r.Source, absoluteUri, StringComparison.InvariantCultureIgnoreCase));
            return rule;
        }
    }
}