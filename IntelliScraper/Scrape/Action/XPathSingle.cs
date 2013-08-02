using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using IntelliScraper.Scrape;
using System.Threading.Tasks;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class XPathSingle 
    {
        
        Db.xpathSingle rule { get; set; }
        List<KeyValuePair<string, object>> res = new List<KeyValuePair<string, object>>();
        public XPathSingle(Db.xpathSingle rule, List<KeyValuePair<string, object>> resToAdd)
        {
            if (resToAdd != null)
                res.AddRange(resToAdd);
            this.rule = rule;
        }

        public List<KeyValuePair<string, object>> Run(string html)
        {
            HtmlNode node = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            node = doc.DocumentNode;
            return run(node);
        }

        public List<KeyValuePair<string, object>> Run(HtmlNode node)
        {
            return run(node);
        }       

        public List<KeyValuePair<string, object>> Run(HtmlDocument doc, string html)
        {
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            return run(node);
        }
       
        /// <summary>
        /// Run xpath from html or node
        /// </summary>
        private List<KeyValuePair<string, object>> run(HtmlNode node)
        {
            Factory.Instance.iInfo(string.Format("Running xpathSingle id : {0}", rule.id));

            if (node == null)
                return new List<KeyValuePair<string, object>>();

            //Get all attriibutes by type and save to List<KeyValuePair<string, object>>
             foreach (Db.xpathSingleAttributes attr in rule.attributes)
            {
                object val = null;

                if (attr.getType == Db.xpathSingleAttributesGetType.nodeCollection)
                    val = node.SelectNodes(attr.xpath);
                else if (attr.getType == Db.xpathSingleAttributesGetType.count)
                {
                    HtmlNodeCollection c = node.SelectNodes(attr.xpath);
                    if (c != null)
                        val = c.Count.ToString();
                }
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
                            if(attr.getType != Db.xpathSingleAttributesGetType.html && attr.getType != Db.xpathSingleAttributesGetType.nodeCollection && 
                                attr.getType != Db.xpathSingleAttributesGetType.singleNode)
                            Factory.Instance.iInfo(string.Format("{0} = {1}",attr.id,val));
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
