using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using IntelliScraper.Scrape;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class XPathCollection : IScrapeAction
    {
        Db.xpathCollection rule { get; set; }
        public XPathCollection(Db.xpathCollection rule)
        {
            this.rule = rule;
        }
       
        /// <summary>
        /// Run xpath from html or node
        /// </summary>
        public object Run(object input)
        {
            string url = string.Empty;
            if(input != null)
                url = (string)input;

            //Load node by input type
            HtmlNode node = null;
            if (input.GetType() == typeof(string))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml((string)input);
                node = doc.DocumentNode;
            }
            else if (input.GetType() == typeof(HtmlNode))
                node = (HtmlNode)input;

            //run multiple xpathSingle passing finded node
            List<List<KeyValuePair<string, object>>> res = new List<List<KeyValuePair<string, object>>>();
            HtmlNodeCollection nodes = node.SelectNodes(rule.xpath);
            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    foreach (Db.xpathSingle x in rule.xpathSingle)
                    {
                        XPathSingle xs = new XPathSingle(x);
                        List<KeyValuePair<string, object>> val = (List<KeyValuePair<string, object>>)xs.Run(n);
                        res.Add(val);
                    }
                }
            }


            return res;
        }


 
    }
}
