using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using john.mcgowan.cf.webcrawler.bo;

namespace john.mcgowan.cf.webcrawler.unittests
{
    [TestClass]
    public class CrawlerUnitTests
    {
        [TestMethod]
        public void CrawlListUnitTest()
        {
            bo.Web.RobotsTxt robots = new bo.Web.RobotsTxt(new Uri("http://www.tyre-shopper.co.uk"));
            robots.DisallowedList.Add("/type-type");
            bo.Crawler.CrawlList list = new bo.Crawler.CrawlList(robots);

            bo.Web.PageCrawlDetail page1 = new bo.Web.PageCrawlDetail(robots.BastUri);

            Assert.IsTrue(list.UrlToBeAdded(page1));
            list.AddUrl(page1);

            Assert.IsFalse(list.UrlToBeAdded(page1));

            bo.Web.PageCrawlDetail page2 = list.GetNext();
            list.UrlsCompleted.Add(page1);
            Assert.IsFalse(list.UrlToBeAdded(page1));


            bo.Web.PageCrawlDetail page3 = new bo.Web.PageCrawlDetail(new Uri("http://www.tyre-shopper.co.uk/type-type"));
            Assert.IsFalse(list.UrlToBeAdded(page3));

            bo.Web.PageCrawlDetail page4 = new bo.Web.PageCrawlDetail(new Uri("http://www.facebook.com"));
            Assert.IsFalse(list.UrlToBeAdded(page4));



        }
    }
}
