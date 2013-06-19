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
using System.IO;

namespace IntelliScraper
{
    /// <summary>
    /// Scraper - run full logic
    /// </summary>
    public class Scraper
    {
        /// <summary>
        /// Run intelliscraping wrapper
        /// </summary>
        public void RunScraping(IntelliScraper.Data.intelliScraper i)
        {
            if (i.settings.usePageLoop)
                runScrapingByPageLoop(i);
            else runScrapingDefault(i,i.Action,false,string.Empty);
        }


        /// <summary>
        /// Loop pages and scraping
        /// </summary>
        private void runScrapingByPageLoop(IntelliScraper.Data.intelliScraper i)
        {
            Factory.Instance.log.Info("Starting scraping by pages loop...");

            //set start end page loop
            int start = 0;
            int end = 0;

            //custom start end
            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromStart)
            {               
                start =  i.settings.pageLoopInfo.start;
                end = i.settings.pageLoopInfo.end;
                Factory.Instance.iInfo("fromStart");
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} - Custom", start, end));
            }

            //find end by rule
            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromZero_to_byRule)
            {
                Data.intelliScraperAction refRule = findRuleById(i.Action, i.settings.pageLoopInfo.ruleId_getEndPage);
                string _end = (string)xpathRunSingle(refRule.Rule.xpath.xpathRule[0], i.Action, refRule.Rule.xpath.xpathRule, true, false, null,false,string.Empty);
                end = Int32.Parse(_end);

                start =  i.settings.pageLoopInfo.start;
                end = i.settings.pageLoopInfo.end;
                Factory.Instance.iInfo(string.Format("fromZero_to_byRule : Checked end page by rule {0}", refRule.Rule.id));
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} - Start from zero & end page fetched", 0, end));
                
            }

            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromCustom_to_byRule)
            {
                start = i.settings.pageLoopInfo.start;
                Data.intelliScraperAction refRule = findRuleById(i.Action, i.settings.pageLoopInfo.ruleId_getEndPage);
                string _end = (string)xpathRunSingle(refRule.Rule.xpath.xpathRule[0], i.Action, refRule.Rule.xpath.xpathRule, true, false, null, false, string.Empty);
                end = Int32.Parse(_end);
                Factory.Instance.iInfo(string.Format("fromCustom_to_byRule : Checked end page by rule {0}", refRule.Rule.id));
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} -  Start from Custom & end page fetched", 0, end));
            }

            //Now i have start and end page
            Factory.Instance.log.Info(string.Format("Looping {0} pages", (end-start).ToString()));           
            for (int ii = start; ii < end; ii++)
            {
                string pageNumber = Factory.Instance.i.settings.pageLoopInfo.appendBeforePageNumber + ii + Factory.Instance.i.settings.pageLoopInfo.appendAfterPageNumber;
                string url = string.Format(Factory.Instance.i.settings.pageLoopInfo.urlFormat,pageNumber);
                Factory.Instance.log.Info(string.Format("Get Html from {0}", url));   
                string html = Scrape.ScraperAction.getHtml(url, i.settings.pageLoopInfo.customHttpHeader);
                i.settings.pageLoopInfo.fetchHtml = html;
                Data.intelliScraperActionCollection actions = new Data.intelliScraperActionCollection();
                foreach (string ruleId in Factory.Instance.i.settings.pageLoopInfo.ruleId_executePage)
                {                    
                    Data.intelliScraperAction refRule = findRuleById(i.Action, ruleId);
                    Factory.Instance.iInfo(string.Format("Determine rule to execute - id = {0}", refRule.Rule.id)); 
                    if (refRule != null)
                        actions.Add(refRule);
                }
                Factory.Instance.iInfo(string.Format("Execute {0} actions rule for page {1}", actions.Count.ToString(),url)); 
                runScrapingDefault(i,actions,true,html);
            }            

        }


        /// <summary>
        /// Scrape data logic
        /// </summary>
        private void runScrapingDefault(IntelliScraper.Data.intelliScraper i,Data.intelliScraperActionCollection actions,bool customHtml,string customHtmlData)
        {         
          
            int x = 0;
            foreach (Data.intelliScraperAction r in actions)
            {
                //Get html
                if (r.Rule.type == Data.RuleEntityType.httpGet)
                {
                    Factory.Instance.iInfo(string.Format("Rule Action : get html from {0}", r.Rule.url));
                    string html = Scrape.ScraperAction.getHtml(r.Rule.url,r.Rule.customHttpHeader);
                    r.Rule.fetchedData = html;
                }

                //Get html from file
                if (r.Rule.type == Data.RuleEntityType.loadHtmlFromfile)
                {
                    Factory.Instance.iInfo(string.Format("Rule Action : get html from file {0}", r.Rule.htmlFileName));
                    if (System.IO.File.Exists(r.Rule.htmlFileName))
                    {
                        string html = System.IO.File.ReadAllText(r.Rule.htmlFileName);
                        r.Rule.fetchedData = html;
                    }
                    else throw new FileNotFoundException(r.Rule.htmlFileName);
                }

                //Get xpath 
                if (r.Rule.type == Data.RuleEntityType.xpath)
                {
                    List<KeyValuePair<string, object>> xpathData = new List<KeyValuePair<string, object>>();

                    //add single
                    if (r.Rule.xpath.type == Data.RuleEntityXpathType.single)
                    {
                        Factory.Instance.iInfo(string.Format("Rule Action : get xpath single for rule id {0}", r.Rule.xpath.xpathRule[0].id));
                        object o = xpathRunSingle(r.Rule.xpath.xpathRule[0], i.Action, r.Rule.xpath.xpathRule, true, false, null,customHtml,customHtmlData);                        
                        string id = string.Format("{0}.{1}", r.Rule.id, r.Rule.xpath.xpathRule[0].id);
                        KeyValuePair<string, object> ks = new KeyValuePair<string, object>(id, o);
                        xpathData.Add(ks);
                        r.Rule.fetchedData = xpathData; 
                    }

                    //add multiple saved as KeyValuePair where id = RuleId.XpathActionId
                    if (r.Rule.xpath.type == Data.RuleEntityXpathType.collection)
                    {
                        Factory.Instance.iInfo(string.Format("Rule Action : get xpath collection - rule id {0}", r.Rule.id));
                        Factory.Instance.iInfo(string.Format("Total xpath actions : {0}", r.Rule.xpath.xpathRule.Count.ToString()));   
                        foreach (Data.xpathEntity a in r.Rule.xpath.xpathRule)
                        {
                            //get previus Data
                            object previusObject = new object();
                            if (a.FindType == Data.xpathEntityFindType.inputData)
                            {
                                Factory.Instance.iInfo(string.Format("Find previus object in rule : id {0}", a.inputRuleId));  
                                Data.intelliScraperAction refRule = findRuleById(i.Action, a.inputRuleId);
                                previusObject = refRule.Rule.fetchedData;
                            }

                            //Case simple Node
                            if (a.inputCollectionType == Data.xpathEntityInputCollectionType.singleNode)
                            {
                                Factory.Instance.iInfo(string.Format("Return single Node : id {0}", a.inputRuleId)); 
                                HtmlNode node = (HtmlNode)previusObject;
                                object o = xpathRunSingle(a, i.Action, r.Rule.xpath.xpathRule, false, true, node, customHtml, customHtmlData);
                                string id = string.Format("{0}.{1}", r.Rule.id, a.id);
                                KeyValuePair<string, object> k = new KeyValuePair<string, object>(id, o);
                                xpathData.Add(k);
                            }

                            //Case node Collection
                            if (a.inputCollectionType == Data.xpathEntityInputCollectionType.nodeCollection)
                            {                               
                                HtmlNodeCollection nodes = (HtmlNodeCollection)previusObject;
                                Factory.Instance.iInfo(string.Format("Loop trought {0} node collection : id {1}",nodes.Count.ToString(), a.inputRuleId)); 
                                foreach (HtmlNode node in nodes)
                                {
                                    object o = xpathRunSingle(a, i.Action, r.Rule.xpath.xpathRule, false, true, node, customHtml, customHtmlData);
                                    string id = string.Format("{0}.{1}", r.Rule.id, a.id);
                                    KeyValuePair<string, object> k = new KeyValuePair<string, object>(id, o);
                                    xpathData.Add(k);
                                }
                            }



                        }

                        //Ordering
                        //Example 2 xpath rules
                        //xpathData was in format : 1,2,3,4
                        //I need : 1,3 and 2,4
                        Factory.Instance.iInfo(string.Format("Reorder fetched data for saving purpose")); 
                        List<KeyValuePair<string, object>> xpathDataOrder = new List<KeyValuePair<string, object>>();
                        int tot = r.Rule.xpath.xpathRule.Count;
                        int t2 = (xpathData.Count / tot);
                        for (int ii = 0; ii < t2; ii++)
                        {
                            for (int s = 0; s < tot; s++)
                            {
                                int index = (t2 * s) + ii;
                                string id = xpathData[index].Key;
                                object o = xpathData[index].Value;
                                KeyValuePair<string, object> k2 = new KeyValuePair<string, object>(id, o);
                                xpathDataOrder.Add(k2);
                            }

                        }

                        r.Rule.fetchedData = xpathDataOrder;
                    }
                }

                //Now Save Fetched Data
                if (r.Save != null)
                {
                    if (r.Save.enableSave == true)
                    {
                        if (Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.csv)
                        {
                            Factory.Instance.iInfo(string.Format("Start saving data into csv file {0}", Factory.Instance.i.storeInfo.csvFileSaveTo));
                            string[] cols = Factory.Instance.i.storeInfo.csvHeader.Split(',');
                            Factory.Instance.iInfo(string.Format("Csv have {0} columns", cols.Length.ToString()));
                            List<KeyValuePair<string, object>> dt = (List<KeyValuePair<string, object>>)r.Rule.fetchedData;

                            List<string[]> rows = ScraperAction.mapCsvToRows(r.Save.mapping, r.Rule.xpath.xpathRule, cols, r.Rule.id, dt);

                            Factory.Instance.iInfo(string.Format("Saving {0} rows", rows.Count.ToString()));
                            foreach (string[] rowData in rows)
                            {
                                Factory.Instance.appendCsvLine(rowData);
                            }
                        }
                    }
                }

                x++;
            }
                
        }

        /// <summary>
        /// Perform xpath action by definitions
        /// </summary>
        private object xpathRunSingle(Data.xpathEntity r, Data.intelliScraperActionCollection rules, Data.xpathEntityCollection xpathRules, bool loadhtml, bool useCustomNode, HtmlNode customNode,bool customHtml,string customHtmlData)
        {

            object res = new object();
            string html = string.Empty;
            HtmlDocument doc = new HtmlDocument();

            //Load html page
            if (loadhtml && !customHtml)
            {
              
                //Load html By page Url Searcher
                if (r.FindType == Data.xpathEntityFindType.pageLoop)
                {
                    if (Factory.Instance.i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromCustom_to_byRule ||
                        Factory.Instance.i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromZero_to_byRule)
                    {
                        Factory.Instance.iInfo(string.Format("Loading html by PageLoop - page {0}", Factory.Instance.i.settings.pageLoopInfo.urlSearcher));
                        html = Scrape.ScraperAction.getHtml(Factory.Instance.i.settings.pageLoopInfo.urlSearcher, Factory.Instance.i.settings.pageLoopInfo.customHttpHeader);
                    }
                    else
                    {
                        Factory.Instance.iInfo(string.Format("Loading html by PageLoop - page {0}", Factory.Instance.i.settings.pageLoopInfo.fetchHtml));                       
                        html = Factory.Instance.i.settings.pageLoopInfo.fetchHtml;
                    }
                }

                //Get from Rule
                if (r.FindType == Data.xpathEntityFindType.inputData && !string.IsNullOrEmpty(r.inputRuleId))
                {
                    Data.intelliScraperAction refRule = findRuleById(rules, r.inputRuleId);
                    Factory.Instance.iInfo(string.Format("Loading html by rule reference fetched html - rule id {0}", r.inputRuleId)); 
                    if (refRule != null)
                        html = (string)refRule.Rule.fetchedData;
                }

                //get Html Web Directly
                if (r.FindType == Data.xpathEntityFindType.getUrl)
                {
                    Factory.Instance.iInfo(string.Format("Load rule html : {0}", r.url));
                    html = Scrape.ScraperAction.getHtml(r.url, null);
                }

                doc.LoadHtml(html);

            }
            if (customHtml)
            {
                Factory.Instance.iInfo(string.Format("Load custom - html previusly fetched"));
                doc.LoadHtml(customHtmlData);
            }
            
            //Make xPath                    

            
            HtmlNode node = null;
            if (useCustomNode)
            {
                node = customNode.SelectSingleNode(r.xpath);
            }
            else node = doc.DocumentNode.SelectSingleNode(r.xpath);

            if (r.getType == Data.xpathEntityGetType.text)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Text"));
                string txt = node.InnerText;
                res = multipletxtOperations(txt, r.postProcessData);
                Factory.Instance.log.Info(string.Format("Found value - {0}",res));
            }
            if (r.getType == Data.xpathEntityGetType.html)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Html"));
                string txt = node.InnerHtml;
                res = multipletxtOperations(txt, r.postProcessData);
                Factory.Instance.log.Info(string.Format("Found value - {0}",res));
            }

            if (r.getType == Data.xpathEntityGetType.attribute)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Attribute Value {0}",r.attributeName));
                if (node.Attributes[r.attributeName] != null)
                {
                    string txt = node.Attributes[r.attributeName].Value;
                    res = multipletxtOperations(txt, r.postProcessData);
                    Factory.Instance.log.Info(string.Format("Found value - {0}",res));
                }
            }


            if (r.getType == Data.xpathEntityGetType.singleNode)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Node"));
                res = doc.DocumentNode.SelectSingleNode(r.xpath);
            }

            if (r.getType == Data.xpathEntityGetType.nodeCollection)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Node Collection"));
                res = doc.DocumentNode.SelectNodes(r.xpath);
            }


            return res;
        }

        /// <summary>
        /// Make text operation like substring,trim,replace and more
        /// </summary>
        private string multipletxtOperations(string txt, IntelliScraper.Data.ResultActionEntityCollection resultActions)
        {
            foreach (IntelliScraper.Data.ResultActionEntity a in resultActions)
                txt = txtOperations(txt, a);
            return txt;
        }

        /// <summary>
        /// Make text operation like substring,trim,replace and more
        /// </summary>
        private string txtOperations(string txt, IntelliScraper.Data.ResultActionEntity resultAction)
        {
            txt = txt.Replace("\n", "");

            //trim
            if (resultAction.trim)
                txt = txt.Trim();

            //Substring
            if (resultAction.makeSubstring)
            {
                Factory.Instance.log.Info("Make Substring of fetched data");

                if(resultAction.substringType == Data.ResultActionEntitySubstringType.simple && resultAction.substringFrom >0)
                {
                    Factory.Instance.iInfo(string.Format("Substring from {0}", resultAction.substringFrom));
                    txt = txt.Substring(resultAction.substringFrom);
                }
                if (resultAction.substringType == Data.ResultActionEntitySubstringType.fromTo && resultAction.substringFrom > 0 &&  resultAction.substringTo >0)
                {
                    Factory.Instance.iInfo(string.Format("Substring from {0} to {1}", resultAction.substringFrom, resultAction.substringTo));
                    txt = txt.Substring(resultAction.substringFrom, resultAction.substringTo);                    
                }

                if (resultAction.substringType == Data.ResultActionEntitySubstringType.search && !string.IsNullOrEmpty(resultAction.substringSearch))
                {
                    Factory.Instance.iInfo(string.Format("Substring from - by string search - {0}", resultAction.substringSearch));                   
                    int index = txt.IndexOf(resultAction.substringSearch);
                    if (index > 0)
                    {
                        txt = txt.Substring(index+1);
                    }
                }

                if (resultAction.substringType == Data.ResultActionEntitySubstringType.searchTo && !string.IsNullOrEmpty(resultAction.substringSearch))
                {
                    Factory.Instance.iInfo(string.Format("Substring from/to - by string search - {0}", resultAction.substringSearch)); 
                    int index = txt.IndexOf(resultAction.substringSearch);
                    if (index > 0)
                    {
                        txt = txt.Substring(0,index);
                    }
                }

            }

            //Replace text
            if (resultAction.replace)
            {
                Factory.Instance.iInfo(string.Format("Replace text {0} with {1}", resultAction.findText, resultAction.replaceText));                   
                if (!string.IsNullOrEmpty(resultAction.findText) && !string.IsNullOrEmpty(resultAction.replaceText))
                {
                    txt = txt.Replace(resultAction.findText, resultAction.replaceText);
                }
            }

            //if Start With
            if (resultAction.startWithEnabled && !string.IsNullOrEmpty(resultAction.startWithVal)  && txt.StartsWith(resultAction.startWithVal))
            {
                Factory.Instance.iInfo(string.Format("If start with append before e/o after"));                                   
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.before)                 
                    txt = resultAction.startWithAddVal + txt;
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.after)
                    txt = txt + resultAction.startWithAddVal;
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.both)
                    txt = resultAction.startWithAddVal + txt + resultAction.startWithAddVal;
            }

            //if End With
            if (resultAction.endWithEnabled && !string.IsNullOrEmpty(resultAction.endWithVal) && txt.StartsWith(resultAction.endWithVal))
            {
                Factory.Instance.iInfo(string.Format("If end with append before e/o after"));   
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.before)
                    txt = resultAction.endWithAddVal + txt;
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.after)
                    txt = txt + resultAction.endWithAddVal;
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.both)
                    txt = resultAction.endWithAddVal + txt + resultAction.endWithAddVal;
            }

            //trim
            if (resultAction.trim)
            {
                Factory.Instance.log.Info("Trim value");
                txt = txt.Trim();
            }

            return txt;
        }
                    

        /// <summary>
        /// Find previusly fetched data by id
        /// </summary>
        private Data.intelliScraperAction findRuleById(Data.intelliScraperActionCollection rules, string inputId)
        {
            var values = from x in rules where x.Rule.id == inputId select x;
            foreach (var val in values)
                return val;
            return null;
        }
        
    }


    
}
