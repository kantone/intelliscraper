using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IntelliScraper.Scrape
{
    public class Scraper
    {
        //private bool getHtmlAsync = false;
        public Model.Stats stat { get; set; }
        public List<Model.ActionResult> actionResult { get; set; }

        /// <summary>
        /// Run scraping actions
        /// </summary>
        public void Run(IntelliScraper.Db.intelliScraper i)
        {
            this.actionResult = new List<Model.ActionResult>();
            this.stat = new Model.Stats();
            stat.startProcess = DateTime.Now;
            foreach(Db.intelliScraperAction action in  i.actions)
            {
                object input = getInput(i, action);
                IScrapeAction act = getScraperAction(input, action, i);
                if (act != null)
                {
                    object result = act.Run(input);
                    actionResult.Add(new Model.ActionResult(action, result, result.GetType()));
                }
                else
                {
                    Factory.Instance.log.Error(string.Format("Cannot find Action/Rule to run from action {0}",action.id));
                    Environment.Exit(8);
                }
            }
            stat.endProcess = DateTime.Now;
            TimeSpan span = stat.endProcess - stat.startProcess;
            Factory.Instance.iInfo(string.Format("Elapsed time : {0} days, {1} hours, {2} minutes, {3} seconds, {4} milliseconds", span.TotalDays, span.TotalHours, span.TotalMinutes, span.TotalSeconds, span.TotalMilliseconds));
     
        }

        /// <summary>
        /// Get type to execute
        /// </summary>
        private IScrapeAction getScraperAction(object input, Db.intelliScraperAction action, IntelliScraper.Db.intelliScraper i)
        {
            //Html From File
            if (action.type == Db.intelliScraperActionType.loadHtmlFromFile)
            {
                return new Action.HtmlFromFile();
            }

            //HttpGet
            if (action.type == Db.intelliScraperActionType.httpGet)
            {
                //find httpGet rule                 
                Db.httpGet get = (from x in i.rules.httpGet where x.id == action.ruleId select x).FirstOrDefault();
                if (get != null)
                {
                    return new Action.HttpGet(get, false);                
                }
            }

            //Xpath Single
            if (action.type == Db.intelliScraperActionType.xpathSingle)
            {
                Db.xpathSingle val =  (from x in i.rules.xpathSingle where x.id == action.ruleId select x).FirstOrDefault();
                if (val != null)
                {
                    return new Action.XPathSingle(val);
                }                
            }

            //Xpath collection
            if (action.type == Db.intelliScraperActionType.xpathCollection)
            {
                Db.xpathCollection val = (from x in i.rules.xpathCollection where x.id == action.ruleId select x).FirstOrDefault();
                if (val != null)
                {
                    return new Action.XPathCollection(val);
                }  
            }

            //HttpPost
            if (action.type == Db.intelliScraperActionType.httpPost)
            {
                //find httpGet rule                 
                Db.httpPost post = (from x in i.rules.httpPost where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                {
                    return new Action.HttpPost(post, false);
                }
            }

            //LoopLink
            if (action.type == Db.intelliScraperActionType.loop_link)
            {
                //find httpGet rule                 
                Db.loop_link post = (from x in i.rules.loop_link where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                {
                    return new Action.LoopLink(post, actionResult);
                }
            }

            //downlaod
            if (action.type == Db.intelliScraperActionType.download)
            {
                //find httpGet rule                 
                Db.download post = (from x in i.rules.download where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)                
                    return new Action.Download(post);
                
            }

            //ftp Put
            if (action.type == Db.intelliScraperActionType.ftpPut)
            {
                //find httpGet rule                 
                Db.ftpPut post = (from x in i.rules.ftpPut where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.FtpPut(post);
            }

            //Zip
            if (action.type == Db.intelliScraperActionType.zip)
            {
                //find httpGet rule                 
                Db.actionZip post = (from x in i.rules.actionZip where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.Zip(post);
            }

            //ScreenShot
            if (action.type == Db.intelliScraperActionType.screenShot)
            {
                //find httpGet rule                 
                Db.screenShot post = (from x in i.rules.screenShot where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.ScreenShot(post);
            }

            //Plugin
            if (action.type == Db.intelliScraperActionType.plugin)
            {
                //find httpGet rule                 
                Db.plugin post = (from x in i.rules.plugin where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.Plugin(post);
            }

            //Upload
            if (action.type == Db.intelliScraperActionType.upload)
            {
                //find httpGet rule                 
                Db.upload post = (from x in i.rules.upload where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.Upload(post);
            }

            //Save
            if (action.type == Db.intelliScraperActionType.save)
            {
                //find httpGet rule                 
                Db.save post = (from x in i.rules.save where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                    return new Action.Save(post, actionResult);
            }

            return null;
        }

        /// <summary>
        /// Get input object (custom or from previus action )
        /// </summary>
        private object getInput(IntelliScraper.Db.intelliScraper i,Db.intelliScraperAction action)
        {
            object input = null;
            if (action.input.type == Db.intelliScraperActionInputType.custom)
                input = action.input.customInputValue;

            if (action.input.type == Db.intelliScraperActionInputType.fromAction)
            {
                Model.ActionResult r = (from x in actionResult where x.action.id == action.input.actionId select x).FirstOrDefault();
                if (r != null)
                    input = r.result;
            }
            return input;
        }
    }
}
