using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Helmet.Net.Tests
{
    public class PermanentRedirectMiddlewareTests : TestBase
    {
        private const string BaseAddress = "http://localhost:9000/";
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup4PermanenetRedirect>(BaseAddress);
        }

        [Test]
        public async void Should_Redirect()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "test");
            var response = await client.SendAsync(request);

            response.RequestMessage.RequestUri.AbsoluteUri.ShouldBeEquivalentTo("http://localhost:9000/test/r");
        }

        [TestFixtureTearDown]
        protected void TestTearDown()
        {
            if (_server != null)
                _server.Dispose();
        }
    }
}