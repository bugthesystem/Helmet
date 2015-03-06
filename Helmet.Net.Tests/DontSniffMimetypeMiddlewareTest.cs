using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Helmet.Net.Tests
{
    public class DontSniffMimetypeMiddlewareTest : TestBase
    {
        private const string BaseAddress = "http://localhost:9000/";
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup4DontSniffMimetype>(BaseAddress);
        }

        [Test]
        public async void Sets_header_properly_Test()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");


            var send = await client.SendAsync(request);
            IEnumerable<string> values;
            send.Headers.TryGetValues("X-Content-Type-Options", out values);
            values.Should().NotBeNullOrEmpty();
            values.First().ShouldBeEquivalentTo("nosniff");
        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }
    }
}