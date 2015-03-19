using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;
using FluentAssertions;
using Helmet.Net.CrossDomain;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;

namespace Helmet.Net.Tests
{
    public class CrossDomainMiddlewareTest : TestBase
    {
        private HttpClient _client;
        private HttpConfiguration _config;
        private IDisposable _server;
        private string Policy =
            "<?xml version=1.0?>" +
            "<!DOCTYPE cross-domain-policy SYSTEM http://www.adobe.com/xml/dtds/cross-domain-policy.dtd>" +
            "<cross-domain-policy>" +
            "<site-control permitted-cross-domain-policies=none/>" +
            "</cross-domain-policy>";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _config = new HttpConfiguration();
            _config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            _config.MapHttpAttributeRoutes();

        }


        [Test]
        public async void ExpectPolicy()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                
                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });

                appBuilder.Use( (context, next) =>
                {
                   context.Response.Write("hello world");
                    return next.Invoke();
                });

                appBuilder.UseWebApi(_config);

            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("Content-Type", out values);
            values.Should().NotBeNull("text/x-cross-domain-policy");
               
            _server.Dispose();
        }

        public string GetRandomAddress()
        {
            return "http://localhost:" + new Random().Next(9000, 9020).ToString(CultureInfo.InvariantCulture) + "/";
        }
    }
}
