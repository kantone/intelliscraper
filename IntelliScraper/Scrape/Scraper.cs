using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using IntelliScraper.Scrape;
using System.Xml.Linq;
using IntelliScraper;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace IntelliScraper
{
    /// <summary>
    /// Scraper - run full logic
    /// </summary>
    public class Scraper
    {
        
        public void RunScraping(IntelliScraper.Data.intelliScraper i)
        {
            List<ScrapeData> data = new List<ScrapeData>();

            foreach (Data.intelliScraperStep step in i.scrapeRules)
            {
                //Get html
                if (step.type == Scrape.ScrapeRuleDef.getHtml)
                {
                    ScrapeData dt = new ScrapeData();
                   string html = Scrape.ScraperAction.getHtml(step.url);
                   data = addData(data, step.id, step.type, html,"");                   
                }
                
                //Get xpath single
                if (step.type == Scrape.ScrapeRuleDef.xpathSingle)
                {
                    string html =(string) findByInputId(data,step.inputId);
                    if (!string.IsNullOrEmpty(html))
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);
                        data = addData(data, step.id, step.type, doc.DocumentNode.SelectSingleNode(step.xpath), "");
                    }
                }

                //Xpath Loop
                if (step.type == Scrape.ScrapeRuleDef.xpathLoop)
                {
                    HtmlNode val = (HtmlNode)findByInputId(data, step.inputId);
                    List<ScrapeData> dataStep = new List<ScrapeData>();
                    if (val != null)
                    {
                       HtmlNodeCollection nodes = val.SelectNodes(step.xpath);

                       foreach (IntelliScraper.Data.intelliScraperStepLoopGetvalue value in step.loop.loopstep)
                       {
                           foreach (HtmlNode node in nodes)
                           {
                               //Type Text
                               if (value.type == Scrape.ScrapeRuleDef.xpathLoopStepText)
                               {
                                   HtmlNode n = node.SelectSingleNode(value.xpath);
                                   string txt = n.InnerText;
                                   txt = txtOperations(txt, value);
                                   dataStep = addData(dataStep, value.name, value.type, txt, value.mapTitle);
                               }
                           }
                       }
                    }
                }
            }
        }


        /// <summary>
        /// Make text operation like substring,trim,replace and more
        /// </summary>
        private string txtOperations(string txt, IntelliScraper.Data.intelliScraperStepLoopGetvalue value)
        {
            txt = txt.Replace("\n", "");

            //trim
            if (value.trim)
                txt = txt.Trim();

            //Substring
            if (!string.IsNullOrEmpty(value.substring))
            {
                txt = subString(value.substring, txt);
            }

            //Replace text
            if (!string.IsNullOrEmpty(value.findText) && !string.IsNullOrEmpty(value.replaceText))
            {
                txt = txt.Replace(value.findText, value.replaceText);
            }

            //if Start With
            if (value.ifStartWith != null)
            {
                if (!string.IsNullOrEmpty(value.ifStartWith.value))
                {
                    //Substring
                    if (!string.IsNullOrEmpty(value.ifStartWith.substring))
                    {
                        txt = subString(value.ifStartWith.substring, txt);
                    }

                    if (!string.IsNullOrEmpty(value.ifStartWith.attachBefore))
                        txt = value.ifStartWith.attachBefore + txt;
                    if (!string.IsNullOrEmpty(value.ifStartWith.attachAfter))
                        txt = txt + value.ifStartWith.attachAfter;
                }
            }

            //if Start With
            if (value.ifEndWith != null)
            {
                if (!string.IsNullOrEmpty(value.ifEndWith.value))
                {
                    //Substring
                    if (!string.IsNullOrEmpty(value.ifEndWith.substring))
                    {
                        txt = subString(value.ifEndWith.substring, txt);
                    }

                    if (!string.IsNullOrEmpty(value.ifEndWith.attachBefore))
                        txt = value.ifEndWith.attachBefore + txt;
                    if (!string.IsNullOrEmpty(value.ifEndWith.attachAfter))
                        txt = txt + value.ifEndWith.attachAfter;
                }
            }

            return txt;
        }
        
        /// <summary>
        /// Make a substring 
        /// </summary>
        /// <param name="value">can be 0,10 or 10</param>
        private string subString(string value,string txt)
        {
            if (value.Contains(","))
            {
                string[] splitted = value.Split(',');
                int from = Int32.Parse(splitted[0]);
                int to = Int32.Parse(splitted[1]);
                if (txt.Length >= to)
                    to = txt.Length - 1;

                txt = txt.Substring(from, to);
            }
            else
            {
                txt = txt.Substring(Int32.Parse(value));
            }
            return txt;
        }
        
        /// <summary>
        /// Add data
        /// </summary>
        private List<ScrapeData> addData( List<ScrapeData> data,string id,string type,object val,string mappingName)
        {
            data.Add(new ScrapeData(id, val, type, mappingName));
            return data;
        }

        /// <summary>
        /// Find previusly fetched data by id
        /// </summary>
        private object findByInputId(List<ScrapeData> data,string inputId)
        {
            var values = from x in data where x.Id == inputId select x;
            foreach (var val in values)
                return val.Value;

            return null;
        }
        
    }


    
}
