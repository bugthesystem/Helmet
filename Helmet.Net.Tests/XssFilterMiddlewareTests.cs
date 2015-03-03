using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using System.Collections.Generic;

namespace Helmet.Net.Tests
{
    public class XssFilterMiddlewareTests:TestBase
    {
        private IDisposable _server;
        private IDisposable _serverForConfigTest;
        private const string BaseAddress = "http://localhost:9000/";
        private const string BaseAddressForConfigTest = "http://localhost:9001/";
        private IEnumerable<string> _enabledBrowsers;
        private IEnumerable<string> _disabledBrowsers;

        protected override async void FinalizeSetUp()
        {
           
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _enabledBrowsers = GrabList("enabled_browser_list.txt");
            _disabledBrowsers = GrabList("disabled_browser_list.txt");

            _server = WebApp.Start<Startup>(url: BaseAddress);
            _serverForConfigTest = WebApp.Start<StartupWithConfig>(url: BaseAddressForConfigTest);
        }

        private IEnumerable<string> GrabList(string path)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            List<string> lines = File.ReadLines(Path.Combine(baseDirectory, path)).ToList();

            return lines;
        }

        [Test]
        public async void Enables_it_for_supported_browsers_Test()
        {
            HttpClient client = new HttpClient();
            foreach (var browser in _enabledBrowsers)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");
                request.Headers.TryAddWithoutValidation("User-Agent", browser);
                HttpResponseMessage send = await client.SendAsync(request);

                IEnumerable<string> values;
                send.Headers.TryGetValues("X-XSS-Protection", out values);

                values.Should().NotBeNullOrEmpty();
                values.First().ShouldBeEquivalentTo("1; mode=block");
            }
        }

        [Test]
        public async void Disables_it_for_unsupported_browsers_Test()
        {
            HttpClient client = new HttpClient();
            foreach (var browser in _disabledBrowsers)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");
                request.Headers.TryAddWithoutValidation("User-Agent", browser);
                HttpResponseMessage send = await client.SendAsync(request);

                IEnumerable<string> values;
                send.Headers.TryGetValues("X-XSS-Protection", out values);

                values.Should().NotBeNullOrEmpty();
                values.First().ShouldBeEquivalentTo("0");
            }
        }

        [Test]
        public async void Sets_header_if_there_is_an_empty_user_agent_Test()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");
            request.Headers.TryAddWithoutValidation("User-Agent", "");
            HttpResponseMessage send = await client.SendAsync(request);

            IEnumerable<string> values;
            send.Headers.TryGetValues("X-XSS-Protection", out values);

            values.Should().NotBeNullOrEmpty();
            values.First().ShouldBeEquivalentTo("1; mode=block");
        }

        [Test]
        public async void Sets_header_if_there_is_no_user_agent_Test()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseAddress + "api/foo");
            request.Headers.Remove("User-Agent");
            HttpResponseMessage send = await client.SendAsync(request);

            IEnumerable<string> values;
            send.Headers.TryGetValues("X-XSS-Protection", out values);

            values.Should().NotBeNullOrEmpty();
            values.First().ShouldBeEquivalentTo("1; mode=block");
        }

        [Test]
        public async void Allows_you_to_force_the_header_for_unsupported_browsers_Test()
        {
            HttpClient client = new HttpClient();
            foreach (var browser in _disabledBrowsers)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseAddressForConfigTest + "api/foo");
                request.Headers.TryAddWithoutValidation("User-Agent", browser);
                HttpResponseMessage send = await client.SendAsync(request);

                IEnumerable<string> values;
                send.Headers.TryGetValues("X-XSS-Protection", out values);

                values.Should().NotBeNullOrEmpty();
                values.First().ShouldBeEquivalentTo("1; mode=block");
            }
        }

        [TestFixtureTearDown]
        protected void TesTearDown()
        {
            if (_server != null)
                _server.Dispose(); 
            
            if (_serverForConfigTest != null)
                _serverForConfigTest.Dispose();
        }
    }
}
