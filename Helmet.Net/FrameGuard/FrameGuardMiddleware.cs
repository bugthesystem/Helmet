using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.FrameGuard
{
    public class FrameGuardMiddleware : OwinMiddleware
    {
        private readonly string _action;
        private string _header;
        private readonly string _options;
        readonly string[] _headers = { "DENY", "ALLOW-FROM", "SAMEORIGIN" };

        public FrameGuardMiddleware(OwinMiddleware next)
            : base(next)
        {
            _action = "SAMEORIGIN";
        }
        public FrameGuardMiddleware(OwinMiddleware next, string action)
            : base(next)
        {
            _action = action;
        }

        public FrameGuardMiddleware(OwinMiddleware next, string action, string options)
            : base(next)
        {
            _options = options;
            _action = action;
        }

        public async override Task Invoke(IOwinContext context)
        {

            _header = string.IsNullOrEmpty(_action) ? "SAMEORIGIN" : _action.ToUpper();

            switch (_header)
            {
                case "ALLOWFROM":
                    _header = "ALLOW-FROM";
                    break;
                case "SAME-ORIGIN":
                    _header = "SAMEORIGIN";
                    break;
            }

            if (!_headers.Contains(_header))
            {
                throw new Exception("X-Frame must be undefined, DENY, ALLOW-FROM, or SAMEORIGIN");
            }

            if (_header == "ALLOW-FROM")
            {
                if (string.IsNullOrEmpty(_options))
                {
                    throw new Exception("X-Frame: ALLOW-FROM requires a second parameter");
                }

                _header = "ALLOW-FROM" + _options;
            }
            string[] headers = { _header };
            context.Response.Headers.Add("X-Frame-Options", headers);

            await Next.Invoke(context);

        }
    }
}
