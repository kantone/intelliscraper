using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IntelliScraper.Scrape
{
    public class Scraper
    {
        private bool getHtmlAsync = false;
        public Model.Stats stat { get; set; }
        public List<Model.ActionResult> actionResult { get; set; }

        /// <summary>
        /// Run scraping actions
        /// </summary>
        public void Run(IntelliScraper.Db.intelliScraper i)
        {
            this.actionResult = new List<Model.ActionResult>();
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

            if (action.type == Db.intelliScraperActionType.loop_link)
            {
                //find httpGet rule                 
                Db.loop_link post = (from x in i.rules.loop_link where x.id == action.ruleId select x).FirstOrDefault();
                if (post != null)
                {
                    return new Action.LoopLink(post, actionResult);
                }
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
