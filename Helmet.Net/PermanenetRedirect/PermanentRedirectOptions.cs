using System.Collections.Generic;

namespace Helmet.Net.PermanenetRedirect
{
    public class PermanentRedirectOptions
    {
        public PermanentRedirectOptions()
        {
            RedirectRules = new List<RedirectRule>();
        }

        public List<RedirectRule> RedirectRules { get; set; }
    }
}