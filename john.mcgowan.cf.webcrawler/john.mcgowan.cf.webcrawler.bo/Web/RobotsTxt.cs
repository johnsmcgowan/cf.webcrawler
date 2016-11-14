using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Web
{
    public class RobotsTxt
    {
        public Uri BastUri { get; set; }
        public List<string> DisallowedList { get; set; } // List that holds Urls which shouldn't be crawled if any

        public RobotsTxt()
        {
            BastUri = null;
            DisallowedList = new List<string>();
        }
        public RobotsTxt(Uri baseUri)
        {
            BastUri = baseUri;
            DisallowedList = new List<string>();
        }
    }
}
