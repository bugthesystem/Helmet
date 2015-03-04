using System.Web.Http;
using Helmet.Net.Configuration;
using Owin;

namespace Helmet.Net.Tests
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            config.MapHttpAttributeRoutes();

            appBuilder.Use<XssFilterMiddleware>();
            appBuilder.UseWebApi(config);
        }
    }

    public class StartupWithConfig
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
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

}