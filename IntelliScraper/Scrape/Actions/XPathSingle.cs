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
    public class XPathSingle : IScrapeAction
    {
        Db.xpathSingle rule { get; set; }
        public XPathSingle(Db.xpathSingle rule)
        {
            this.rule = rule;
        }
       
        /// <summary>
        /// Run xpath from html or node
        /// </summary>
        public object Run(object input)
        {
           
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

            //Get all attriibutes by type and save to List<KeyValuePair<string, object>>
            List<KeyValuePair<string, object>> res = new List<KeyValuePair<string, object>>();
            foreach (Db.xpathSingleAttributes attr in rule.attributes)
            {
                object val = null;

                if (attr.getType == Db.xpathSingleAttributesGetType.nodeCollection)
                    val = node.SelectNodes(attr.xpath);               
                else
                {
                    string val2 = string.Empty;
                    HtmlNode n = node.SelectSingleNode(attr.xpath);
                    if (n != null)
                    {
                        if (attr.getType == Db.xpathSingleAttributesGetType.singleNode)
                            val = n;
                        else
                        {
                            if (attr.getType == Db.xpathSingleAttributesGetType.text)
                                val2 = n.InnerText.Trim();

                            if (attr.getType == Db.xpathSingleAttributesGetType.html)
                                val2 = n.InnerHtml.Trim();

                            if (attr.getType == Db.xpathSingleAttributesGetType.attribute)
                            {
                                if (n.Attributes[attr.attributeName] != null)
                                    val2 = n.Attributes[attr.attributeName].Value;
                            }

                            val = postProcessResult(val2, attr);
                        }
                    }                    
                }
                res.Add(new KeyValuePair<string, object>(attr.id, val));
            }

            return res;
        }


        /// <summary>
        /// Wrapper for post process result by rules
        /// </summary>
        private string postProcessResult(string val, Db.xpathSingleAttributes attr)
        {
            //Add post process trigger
            List<Db.postProcess> pp = new List<Db.postProcess>();
            if(attr.postProcessTrigger!=null)
                pp.AddRange(attr.postProcessTrigger);

            //add post process trigger by group definition
            if (attr.postProcessGroupId != null)
            {
                foreach (string pid in attr.postProcessGroupId)
                {
                    Db.xpathSinglePostProcessTriggerGroup gr = (from x in rule.postProcessTriggerGroup where x.id == pid select x).FirstOrDefault();
                    pp.AddRange(gr.postProcessTrigger);
                }
            }
            

            foreach (Db.postProcess trigger in pp)
            {                
                if (trigger.type ==  Db.postProcessType.append)
                    val = val.append(trigger.id);

                if (trigger.type == Db.postProcessType.htmlEncodeDecode)
                    val = val.htmlDecodeEncode(trigger.id);

                if (trigger.type == Db.postProcessType.regex)
                    val = val.regularExpression(trigger.id);

                if (trigger.type == Db.postProcessType.replace)
                    val = val.replace(trigger.id);

                if (trigger.type == Db.postProcessType.startEndWith)
                    val = val.startEndWith(trigger.id);

                if (trigger.type == Db.postProcessType.substring)
                    val = val.substring(trigger.id);

                if (trigger.type == Db.postProcessType.trim)
                    val = val.trim(trigger.id);
            }

            return val;
        }

    }
}
