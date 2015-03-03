using System;
using System.Collections.Generic;
using System.Linq;

namespace Helmet.Net.Configuration
{
    public class HelmetConfiguration : IHelmetConfiguration
    {
        public HelmetConfiguration()
        {
            Activates = Enumerable.Empty<Type>();
        }

        public IEnumerable<Type> Activates { get; set; }
    }
}