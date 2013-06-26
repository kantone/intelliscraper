using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class HttpPost : IScrapeAction
    {
        public string Name = "HttpPost";
        Db.httpPost rule { get; set; }
        public bool async { get; set; }
        public HttpPost(Db.httpPost rule, bool async)
        {
            this.rule = rule;
            this.async = async;            
        }

        public string getName()
        {
            return "HttpPost";
        }


        public object Run(object input)
        {
            if (input != null)
            {
                List<KeyValuePair<string, object>> vals = (List<KeyValuePair<string, object>>)input;

                //build post data
                string _prms = buildPostData(vals);
               
                string url = string.Empty;
                if (rule.urlType == Db.httpPostUrlType.custom)
                {
                    if (!string.IsNullOrEmpty(rule.customUrl))
                        url = rule.customUrl;
                }

                if (rule.urlType == Db.httpPostUrlType.fromInput)
                {
                    KeyValuePair<string, object> t = (from x in vals where x.Key == rule.inputUrlAttributeKey select x).SingleOrDefault();
                    try
                    {
                        url = (string)t.Value;
                    }
                    catch (Exception ex)
                    {
                        Factory.Instance.log.Error(ex);
                    }
                }

                if (!string.IsNullOrEmpty(url))
                  return  HttpUtils.postHtml(url, rule.customUserAgent, rule.customHttpHeadersInfo, _prms);

            }




            return null;
           
        }

        /// <summary>
        /// Build post data Params
        /// </summary>
        private string buildPostData(List<KeyValuePair<string, object>> vals)
        {
            string _prms = string.Empty;
            foreach (Db.httpPostPostData pd in rule.postData)
            {
                if (pd.type == Db.httpPostPostDataType.custom)
                    _prms += string.Format("{0}={1}&", pd.key, pd.customValue);

                if (pd.type == Db.httpPostPostDataType.fromInput)
                {
                    try
                    {
                        KeyValuePair<string, object> t = (from x in vals where x.Key == pd.inputAttributeKey select x).SingleOrDefault();
                        _prms += string.Format("{0}={1}&", pd.key, t.Value);
                    }
                    catch (Exception ex)
                    {
                        Factory.Instance.log.Error(ex);
                    }
                }
            }
            if (_prms.EndsWith("&"))
                _prms = _prms.Substring(0, (_prms.Length - 1));
            return _prms;
        }
    }
}
