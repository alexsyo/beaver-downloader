using NUnit.Framework;
using Rhino.Mocks;
using Beaver_Downloader;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Beaver_Downloader.Tests
{
    [TestFixture]
    public class DownloaderTests
    {
        private Downloader downloader;
        private HttpClient httpClientMock;

        [OneTimeSetUp]
        public void init()
        {
            httpClientMock = MockRepository.GenerateMock<HttpClient>();

            downloader = new Downloader(httpClientMock);
        }

        [Test]
        public void GetWebResponse()
        {
            // Arrange
            HttpClient httpClientMock = MockRepository.GenerateStub<HttpClient>();
            httpClientMock.Stub(s => s.GetAsync("http://google.com")).Return(Task.FromResult(new HttpResponseMessage()));
            Downloader downloader = new Downloader(httpClientMock);

            // Act
            downloader.GetWebResponse("http://google.com");

            // Assert
            httpClientMock.VerifyAllExpectations();
        }

        [Test]
        public void GetFileNameTest()
        {
            // Arrange
            HttpResponseMessage webResponse = new HttpResponseMessage();
            webResponse.Content = new StringContent("");
            webResponse.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "test.mp3"
            };

            // Act
            string fileName = downloader.GetFileName(webResponse);

            // Assert
            Assert.That(fileName, Is.EqualTo("test.mp3"));
        }

        [Test]
        public void GetFileTest()
        {
            Assert.Fail();
        }
    }
}