using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Helmet.Net.Tests
{
    public class NoCacheMiddlewareTest : TestBase
    {
        private const string BaseAddress = "http://localhost:9000/";
        private const string BaseAddressForConfigTest = "http://localhost:9001/";
        private IDisposable _server;
        private IDisposable _serverForConfigTest;
        private HttpClient _client;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = WebApp.Start<Startup4NoCache>(BaseAddress);
            _serverForConfigTest = WebApp.Start<Startup4NoCacheWithConfig>(BaseAddressForConfigTest);
             _client = new HttpClient();
        }

        [Test]
        public async void Sets_header_properly_Test()
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");

            var response = await _client.SendAsync(request);

            IEnumerable<string> cacheControlHaderValues;
            IEnumerable<string> pragmaHaderValues;
            IEnumerable<string> expiresHaderValues;

            response.Headers.TryGetValues("Cache-Control", out cacheControlHaderValues);
            response.Headers.TryGetValues("Pragma", out pragmaHaderValues);
            response.Headers.TryGetValues("Expires", out expiresHaderValues);


            cacheControlHaderValues.Should().NotBeNullOrEmpty();
            string cacheControlHeaderValue = cacheControlHaderValues.First();

            cacheControlHeaderValue.Should().Contain("no-store");
            cacheControlHeaderValue.Should().Contain("no-cache");
            cacheControlHeaderValue.Should().Contain("must-revalidate");
            cacheControlHeaderValue.Should().Contain("proxy-revalidate");

            pragmaHaderValues.Should().NotBeNullOrEmpty();
            pragmaHaderValues.First().ShouldBeEquivalentTo("no-cache");

            expiresHaderValues.Should().BeNull();
            expiresHaderValues.First().ShouldBeEquivalentTo("0");
        }

        [Test]
        public async void Can_be_told_to_squash_etags_Test()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, BaseAddressForConfigTest + "api/foo?op=etag");

            var response = await client.SendAsync(request);

            IEnumerable<string> cacheControlHaderValues;
            IEnumerable<string> pragmaHaderValues;
            IEnumerable<string> expiresHaderValues;

            response.Headers.TryGetValues("Cache-Control", out cacheControlHaderValues);
            response.Headers.TryGetValues("Pragma", out pragmaHaderValues);
            response.Headers.TryGetValues("Expires", out expiresHaderValues);


            cacheControlHaderValues.Should().NotBeNullOrEmpty();

            string cacheControlHeaderValue = cacheControlHaderValues.First();

            cacheControlHeaderValue.Should().Contain("no-store");
            cacheControlHeaderValue.Should().Contain("no-cache");
            cacheControlHeaderValue.Should().Contain("must-revalidate");
            cacheControlHeaderValue.Should().Contain("proxy-revalidate");

            pragmaHaderValues.Should().NotBeNullOrEmpty();
            pragmaHaderValues.First().ShouldBeEquivalentTo("no-cache");

            expiresHaderValues.Should().BeNull();

            response.Headers.ETag.Should().BeNull();
        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose();

            if (_serverForConfigTest != null)
                _serverForConfigTest.Dispose();

            using (_client){}
        }
    }
}
