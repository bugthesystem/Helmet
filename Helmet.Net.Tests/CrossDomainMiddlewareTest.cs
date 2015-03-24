using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                    CaseSensitive = true
                });

                appBuilder.UseWebApi(_config);

            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/crossdomain.xml");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");
               
            _server.Dispose();
        }

        [Test]
        public async void ExpectPolicy2()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {

                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });

                appBuilder.UseWebApi(_config);

            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/crossdomain.XML");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }

        [Test]
        public async void ExpectPolicy3()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {

                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });

                appBuilder.UseWebApi(_config);

            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/CrossDomain.xml");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }

        [Test]
        public async void ExpectPolicy4()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {

                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/CROSSDOMAIN.xml");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }

        [Test]
        public async void ExpectPolicy5()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/CROSSDOMAIN.XML");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }

        [Test]
        public async void ExpectPolicy6()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/crossdomain.xml?");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }



        [Test]
        public async void ExpectPolicy7()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<CrossDomainMiddleware>(new CrossDomainOptions()
                {
                    CaseSensitive = false
                });
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "/crossdomain.xml?foo=123&bar=456");

            var httpResponseMessage = await _client.SendAsync(request);
            HttpContent httpContent = httpResponseMessage.Content;
            IEnumerable<string> values;
            httpContent.Headers.TryGetValues("Content-Type", out values);
            var assert = values as string[] ?? values.ToArray();
            assert.Should().NotBeNull(Policy);
            assert.Should().NotBeNull("text/x-cross-domain-policy");

            _server.Dispose();
        }

        public string GetRandomAddress()
        {
            return "http://localhost:" + new Random().Next(9000, 9020).ToString(CultureInfo.InvariantCulture) + "/";
        }
    }
}
