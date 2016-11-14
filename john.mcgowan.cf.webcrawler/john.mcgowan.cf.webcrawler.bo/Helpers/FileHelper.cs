using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace john.mcgowan.cf.webcrawler.bo.Helpers
{
    public class FileHelper
    {
        public static void PageDetailToCSV(List<Web.PageCrawlDetail> Urls, string path)
        {

            const char SEPARATOR = ',';
            using (StreamWriter writer = new StreamWriter(path))
            {
                Urls.ForEach(line =>
                {
                    var lineString = line.PageUri.AbsoluteUri + SEPARATOR + line.PageTitle + SEPARATOR + (int)line.StatusCode;
                    writer.WriteLine(lineString);
                });
            }
        }
    }
}
