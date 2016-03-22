using NUnit.Framework;
using Beaver_Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_Downloader.Tests
{
    [TestFixture]
    public class FileObjectTests
    {
        [Test]
        public void FileObjectTest()
        {
            Assert.Fail();
        }

        [Test]
        public void GetFileNameTest()
        {
            string actual = FileObject.GetFileName("url");

            Assert.That(actual, Is.EqualTo("fileName"));
        }

        [Test]
        public void DownloadTest()
        {
            Assert.Fail();
        }

        [Test]
        public void PauseTest()
        {
            Assert.Fail();
        }
    }
}