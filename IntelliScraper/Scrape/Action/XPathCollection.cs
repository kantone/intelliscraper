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
    public class XPathCollection
    {       
        Db.xpathCollection rule { get; set; }
        public List<List<KeyValuePair<string, object>>> res = new List<List<KeyValuePair<string, object>>>();
        public XPathCollection(Db.xpathCollection rule)
        {
            this.rule = rule;
        }

        public List<List<KeyValuePair<string, object>>> Run(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            return run(node);
        }

        /// <summary>
        /// Run xpath from html or node
        /// </summary>
        public List<List<KeyValuePair<string, object>>> run(HtmlNode node)
        {
            Factory.Instance.iInfo(string.Format("Running xpathCollection id : {0}", rule.id));

            HtmlNodeCollection nodes = new HtmlNodeCollection(node);
            HtmlNodeCollection n2 = node.SelectNodes(rule.xpath);
            if (n2 != null)
            {
                foreach (HtmlNode n in n2)
                    nodes.Add(n);
            }

            //run
            if (node != null)
            {

                foreach (HtmlNode n in nodes)
                {
                    List<KeyValuePair<string, object>> last_val = null;
                    if (rule.xpathSingle != null)
                    {
                        XPathSingle xs = new XPathSingle(rule.xpathSingle, last_val);
                        last_val = (List<KeyValuePair<string, object>>)xs.Run(n);
                        res.Add(last_val);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Run xpath from html or node
        /// </summary>
        /*public List<List<KeyValuePair<string, object>>> run(HtmlNode node)
        {
            Factory.Instance.iInfo(string.Format("Running xpathCollection id : {0}", rule.id));
                 
            
            //Load nodes
            HtmlNodeCollection nodes = new HtmlNodeCollection(node);
            if (rule.xpath != null)
            {
                foreach (string xpath in rule.xpath)
                {
                    HtmlNodeCollection n2 = node.SelectNodes(xpath);
                    if (n2 != null)
                    {
                        foreach (HtmlNode n in n2)
                            nodes.Add(n);
                    }
                }
            }

            //run
            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    List<KeyValuePair<string, object>> last_val = null;
                    if (rule.xpathSingle != null)
                    {
                        XPathSingle xs = new XPathSingle(rule.xpathSingle, last_val);
                        last_val = (List<KeyValuePair<string, object>>)xs.Run(n);
                        res.Add(last_val);
                    }
                    /*foreach (Db.xpathSingle x in rule.xpathSingle)
                    {
                        XPathSingle xs = new XPathSingle(x, last_val);
                        last_val = (List<KeyValuePair<string, object>>)xs.Run(n);                        
                        res.Add(last_val);
                    }
                }
            }
            return res;
        }*/

    }
}
