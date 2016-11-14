using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Crawler
{
    public class CrawlList
    {
        public readonly Queue<Web.PageCrawlDetail> UrlsToCrawl;
        public readonly List<Web.PageCrawlDetail> UrlsCompleted;
        public readonly bo.Web.RobotsTxt Robots;

        public CrawlList(bo.Web.RobotsTxt robots)
        {
            Robots = robots;
            UrlsToCrawl = new Queue<Web.PageCrawlDetail>();
            UrlsCompleted = new List<Web.PageCrawlDetail>();
        }

        public bool HasNext()
        {
            return UrlsToCrawl.Any();
        }

        public Web.PageCrawlDetail GetNext()
        {
            return UrlsToCrawl.Dequeue();
        }

        public void AddUrls(List<Web.PageCrawlDetail> urls)
        {
            foreach (var url in urls)
            {
                AddUrl(url);
            }
        }

        public void AddUrl(Web.PageCrawlDetail url)
        {
            if (UrlToBeAdded(url))
            { 
                UrlsToCrawl.Enqueue(url);
            }


        }

        public bool UrlToBeAdded(Web.PageCrawlDetail url)
        {
            bool isInQueue = UrlsToCrawl.Where(x => x.PageUri.AbsoluteUri == url.PageUri.AbsoluteUri).Count() > 0;
            bool isInComplete = UrlsCompleted.Where(x => x.PageUri.AbsoluteUri == url.PageUri.AbsoluteUri).Count() > 0;
            bool isAllowed = Robots.DisallowedList.Where(x => url.PageUri.AbsolutePath.StartsWith(x)).Count() == 0;
            bool isInDomain = Robots.BastUri.Authority == url.PageUri.Authority;

            return !isInQueue && !isInComplete && isAllowed && isInDomain;
        }
    }
}
