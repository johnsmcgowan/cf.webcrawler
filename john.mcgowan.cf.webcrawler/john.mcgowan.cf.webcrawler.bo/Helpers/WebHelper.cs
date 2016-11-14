using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Helpers
{
    public class WebHelper
    {

        public static string ResponseToString(HttpWebResponse response)
        {
            var responseString = string.Empty;
            var encoding = response.CharacterSet == ""
                    ? Encoding.UTF8
                    : Encoding.GetEncoding(response.CharacterSet);

            using (var stream = response.GetResponseStream())
            {
                var reader = new System.IO.StreamReader(stream, encoding);
                responseString = reader.ReadToEnd();
            }
            return responseString;
        }

        public static bool IsUrlValid(string url)
        {
            Uri uri = null;
            if (url.Length > 12 && !Uri.TryCreate(url, UriKind.Absolute, out uri) || null == uri)
                return false;
            else
                return true;
        }
        public static Uri GetUrifromHREF(Uri baseUri, string href)
        {
            Uri uri = null;
            if(IsUrlValid(href))
            {
                Uri.TryCreate(href, UriKind.Absolute, out uri);
            }
            else
            {
                if(href.StartsWith("/")) // make sure its like relative path
                {
                    string url = CombineUrl(baseUri, href);
                    if (IsUrlValid(url))
                    {
                        Uri.TryCreate(url, UriKind.Absolute, out uri);
                    }
                }
            }
            return uri;
        }

        public static string CombineUrl (Uri baseUri, string href)
        {
            return string.Format("{0}://{1}{2}", baseUri.Scheme, baseUri.Authority, href);
        }

        public static List<Web.PageCrawlDetail> GetUris(HtmlAgilityPack.HtmlDocument doc, Uri baseUri)
        {
            List<Web.PageCrawlDetail> uris = new List<Web.PageCrawlDetail>();
            foreach (HtmlAgilityPack.HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                // Get the value of the HREF attribute
                string hrefValue = link.GetAttributeValue("href", string.Empty);

                Uri uri = GetUrifromHREF(baseUri, hrefValue);
                if (uri != null && uris.Where(x => x.PageUri.AbsoluteUri == uri.AbsoluteUri).Count() == 0)
                {
                    Web.PageCrawlDetail url = new Web.PageCrawlDetail(uri);
                    uris.Add(url);
                }
            }
            return uris;
        }

        public static string GetPageTitle(HtmlAgilityPack.HtmlDocument doc)
        {
            string pageTitle = string.Empty;

            HtmlAgilityPack.HtmlNode node = doc.DocumentNode.Descendants("title").SingleOrDefault();
            if(node != null)
            { 
                pageTitle = node.InnerText;
            }
            return pageTitle;
        }
    }
}
