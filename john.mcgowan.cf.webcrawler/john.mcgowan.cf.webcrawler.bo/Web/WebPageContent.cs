using System;
using System.Net;

namespace john.mcgowan.cf.webcrawler.bo.Web
{
    public class WebPageContent
    {
        /* Properties*/
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HttpWebRequest request;
        public HttpWebResponse response;

        public WebPageContent()
        {

        }

        public void SetRequestFromURL(string url)
        {
            request = (HttpWebRequest)HttpWebRequest.Create(url);
        }

        public bool TryGetResponse()
        {
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Debug("http web request invalid", ex);
                return false;
            }
        }
    }
}
