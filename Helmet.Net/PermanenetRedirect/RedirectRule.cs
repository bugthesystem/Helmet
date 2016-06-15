using System;

namespace Helmet.Net.PermanenetRedirect
{
    public class RedirectRule
    {
        public RedirectRule(string source, string destination)
        {
            if (!source.StartsWith("http://") && !source.StartsWith("https://"))
            {
                throw new ArgumentException("Please specify a url scheme.", "source");
            }
            else
            {
                Source = source;
            }

            if (!destination.StartsWith("http://") && !destination.StartsWith("https://"))
            {
                throw new ArgumentException("Please specify a url scheme.", "destination");
            }
            else
            {
                Destination = destination;
            }
        }

        public string Source { get; }
        
        public string Destination { get; } 
    }
}