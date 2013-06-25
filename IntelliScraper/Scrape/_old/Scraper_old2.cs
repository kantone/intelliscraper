using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntelliScraper.Scrape;
using HtmlAgilityPack;
using System.IO;

namespace IntelliScraper.Scrape
{
    /// <summary>
    /// Scraper - run full logic
    /// </summary>
    public class ScraperOld2
    {
        private bool getHtmlAsync = false;
        private DateTime startProcess;
        private DateTime endProcess;
        public int PercentProcessedRules = 0;
        public int PercentProcessedPages = 0;
        public int TotalProcessedPages = 0;

        /// <summary>
        /// Run intelliscraping wrapper
        /// </summary>
      /*  public void RunScraping(IntelliScraper.Db.intelliScraper i)
        {
            //Perform Rule
            startProcess = DateTime.Now;
            var j = 0;
            foreach (IntelliScraper.Db.RuleEntity rule in i.Rules)
            {
                //Only rule direct ( trigger-rule will run from other rules )
                if (!rule.isTrigger)
                {
                    //get input data
                    Model.InputValueResult inputData = getInputDataAndPostProcess(rule, i);

                    //Load html from file ( for testing purposes )
                    if (rule.ActionType == Db.RuleEntityActionType.loadHtmlFromfile)
                    {
                        Factory.Instance.iInfo(string.Format("Rule Action : get html from file {0}", rule.htmlFileName));
                        if (System.IO.File.Exists(rule.htmlFileName))
                        {
                            string html = System.IO.File.ReadAllText(rule.htmlFileName);
                            rule.FetchedData.html = html;
                            runRuleAttributes(rule, i,true);
                            this.TotalProcessedPages++;
                        }
                        else throw new FileNotFoundException(rule.htmlFileName);
                    }

                    //Get url and save html here + perform attributes
                    if (rule.ActionType == Db.RuleEntityActionType.httpGet)
                    {
                        rule.FetchedData.html = HttpUtils.getHtml(rule.getUrl, rule, this.getHtmlAsync);
                        runRuleAttributes(rule, i, true);
                        this.TotalProcessedPages++;
                    }


                    if (rule.ActionType == Db.RuleEntityActionType.loop_links)
                    {

                        if (rule.ActionLoopLink.ActionType == Db.RuleEntityActionLoopLinkActionType.inputRuleId_attributeCollection)
                        {
                            Db.RuleEntity refRule = RuleUtils.findRuleById(i.Rules, rule.inputRuleId);
                            List<List<KeyValuePair<string, object>>> vals = (List<List<KeyValuePair<string, object>>> )refRule.Attributes.collection.fetchedNodes;
                            foreach (List<KeyValuePair<string, object>> val in vals)
                            {
                                foreach (KeyValuePair<string, object> v in val)
                                {
                                    if (v.Key == rule.ActionLoopLink.attributeId)
                                    {
                                        string link = (string)v.Value;
                                        rule.FetchedData.html = HttpUtils.getHtml(link, rule, this.getHtmlAsync);
                                        runRuleAttributes(rule, i,false);

                                        break;
                                    }
                                }
                            }
                        }


                    }

                       
                    /******* HERE  */

                   

                   

                    //Perform httpPost -> save Cookie and WebClient Status
                  /*  if (rule.ActionType == Db.RuleEntityActionType.httpPost)
                    {
                        //http://stackoverflow.com/questions/5401501/how-to-post-data-to-specific-url-using-webclient-in-c-sharp

                        this.TotalProcessedPages++;
                        //TO-DO --> how to save webClient status? 
                        //INTO Factory.getWebClientNext() -> passing Client Specification
                        //If use only 1 web client cannot perform multitread request but can clone webClient?

                        //see : http://stackoverflow.com/questions/15212148/copy-cookies-from-one-webclient-to-another
                    }

                    //Upload from InputData
                    if (rule.ActionType == Db.RuleEntityActionType.Upload)
                    {

                    }

                    //Download from InputData
                    if (rule.ActionType == Db.RuleEntityActionType.Download)
                    {

                    }

                    //Perform attibutes list from InputData
                    if (rule.ActionType == Db.RuleEntityActionType.attributeFromInputData)
                    {
                        
                    }
                  
                    //Perform custom Plugin ( return : list of attributes, html, webClient )
                    if (rule.ActionType == Db.RuleEntityActionType.plugin)
                    {

                    }

                    //Perform Ftp Upload from inputData reference
                    if (rule.ActionType == Db.RuleEntityActionType.ftpPut)
                    {

                    }

                    //Perform Zip operation
                    if (rule.ActionType == Db.RuleEntityActionType.zip)
                    {

                    }*/

                    //Trigger Rules

                       //trigger.FtpUploadFiles
                       //trigger.ImageConvert
                       //trigger.OnError
                       //trigger.PostProcessData
                       //trigger.Save                        
                      //trigger.Zip

                    //trigger.triggerRuleId -> run other rules
                    
         /*       }

                //stats
                this.PercentProcessedRules = 100 / i.Rules.Count;
                j++;
            }



            endProcess = DateTime.Now;
            TimeSpan span = endProcess - startProcess;
            Factory.Instance.iInfo(string.Format("Elapsed time : {0} days, {1} hours, {2} minutes, {3} seconds, {4} milliseconds", span.TotalDays, span.TotalHours, span.TotalMinutes, span.TotalSeconds, span.TotalMilliseconds));
        
        }

        private void runActionLoopLink(IntelliScraper.Db.RuleEntity rule, IntelliScraper.Db.intelliScraper i)
        {
            //Determine input link from custom Url
            if (rule.ActionLoopLink.ActionType == Db.RuleEntityActionLoopLinkActionType.customUrl)
            {
                int start = 0;
                int end = 0;

                //get start end                          
                if (!rule.ActionLoopLink.StartEndData.getEndPageNumberFromRule)
                    end = rule.ActionLoopLink.StartEndData.end;
                else
                {
                    string e = (string)RuleUtils.findValueByRuleId(i.Rules, rule.ActionLoopLink.StartEndData.ruleId_getEndPage);
                    end = Int32.Parse(e.Trim());
                }

                if (!rule.ActionLoopLink.StartEndData.getStartPageNumberFromRule)
                    start = rule.ActionLoopLink.StartEndData.start;
                else
                {
                    string e = (string)RuleUtils.findValueByRuleId(i.Rules, rule.ActionLoopLink.StartEndData.ruleId_getStartPage);
                    start = Int32.Parse(e.Trim());
                }

                //Run
                for (int ii = start; ii < end; ii++)
                {
                    string fetchUrl = (string.Format(rule.ActionLoopLink.StartEndData.url, ii));
                    Factory.Instance.log.Info(string.Format("Get Html from {0}", fetchUrl));
                    string html = HttpUtils.getHtml(fetchUrl, rule, this.getHtmlAsync);

                    //set att

                }
            }

            //Determine input link from csv File
            if (rule.ActionLoopLink.ActionType == Db.RuleEntityActionLoopLinkActionType.csvLinkList)
            {

            }

            //Determine input link from database
            if (rule.ActionLoopLink.ActionType == Db.RuleEntityActionLoopLinkActionType.databaseLinkList)
            {

            }

            this.PercentProcessedPages = 0;

            //Loop Trought Pages ( enabled multithread?

            //Trigger Rules

           
            this.TotalProcessedPages++;
        }


        /// <summary>
        /// Set rule Attribute
        /// </summary>
        private IntelliScraper.Db.RuleEntity runRuleAttributes(IntelliScraper.Db.RuleEntity rule, IntelliScraper.Db.intelliScraper i,bool fetchXpath)
        {
            List<List<KeyValuePair<string, object>>> vals = new List<List<KeyValuePair<string, object>>>();
            //Attribute from collection nodes
            if(rule.Attributes.collection != null)
            {
                if(rule.Attributes.collection.useAttributeCollection)
                {
                    //Load Html 
                    string html = rule.FetchedData.html;
                    if(rule.Attributes.collection.getHtmlFromRule)
                    {
                       html = (string)RuleUtils.findRuleValueById(i.Rules,rule.Attributes.collection.inputRuleId);
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    //Loop Nodes
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(rule.Attributes.xpath);                   
                    foreach(HtmlNode node in nodes)
                    {
                        if(node != null)
                        {
                            List<KeyValuePair<string, object>> vals2 = new List<KeyValuePair<string, object>>();
                            foreach (Db.xpathEntity attrRule in rule.Attributes.collection.attribute)
                            {
                                object val = getAttributeXpath(attrRule, node, fetchXpath);
                                vals2.Add(new KeyValuePair<string, object>(attrRule.id, val));
                                attrRule.fetchedData = val;
                            }
                            vals.Add(vals2);
                        }
                       
                    }
                  
                }
                rule.Attributes.collection.fetchedNodes = vals;
            }
            
            
            //Attribute from single Node
            if( rule.Attributes.attribute.Count > 0)
            {
                int ii = 0;         
                foreach (Db.xpathEntity attrRule in rule.Attributes.attribute)
                {
                    //get Node
                    HtmlNode node = null;
                    if (attrRule.inputType == Db.xpathEntityInputType.html)
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(rule.FetchedData.html);
                        node = doc.DocumentNode.SelectSingleNode(attrRule.xpath);
                    }
                    if (attrRule.inputType == Db.xpathEntityInputType.node)
                    {
                        node = (HtmlNode)rule.FetchedData.value;
                    }

                    if(node!=null)
                        rule.Attributes.attribute[ii].fetchedData = getAttributeXpath(attrRule, node, fetchXpath);
                    ii++;
                }
                
              
            }

          

            return rule;
        
        }
        
        /// <summary>
        /// Run single attribute xpath and postProcess Result
        /// </summary>
        private object getAttributeXpath(Db.xpathEntity attrRule,HtmlNode node,bool fetchXpath)
        {
             HtmlNode n = node;
            if(fetchXpath)
                 n = node.SelectSingleNode(attrRule.xpath);
            if (n != null)
            {

                if (attrRule.getType == Db.xpathEntityGetType.text)
                {
                    string val = n.InnerText;
                    val = val.PostProcessString(attrRule.PostProcessData);
                    return val;                  
                }

                if (attrRule.getType == Db.xpathEntityGetType.html)
                {
                    string val = n.InnerHtml;
                    val = val.PostProcessString(attrRule.PostProcessData);
                    return val;
                }

                if (attrRule.getType == Db.xpathEntityGetType.attribute)
                {
                    if (n.Attributes[attrRule.attributeName] != null)
                    {
                        string val = n.Attributes[attrRule.attributeName].Value;
                        val = val.PostProcessString(attrRule.PostProcessData);
                        return val;
                    }
                }

                if (attrRule.getType == Db.xpathEntityGetType.singleNode)
                {
                    object val = n.SelectSingleNode(attrRule.xpath);                   
                    return val;
                }

                if (attrRule.getType == Db.xpathEntityGetType.nodeCollection)
                {
                    object val = n.SelectNodes(attrRule.xpath);                   
                    return val;
                }               
            }           
            return null;
        }

        /// <summary>
        /// Get Rule InputData and process this Data
        /// </summary>
        private Model.InputValueResult getInputDataAndPostProcess(IntelliScraper.Db.RuleEntity rule, IntelliScraper.Db.intelliScraper i)
        {
            Model.InputValueResult res = new Model.InputValueResult();
            //Triggered rules run only inside others rules

            if (rule.InputData.getInputData)
            {
                //Get input Data
                if (rule.InputData.type == Db.RuleEntityInputDataType.loadcustomurlhtml)
                {
                    Factory.Instance.iInfo(string.Format("{0} - GetInputData - Loading custom html from url",rule.Id));
                    res.inputValue = (string)HttpUtils.getHtml(rule.getUrl, rule, this.getHtmlAsync);
                    res.inputType = typeof(string);
                }

                //Get input rule
                if (rule.InputData.type == Db.RuleEntityInputDataType.ruleIdRule)
                {
                    Factory.Instance.iInfo(string.Format("{0} - GetInputData - Get rule from rule id", rule.Id));
                    res.inputValueO = RuleUtils.findRuleById(i.Rules, rule.inputRuleId);
                    res.inputType = typeof(Db.RuleEntity);
                }

                //Get input rule/attribute Value
                if (rule.InputData.type == Db.RuleEntityInputDataType.ruleIdString)
                {
                    if (rule.inputRuleId.Contains("."))
                    {
                        Factory.Instance.iInfo(string.Format("{0} - GetInputData - Get value from ruleId.attributeId", rule.Id));
                        string[] ids = rule.inputRuleId.Split('.');
                        Db.RuleEntity refRule = RuleUtils.findRuleById(i.Rules, ids[0]);
                        res.inputValueO = RuleUtils.findRuleAttributesValueById(refRule, ids[1]);
                        res.inputType = typeof(Db.RuleEntity);
                    }
                    else
                    {
                        Factory.Instance.iInfo(string.Format("{0} - GetInputData - Get value from ruleId", rule.Id));
                        res.inputValue = (string)RuleUtils.findRuleValueById(i.Rules, rule.inputRuleId);
                        res.inputType = typeof(string);

                    }

                }

                if (!string.IsNullOrEmpty(res.inputValue))
                {
                    if (rule.InputData.PostProcessInputData.Count > 0)
                    {
                        Factory.Instance.iInfo(string.Format("{0} - Post process input data", rule.Id));
                        res.inputValue = res.inputValue.PostProcessString(rule.InputData.PostProcessInputData);
                    }
                }
            }
            return res;
        }*/
            
        
    }
}
