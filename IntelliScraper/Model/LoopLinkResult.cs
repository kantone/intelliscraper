using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Model
{
    public class LoopLinkResult
    {
        public string link { get; set; }
        public string html { get; set; }
        public List<KeyValuePair<string, object>> xpathResultSingle { get; set; }
        public List<List<KeyValuePair<string, object>>> xpathResultCollection { get; set; }
        public LoopLinkResult(string link, string html, List<KeyValuePair<string, object>> xpathResultSingle, List<List<KeyValuePair<string, object>>> xpathResultCollection)
        {
            this.link = link;
            this.html = html;
            this.xpathResultSingle = xpathResultSingle;
            this.xpathResultCollection = xpathResultCollection;
        }

    }
}
