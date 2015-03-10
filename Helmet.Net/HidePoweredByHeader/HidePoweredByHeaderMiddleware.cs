using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helmet.Net.HidePoweredByHeader
{
   public class HidePoweredByHeaderMiddleware:OwinMiddleware
    {
       private IHidePoweredOptions _options;


       public HidePoweredByHeaderMiddleware(OwinMiddleware next)
           : base(next)
       {
       }
       public HidePoweredByHeaderMiddleware(OwinMiddleware next,IHidePoweredOptions options) : base(next)
       {
           _options = options;
       }

       public override async Task Invoke(IOwinContext context)
       {
           _options = _options ?? new HidePoweredOptions {SetTo =null};

           if (!string.IsNullOrEmpty(_options.SetTo))
           {
               context.Response.Headers.Add("X-Powered-By", new[] {_options.SetTo});
           }
           else
           {
               context.Response.Headers.Remove("X-Powered-By");
           }
           await Next.Invoke(context);

       }
    }
}
