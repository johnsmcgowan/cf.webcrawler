using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace john.mcgowan.cf.webcrawler.unittests
{
    [TestClass]
    public class FileUnitTest
    {
        [TestMethod]
        public void TestToCSV()
        {
            string filePath = "testToCSV.csv";
            List<bo.Web.PageCrawlDetail> list = new List<bo.Web.PageCrawlDetail>();
            bo.Web.PageCrawlDetail n1 = new bo.Web.PageCrawlDetail(new Uri("http://www.google.co.uk"));
            n1.PageTitle = "Google Search Engine";
            n1.StatusCode = System.Net.HttpStatusCode.NotFound;
            list.Add(n1);

            bo.Web.PageCrawlDetail n2 = new bo.Web.PageCrawlDetail(new Uri("http://www.yahoo.co.uk"));
            n2.PageTitle = "Yahoo Search Engine";
            n2.StatusCode = System.Net.HttpStatusCode.OK;
            list.Add(n2);

            bo.Helpers.FileHelper.PageDetailToCSV(list, filePath);

            File.Exists(filePath);

        }
    }
}
