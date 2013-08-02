using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape.Action
{
    public class HttpGet 
    {
        public IntelliScraper.Db.httpGet rule { get; set; }
        public HttpGet(IntelliScraper.Db.httpGet rule)
        {
            this.rule = rule;
        }

        /// <summary>
        /// Execute Httpget
        /// </summary>
        /// <param name="customUrl">Custom url (ovverided from if rule have own custom url)</param>
        /// <param name="async"></param>
        /// <returns>Html</returns>
        public string Run(string customUrl,bool async)
        {
            string url = customUrl;
            if (rule.urlType == Db.httpGetUrlType.custom && !string.IsNullOrEmpty(rule.customUrl))
                url = rule.customUrl;
            return HttpUtils.getHtml(url, rule.customUserAgent, rule.customHttpHeadersInfo, async);
        }

       

    }
}
