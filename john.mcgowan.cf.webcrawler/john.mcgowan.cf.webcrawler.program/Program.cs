using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using john.mcgowan.cf.webcrawler.bo;
using System.Net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4net.config", Watch = true)]
namespace john.mcgowan.cf.webcrawler.program
{ 
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bo.Web.WebPageContent content;

        //to do parameterise
        private const int SleepTime = 500;
        private const int NoOfThreads = 1;
        private const string outputFilePath = @"c:\temp\webcrawler.csv";
        static void Main(string[] args)
        {

            
            try
            {
                if (args.Length == 1 && HelpRequired(args[0]))
                {
                    DisplayHelp();
                }
                else
                {
                    // validate url
                    string url = args[0];
                    ProcessUrl(url);


                }



            }
            catch(Exception ex)
            {
                log.Error("webcrawler.program", ex);
            }
        }

        private static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "/?";
        }
        private static void DisplayHelp()
        {
            Console.WriteLine(@"# Help for web crawler");
            Console.WriteLine(@"First argument is a full url with http/https definition");
            Console.WriteLine(@"e.g. <command name> http://www.t-shirt.co.uk");
            Console.WriteLine(@"");
            Console.WriteLine(@"Press any key to continue...");
            Console.ReadLine();

        }

        private static void ProcessUrl(string url)
        {
            content = new bo.Web.WebPageContent();
            content.SetRequestFromURL(url);

            if (bo.Helpers.WebHelper.IsUrlValid(url)&& content.TryGetResponse())
            {
                // Now we can get the robots file
                Uri baseUri = new Uri(url);
                Uri robotsUri = new Uri(bo.Helpers.WebHelper.CombineUrl(baseUri, "/robots.txt"));
                content = new bo.Web.WebPageContent();
                content.SetRequestFromURL(robotsUri.AbsoluteUri);
                string responseString = string.Empty;
                if (content.TryGetResponse())
                {
                    responseString =  bo.Helpers.WebHelper.ResponseToString(content.response);      
                }

                bo.Web.RobotsTxt robots = bo.Helpers.RobotsHelper.ParseRobotsTxt(responseString);

                bo.Web.PageCrawlDetail pageDetails = new bo.Web.PageCrawlDetail(baseUri);
                

                try
                {
                    //Start the Crawling
                    bo.Crawler.CrawlerProcessing crw = new bo.Crawler.CrawlerProcessing(NoOfThreads, SleepTime, baseUri, robots);
                    crw.Crawl(pageDetails).Wait();
                    bo.Helpers.FileHelper.PageDetailToCSV(crw.CrawlList.UrlsCompleted, outputFilePath);
                }
                catch (AggregateException e)
                {
                    foreach (var ex in e.InnerExceptions)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                    Console.ReadLine();
                }


            }
            else
            {
                string info = "Invalid URL: " + url;
                Console.WriteLine(url);
                log.Info(url);
            }

        }
    }
}
