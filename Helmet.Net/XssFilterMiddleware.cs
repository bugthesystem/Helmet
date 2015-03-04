using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helmet.Net.Configuration;
using Microsoft.Owin;

namespace Helmet.Net
{
    public class XssFilterMiddleware : OwinMiddleware
    {
        private const string XssProtectionHeaderValue = "1; mode=block";
        private readonly IXssFilterOptions _options;

        public XssFilterMiddleware(OwinMiddleware next, IXssFilterOptions options)
            : base(next)
        {
            _options = options;
        }

        public XssFilterMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (_options != null && _options.SetOnOldIE)
            {
                context.Response.Headers.Add("X-XSS-Protection", new[] {XssProtectionHeaderValue});
                await Next.Invoke(context);
            }
            else
            {
                string headerToSet;

                if (context.Request.Headers.ContainsKey("User-Agent"))
                {
                    var headerFromRequest = context.Request.Headers.Get("User-Agent");

                    var regex = new Regex(@"msie\s*(\d+)", RegexOptions.IgnoreCase);

                    var matches = regex.Matches(headerFromRequest);

                    if ((matches.Count <= 0) || CheckUseragentVersion(matches))
                    {
                        headerToSet = XssProtectionHeaderValue;
                    }
                    else
                    {
                        headerToSet = "0";
                    }
                }
                else
                {
                    headerToSet = XssProtectionHeaderValue;
                }

                context.Response.Headers.Add("X-XSS-Protection", new[] {headerToSet});

                await Next.Invoke(context);
            }
        }

        private bool CheckUseragentVersion(MatchCollection matches)
        {
            float version;

            if (float.TryParse(matches[0].Groups[1].ToString(), out version))
            {
                return (matches.Count > 0 && matches[0].Groups.Count > 1 && (version >= 9));
            }

            return false;
        }
    }
}