using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class HttpGet : IScrapeAction
    {
       
        Db.httpGet rule { get; set; }
        public bool async { get; set; }
        public HttpGet(Db.httpGet rule, bool async)
        {
            this.rule = rule;
            this.async = async;
        }


        public string getName()
        {
            return "HttpGet";
        }

        public object Run(object input)
        {
            string url = string.Empty;
            if(input != null)
                url = (string)input;

            if (rule.urlType == Db.httpGetUrlType.custom && !string.IsNullOrEmpty(rule.customUrl))
                url = rule.customUrl;
            return HttpUtils.getHtml(url,rule.customUserAgent,rule.customHttpHeadersInfo,async);
        }
    }
}
