using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;
using FluentAssertions;
using Helmet.Net.HidePoweredByHeader;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;

namespace Helmet.Net.Tests
{
   public class HidePoweredMiddlewareTest:TestBase

    {
        private HttpClient _client;
        private HttpConfiguration _config;
        private IDisposable _server;


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _config = new HttpConfiguration();
            _config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            _config.MapHttpAttributeRoutes();
        }

        [Test]
        public async void Works_Even_If_No_Header_İs_Set()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<HidePoweredByHeaderMiddleware>(new HidePoweredOptions{});
                appBuilder.UseWebApi(_config);
            });


            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("x-powered-by", out values);
            values.Should().BeNull();
          
            _server.Dispose();

        }

        [Test]
        public async void Allows_You_Explicitly_Set_Header()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<HidePoweredByHeaderMiddleware>(new HidePoweredOptions { SetTo = "steampowered" });
                appBuilder.UseWebApi(_config);
            });

           _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("x-powered-by", out values);
            values.Should().NotBeNull("steampowered");

            _server.Dispose();

        }

   
        public string GetRandomAddress()
        {
            return "http://localhost:" + new Random().Next(9000, 9020).ToString(CultureInfo.InvariantCulture) + "/";
        }
    }


}
