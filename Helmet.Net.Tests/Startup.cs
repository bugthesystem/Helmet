using System.Web.Http;
using Helmet.Net.Configuration;
using Owin;

namespace Helmet.Net.Tests
{
    public class Startup4XssFilter
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<XssFilterMiddleware>();
            appBuilder.UseWebApi(config);
        }
    }

    public class Startup4XssFilterWithConfig
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<XssFilterMiddleware>(new XssFilterOptions
            {
                SetOnOldIE = true
            });
            appBuilder.UseWebApi(config);
        }
    }

    public class Startup4IeNoOpen
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<IeNoOpenMiddleware>();
            appBuilder.UseWebApi(config);
        }
    }

    public class Startup4DontSniffMimetype
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<DontSniffMimetypeMiddleware>();
            appBuilder.UseWebApi(config);
        }
    }

    public class Startup4NoCache
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<NoCacheMiddleware>();
            appBuilder.UseWebApi(config);
        }
    }

    public class Startup4NoCacheWithConfig
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();

            appBuilder.Use<NoCacheMiddleware>(new NoCacheOptions { NoEtag = true });
            appBuilder.UseWebApi(config);
        }
    }
}