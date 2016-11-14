using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Web
{
    public class PageCrawlDetail
    {
        public Uri PageUri { get; set; }
        public string PageTitle { get; set; }

        public System.Net.HttpStatusCode StatusCode { get; set; }
        public List<PageCrawlDetail> AllLinks { get; set; }

        public HtmlAgilityPack.HtmlDocument document { get; set; }

        public PageCrawlDetail(Uri uri)
        {
            PageUri = uri;
            AllLinks = new List<PageCrawlDetail>();            
        }

        public void LoadContent(string content, System.Net.HttpStatusCode statusCode)
        {
            document = new HtmlDocument();
            document.LoadHtml(content);
            StatusCode = statusCode;
            PageTitle = Helpers.WebHelper.GetPageTitle(document);
        }


        public void LoadUris()
        {
            AllLinks = bo.Helpers.WebHelper.GetUris(document, PageUri);
        }
    }
}
