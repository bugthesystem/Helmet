using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using FluentAssertions;
using Helmet.Net.FrameGuard;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;

namespace Helmet.Net.Tests
{
    internal class FrameGuardMiddlewareTest : TestBase
    {
        private HttpClient _client;
        private HttpConfiguration _config;
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _config = new HttpConfiguration();
            _config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            _config.MapHttpAttributeRoutes();
        }

        [Test]
        public async void Set_Header_To_SameOrigin_With_NoArguments()
        {
            var address = GetRandomAddress();
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("SAMEORIGIN");
                appBuilder.UseWebApi(_config);
            });


            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("SAMEORIGIN");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Deny_When_Called_WithLowerCaseDeny()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("deny");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("DENY");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Deny_When_Called_WithUpperCaseDeny()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("DENY");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("DENY");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_SAMEORIGIN_When_Called_LowerCaseSameOrigin()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("sameorigin");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("SAMEORIGIN");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_SAMEORIGIN_When_Called_UpperCaseSameOrigin()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("SAMEORIGIN");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("SAMEORIGIN");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Properly_When_Called_WithLowerCaseAllow_From()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("allow-from", "http://example.com");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("ALLOW-FROMhttp://example.com");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Properly_When_Called_WithUpperCaseALLOW_FROM()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("ALLOW-FROM", "http://example.com");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("ALLOW-FROMhttp://example.com");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Properly_When_Called_WithLowerCaseallowfrom()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("allowfrom", "http://example.com");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("ALLOW-FROMhttp://example.com");

            _server.Dispose();
        }

        [Test]
        public async void Sets_Header_To_Properly_When_Called_WithUpperCaseALLOWFROM()
        {
            var address = GetRandomAddress();

            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>("ALLOWFROM", "http://example.com");
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("ALLOW-FROMhttp://example.com");

            _server.Dispose();
        }

        [Test]
        public async void Work_With_StringObject_SetTo_SAMEORIGIN()
        {
            var address = GetRandomAddress();

            const string str = "SAMEORIGIN";
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>(str);
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("SAMEORIGIN");
            assert.First().ShouldBeEquivalentTo(str, "SAMEORIGIN");

            _server.Dispose();
        }

        [Test]
        public async void Work_With_ALLOW_FROM_With_StringObject()
        {
            var address = GetRandomAddress();

            const string directive = "ALLOW-FROM";
            const string url = "http://example.com";
            _server = WebApp.Start(address, appBuilder =>
            {
                appBuilder.Use<FrameGuardMiddleware>(directive, url);
                appBuilder.UseWebApi(_config);
            });

            _client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, address + "api/foo");

            var send = await _client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Frame-Options", out values);
            var assert = values as IList<string> ?? values.ToList();
            assert.Should().NotBeNullOrEmpty();
            assert.First().ShouldBeEquivalentTo("ALLOW-FROMhttp://example.com");


            _server.Dispose();
        }

        public string GetRandomAddress()
        {
            return "http://localhost:" + new Random().Next(9000, 9020).ToString(CultureInfo.InvariantCulture) + "/";
        }
    }
}