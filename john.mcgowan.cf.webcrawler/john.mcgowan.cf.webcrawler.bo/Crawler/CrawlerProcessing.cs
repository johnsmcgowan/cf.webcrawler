using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Crawler
{

    public class CrawlerProcessing
    {
        public readonly CrawlList CrawlList;
        private readonly List<Task<string>> runningTasks = new List<Task<string>>();
        private readonly HttpClient client;
        private readonly int maxConcurrentDownload;
        private readonly Uri BaseUri;
        private readonly int SleepTime;
        private readonly Web.RobotsTxt Robots;

        public CrawlerProcessing(int maxConcurrentDownload, int sleepTime, Uri uri, Web.RobotsTxt robots)
        {
            CrawlList = new CrawlList(robots);
            client = new HttpClient();
            this.maxConcurrentDownload = maxConcurrentDownload;
            ServicePointManager.DefaultConnectionLimit = maxConcurrentDownload;
            SleepTime = sleepTime;
            BaseUri = uri;
            Robots = robots;
        }

        public async Task<bool> Crawl(Web.PageCrawlDetail startUrl)
        {
            runningTasks.Add(ProcessUrl(startUrl));

            while (runningTasks.Any())
            {
                var completedTask = await Task.WhenAny(runningTasks);
                runningTasks.Remove(completedTask);
                var pageHtml = await completedTask;
                Thread.Sleep(SleepTime); // added so there is not too many
                while (CrawlList.HasNext() && runningTasks.Count < maxConcurrentDownload)
                {
                    var url = CrawlList.GetNext();
                    runningTasks.Add(ProcessUrl(url));
                }
            }

            return true;
        }

        private async Task<string> ProcessUrl(Web.PageCrawlDetail url)
        {
            Console.WriteLine("url " + url);
            var response = await client.GetAsync(url.PageUri.AbsoluteUri);
            var content = await response.Content.ReadAsStringAsync();

            url.LoadContent(content, response.StatusCode);
            url.LoadUris();
            CrawlList.AddUrls(url.AllLinks);
            url.AllLinks = new List<Web.PageCrawlDetail>(); // dont need it anymore
            CrawlList.UrlsCompleted.Add(url);
            return content;
        }
    }
}
