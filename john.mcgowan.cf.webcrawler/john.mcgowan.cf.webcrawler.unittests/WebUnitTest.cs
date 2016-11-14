using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using john.mcgowan.cf.webcrawler.bo;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace john.mcgowan.cf.webcrawler.unittests
{
    [TestClass]
    public class WebUnitTest
    {
        private string GetUnitTestDataFilePath(string sampleFile)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\UnitTestData\" + sampleFile;
            if (File.Exists(path))
            {
                return path;
            }
            return string.Empty;
        }
        private string GetFileContents(string sampleFile)
        {
            string path = GetUnitTestDataFilePath(sampleFile);
            if (!string.IsNullOrEmpty(path))
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
            return string.Empty;
        }

        [TestMethod]
        public void InvalidHttpResponse()
        {
            Moq.Mock<HttpWebResponse> fakeResponse = new Moq.Mock<HttpWebResponse>();
            fakeResponse.Setup(response => response.StatusCode).Returns(HttpStatusCode.NotFound);
            Moq.Mock<HttpWebRequest> fakeRequest = new Moq.Mock<HttpWebRequest>();
            fakeRequest.Setup(request => request.GetResponse()).Returns(fakeResponse.Object);

            bo.Web.WebPageContent content = new bo.Web.WebPageContent();

            content.request = fakeRequest.Object;
            Assert.IsFalse(content.TryGetResponse());     


        }

        [TestMethod]
        public void ValidHttpResponse()
        {
            Moq.Mock<HttpWebResponse> fakeResponse = new Moq.Mock<HttpWebResponse>();
            fakeResponse.Setup(response => response.StatusCode).Returns(HttpStatusCode.OK);
            Moq.Mock<HttpWebRequest> fakeRequest = new Moq.Mock<HttpWebRequest>();
            fakeRequest.Setup(request => request.GetResponse()).Returns(fakeResponse.Object);

            bo.Web.WebPageContent content = new bo.Web.WebPageContent();

            content.request = fakeRequest.Object;
            Assert.IsTrue(content.TryGetResponse());

            content = new bo.Web.WebPageContent();
            //content.SetRequestFromURL("http://www.tyre-shopper.co.uk/robots.txt
            content.SetRequestFromURL("http://localhost:4174/");

            Assert.IsTrue(content.TryGetResponse());
        }

        [TestMethod]
        public void DisallowedNone1()
        {
            string robotTxt = string.Empty;
            bo.Web.RobotsTxt robots = bo.Helpers.RobotsHelper.ParseRobotsTxt(robotTxt);
            Assert.IsTrue(robots.DisallowedList.Count == 0);
        }


        [TestMethod]
        public void DisallowedNone2()
        {
            string robotTxt = "#\n# To ban all spiders from the entire site uncomment the next two lines:\n"
                            + "# User-Agent: *\n# Disallow: /\n  User-agent: *\n";

            bo.Web.RobotsTxt robots = bo.Helpers.RobotsHelper.ParseRobotsTxt(robotTxt);
            Assert.IsTrue(robots.DisallowedList.Count == 0);
        }
        [TestMethod]
        public void Disallowed1()
        {
            string robotTxt = GetFileContents("robots.txt");

            bo.Web.RobotsTxt robots = bo.Helpers.RobotsHelper.ParseRobotsTxt(robotTxt);
            Assert.IsTrue(robots.DisallowedList.Count == 9);
        }

        [TestMethod]
        public void GetPageCrawlerDetailTest()
        {
            string page = string.Empty;
            bo.Web.WebPageContent content = new bo.Web.WebPageContent();
            string fileName = "tyre-shopper.co.uk.home.html";
            string path = GetUnitTestDataFilePath(fileName);
            page = GetFileContents(fileName);
            Uri uri = new Uri(path);
            bo.Web.PageCrawlDetail pageCrawlDetail = new bo.Web.PageCrawlDetail(uri);
            pageCrawlDetail.PageUri = new Uri("http://www.tyre-shopper.co.uk");
            pageCrawlDetail.LoadContent(page, HttpStatusCode.OK);
            pageCrawlDetail.LoadUris();
            Assert.IsTrue(pageCrawlDetail.AllLinks.Count == 37);

            content = new bo.Web.WebPageContent();
            fileName = "tyre-shopper.co.uk.aboutUs.html";
            path = GetUnitTestDataFilePath(fileName);
            page = GetFileContents(fileName);
            uri = new Uri(path);
            pageCrawlDetail = new bo.Web.PageCrawlDetail(uri);
            pageCrawlDetail.PageUri = new Uri("http://www.tyre-shopper.co.uk/about-us/");
            pageCrawlDetail.LoadContent(page, HttpStatusCode.OK);
            pageCrawlDetail.LoadUris();
            Assert.IsTrue(pageCrawlDetail.AllLinks.Count == 22);
        }

        [TestMethod]
        public void ValidateHrefTest()
        {
            Uri uri = new Uri("http://www.tyre-shopper.co.uk/about-us/");
            string href = "/new-tyres";
            Uri finalUri = bo.Helpers.WebHelper.GetUrifromHREF(uri, href);
            Assert.IsTrue(finalUri != null && finalUri.AbsolutePath == href);

            href = "http://www.google.co.uk";
            finalUri = bo.Helpers.WebHelper.GetUrifromHREF(uri, href);
            Assert.IsFalse(finalUri != null && finalUri.AbsolutePath == href);
            Assert.IsTrue(finalUri != null && finalUri.OriginalString == href);

            href = "1234567891011";
            finalUri = bo.Helpers.WebHelper.GetUrifromHREF(uri, href);
            Assert.IsTrue(finalUri == null);

            href = "javascript:void(0);";
            finalUri = bo.Helpers.WebHelper.GetUrifromHREF(uri, href);
            Assert.IsTrue(finalUri.AbsoluteUri == href);
        }



    }
}
