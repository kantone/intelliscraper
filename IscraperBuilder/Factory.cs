using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IscraperBuilder
{
    public class Factory
    {  
        //SIngleTon       
        private static Factory instance;
        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }

        public IntelliScraper.Db.intelliScraper i { get; set; }
        public string openedFileProject { get; set; }

        /// <summary>
        /// Save project
        /// </summary>
        public void Save()
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string caller = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;

            System.IO.File.Delete(Factory.Instance.openedFileProject);
            System.IO.FileStream s = System.IO.File.Create(Factory.Instance.openedFileProject);
            s.Close();
            System.Threading.Thread.Sleep(100);
            IntelliScraper.Xml.Serialization.DeSerialize(Factory.Instance.i, Factory.Instance.openedFileProject);
            this.i = IntelliScraper.Xml.Serialization.Serialize(openedFileProject);
            MainWindow.main.Status = string.Format("Saved ({0})",caller);
        }

       

        /// <summary>
        /// Get all rule id
        /// </summary>
        public List<string> getRuleIds()
        {
            List<string> ids = new List<string>();
            if (Factory.Instance.i.rules != null)
            {
                if (Factory.Instance.i.rules.actionZip != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.actionZip.ToList<object>()));

                if (Factory.Instance.i.rules.download != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.download.ToList<object>()));

                if (Factory.Instance.i.rules.ftpPut != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.ftpPut.ToList<object>()));

                if (Factory.Instance.i.rules.httpGet != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.httpGet.ToList<object>()));

                if (Factory.Instance.i.rules.httpPost != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.httpPost.ToList<object>()));

                if (Factory.Instance.i.rules.loop_link != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.loop_link.ToList<object>()));

                if (Factory.Instance.i.rules.plugin != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.plugin.ToList<object>()));

                if (Factory.Instance.i.rules.save != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.save.ToList<object>()));

                if (Factory.Instance.i.rules.screenShot != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.screenShot.ToList<object>()));

                if (Factory.Instance.i.rules.upload != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.upload.ToList<object>()));

                if (Factory.Instance.i.rules.xpathCollection != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.xpathCollection.ToList<object>()));

                if (Factory.Instance.i.rules.xpathSingle != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.xpathSingle.ToList<object>()));
            }
            return ids;
        }

        /// <summary>
        /// Get all Rules as objects
        /// </summary>        
        public List<object> getAllObj()
        {
            List<object> ids = new List<object>();
               if (Factory.Instance.i.rules.actionZip != null)
                ids.AddRange(Factory.Instance.i.rules.actionZip.ToList<object>());

            if (Factory.Instance.i.rules.download != null)
                ids.AddRange(Factory.Instance.i.rules.download.ToList<object>());

            if (Factory.Instance.i.rules.ftpPut != null)
                ids.AddRange(Factory.Instance.i.rules.ftpPut.ToList<object>());

            if (Factory.Instance.i.rules.httpGet != null)
                ids.AddRange(Factory.Instance.i.rules.httpGet.ToList<object>());

            if (Factory.Instance.i.rules.httpPost != null)
                ids.AddRange(Factory.Instance.i.rules.httpPost.ToList<object>());

            if (Factory.Instance.i.rules.loop_link != null)
                ids.AddRange(Factory.Instance.i.rules.loop_link.ToList<object>());

            if (Factory.Instance.i.rules.plugin != null)
                ids.AddRange(Factory.Instance.i.rules.plugin.ToList<object>());

            if (Factory.Instance.i.rules.save != null)
                ids.AddRange(Factory.Instance.i.rules.save.ToList<object>());

            if (Factory.Instance.i.rules.screenShot != null)
                ids.AddRange(Factory.Instance.i.rules.screenShot.ToList<object>());

            if (Factory.Instance.i.rules.upload != null)
                ids.AddRange(Factory.Instance.i.rules.upload.ToList<object>());

            if (Factory.Instance.i.rules.xpathCollection != null)
                ids.AddRange(Factory.Instance.i.rules.xpathCollection.ToList<object>());

            if (Factory.Instance.i.rules.xpathSingle != null)
                ids.AddRange(Factory.Instance.i.rules.xpathSingle.ToList<object>());

            return ids;
        }

        /// <summary>
        /// Get all Post Process as objects
        /// </summary>     
        public List<object> getAllPostProcessObj()
        {
            List<object> ids = new List<object>();
            if (Factory.Instance.i.postProcess != null)
            {
                if(Factory.Instance.i.postProcess.append != null)
                    ids.AddRange(Factory.Instance.i.postProcess.append.ToList<object>());

                if (Factory.Instance.i.postProcess.htmlEncodeDecode != null)
                    ids.AddRange(Factory.Instance.i.postProcess.htmlEncodeDecode.ToList<object>());

                if (Factory.Instance.i.postProcess.regularExpression != null)
                    ids.AddRange(Factory.Instance.i.postProcess.regularExpression.ToList<object>());

                if (Factory.Instance.i.postProcess.replace != null)
                    ids.AddRange(Factory.Instance.i.postProcess.replace.ToList<object>());

                if (Factory.Instance.i.postProcess.startEndWith != null)
                    ids.AddRange(Factory.Instance.i.postProcess.startEndWith.ToList<object>());

                if (Factory.Instance.i.postProcess.substring != null)
                    ids.AddRange(Factory.Instance.i.postProcess.substring.ToList<object>());

                if (Factory.Instance.i.postProcess.trim != null)
                    ids.AddRange(Factory.Instance.i.postProcess.trim.ToList<object>());                    
            }          

            
            return ids;
        }

        /// <summary>
        /// Get all Post Process as objects
        /// </summary>     
        public List<string> getAllPostProcessObjTypesName()
        {
            List<string> ids = new List<string>();
            if (Factory.Instance.i.postProcess != null)
            {
                if (Factory.Instance.i.postProcess.append != null)
                    ids.Add(Factory.Instance.i.postProcess.append.GetType().Name.Replace("Collection",""));

                if (Factory.Instance.i.postProcess.htmlEncodeDecode != null)
                    ids.Add(Factory.Instance.i.postProcess.htmlEncodeDecode.GetType().Name.Replace("Collection", ""));

                if (Factory.Instance.i.postProcess.regularExpression != null)
                    ids.Add(Factory.Instance.i.postProcess.regularExpression.GetType().Name.Replace("Collection", ""));

                if (Factory.Instance.i.postProcess.replace != null)
                    ids.Add(Factory.Instance.i.postProcess.replace.GetType().Name.Replace("Collection", ""));

                if (Factory.Instance.i.postProcess.startEndWith != null)
                    ids.Add(Factory.Instance.i.postProcess.startEndWith.GetType().Name.Replace("Collection", ""));

                if (Factory.Instance.i.postProcess.substring != null)
                    ids.Add(Factory.Instance.i.postProcess.substring.GetType().Name.Replace("Collection", ""));

                if (Factory.Instance.i.postProcess.trim != null)
                    ids.Add(Factory.Instance.i.postProcess.trim.GetType().Name.Replace("Collection", ""));
            }

            return ids;
        }

        /// <summary>
        /// Get id from rule with reflection
        /// </summary>
        public List<string> getAllRulesId(List<object> coll)
        {
            List<string> ids = new List<string>();
            foreach (var x in coll)
            {
                string id = (string)x.GetType().GetProperty("id").GetValue(x, null);
                ids.Add(id);
            }
            return ids;
        }
        
        /// <summary>
        /// Get id from rule with reflection
        /// </summary>
        public List<KeyValuePair<string,Type>> getAllRulesKeyVal(List<object> coll)
        {
            List<KeyValuePair<string,Type>> ids = new List<KeyValuePair<string,Type>>();
            foreach (var x in coll)
            {
                string id = (string)x.GetType().GetProperty("id").GetValue(x, null);
                ids.Add(new KeyValuePair<string, Type>(id, x.GetType()));
            }
            return ids;
        }

        /// <summary>
        /// Get rule by id
        /// </summary>
        public IscraperBuilder.Controls.Rules.Rule.IRule getRuleById(string ruleId, bool useCustomType, Type t)
        {
            IscraperBuilder.Controls.Rules.Rule.IRule h = null;

            //get Type
            Type type = null;
            if (useCustomType)
                type = t;
            else
            {
                if (!string.IsNullOrEmpty(ruleId))
                {
                    List<KeyValuePair<string, Type>> ids = Factory.Instance.getAllRulesKeyVal(Factory.Instance.getAllObj());
                    KeyValuePair<string, Type> val = (from x in ids where x.Key == ruleId select x).FirstOrDefault();
                    type = val.Value;
                }
            }
             //Load frame by type
            if (type != null)
            {
                if (type == typeof(IntelliScraper.Scrape.Action.HttpGet) || type == typeof(IntelliScraper.Db.httpGet))
                    h = new IscraperBuilder.Controls.Rules.RuleHttpGet(ruleId);

                if (type == typeof(IntelliScraper.Db.httpPost))
                    h = new IscraperBuilder.Controls.Rules.Rule.RuleHttpPost(ruleId);    

                if (type == typeof(IntelliScraper.Db.xpathSingle))
                    h = new IscraperBuilder.Controls.Rules.Rule.RulexpathSingle(ruleId);

                if (type == typeof(IntelliScraper.Db.xpathCollection))
                    h = new IscraperBuilder.Controls.Rules.Rule.xpathCollection(ruleId);

                if (type == typeof(IntelliScraper.Db.loop_link))
                    h = new IscraperBuilder.Controls.Rules.Rule.RuleLoopLink(ruleId);

                if (type == typeof(IntelliScraper.Db.download))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleDownload(ruleId);

                if (type == typeof(IntelliScraper.Scrape.Action.Zip) || type == typeof(IntelliScraper.Db.actionZip))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleZip(ruleId);

                if (type == typeof(IntelliScraper.Db.screenShot))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleScreenShot(ruleId);

                if (type == typeof(IntelliScraper.Db.upload))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleUpload(ruleId);

                if (type == typeof(IntelliScraper.Db.ftpPut))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleFtpPut(ruleId);
                
                if (type == typeof(IntelliScraper.Db.plugin))
                    h = new IscraperBuilder.Controls.Rules.Rule.rulePlugin(ruleId);  
        
                if (type == typeof(IntelliScraper.Db.save))
                    h = new IscraperBuilder.Controls.Rules.Rule.ruleSave(ruleId); 
           
            }
            return h;

        }

        /// <summary>
        /// Load rule Frame by Rule Id
        /// </summary>
        public IscraperBuilder.Controls.Rules.Rule.IRule LoadRuleFrame(string ruleId, System.Windows.Controls.Frame f, bool useCustomType, Type t)
        {
            IscraperBuilder.Controls.Rules.Rule.IRule h = getRuleById(ruleId, useCustomType, t);
            if (h != null)
            {
                f.Navigate(h);
                f.Visibility = System.Windows.Visibility.Visible;
                return h;
            }
            return null;

        }
        
        /// <summary>
        /// Get rule Type by typeName
        /// </summary>
        public Type GetRuleTypeByName(string name)
        {          

            //if (name == "loadHtmlFromFile")
                
            if (name == "httpGet")
                return typeof(IntelliScraper.Scrape.Action.HttpGet);

            if (name == "httpPost")
                return typeof(IntelliScraper.Scrape.Action.HttpPost);

            if (name == "xpathSingle")
                return typeof(IntelliScraper.Scrape.Action.XPathSingle);

            if (name == "xpathCollection")
                return typeof(IntelliScraper.Scrape.Action.XPathCollection);

            if (name == "loop_link")
                return typeof(IntelliScraper.Scrape.Action.LoopLink);

            if (name == "download")
                return typeof(IntelliScraper.Scrape.Action.Download);

            if (name == "upload")
                return typeof(IntelliScraper.Scrape.Action.Upload);

            if (name == "ftpPut")
                return typeof(IntelliScraper.Scrape.Action.FtpPut);

            if (name == "zip")
                return typeof(IntelliScraper.Scrape.Action.Zip);

            if (name == "save")
                return typeof(IntelliScraper.Scrape.Action.Save);

            if (name == "screenShot")
                return typeof(IntelliScraper.Scrape.Action.ScreenShot);

            if (name == "plugin")
                return typeof(IntelliScraper.Scrape.Action.Plugin);


            return null;

        }
               
        /// <summary>
        /// delete Rule By Type And Index
        /// </summary>
        public void deleteRuleByTypeAndIndex(string name)
        {
            foreach (object o in getAllObj())
            {
                string id = (string) o.GetType().GetProperty("id").GetValue(o, null);

                if (id == name)
                {
                    if (o.GetType() == typeof(IntelliScraper.Db.actionZip))
                    {
                        if (Factory.instance.i.rules.actionZip != null)
                            Factory.instance.i.rules.actionZip.Remove((IntelliScraper.Db.actionZip)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.httpGet))
                    {
                        if (Factory.instance.i.rules.httpGet != null)
                            Factory.instance.i.rules.httpGet.Remove((IntelliScraper.Db.httpGet)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.httpPost))
                    {
                        if (Factory.instance.i.rules.httpPost != null)
                            Factory.instance.i.rules.httpPost.Remove((IntelliScraper.Db.httpPost)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.xpathSingle))
                    {
                        if (Factory.instance.i.rules.xpathSingle != null)
                            Factory.instance.i.rules.xpathSingle.Remove((IntelliScraper.Db.xpathSingle)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.xpathCollection))
                    {
                        if (Factory.instance.i.rules.xpathCollection != null)
                            Factory.instance.i.rules.xpathCollection.Remove((IntelliScraper.Db.xpathCollection)o);
                    }


                    if (o.GetType() == typeof(IntelliScraper.Db.loop_link))
                    {
                        if (Factory.instance.i.rules.loop_link != null)
                            Factory.instance.i.rules.loop_link.Remove((IntelliScraper.Db.loop_link)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.download))
                    {
                        if (Factory.instance.i.rules.download != null)
                            Factory.instance.i.rules.download.Remove((IntelliScraper.Db.download)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.upload))
                    {
                        if (Factory.instance.i.rules.upload != null)
                            Factory.instance.i.rules.upload.Remove((IntelliScraper.Db.upload)o);
                    }
                    if (o.GetType() == typeof(IntelliScraper.Db.ftpPut))
                    {
                        if (Factory.instance.i.rules.ftpPut != null)
                            Factory.instance.i.rules.ftpPut.Remove((IntelliScraper.Db.ftpPut)o);
                    }
                    if (o.GetType() == typeof(IntelliScraper.Db.save))
                    {
                        if (Factory.instance.i.rules.save != null)
                            Factory.instance.i.rules.save.Remove((IntelliScraper.Db.save)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.screenShot))
                    {
                        if (Factory.instance.i.rules.screenShot != null)
                            Factory.instance.i.rules.screenShot.Remove((IntelliScraper.Db.screenShot)o);
                    }

                    if (o.GetType() == typeof(IntelliScraper.Db.plugin))
                    {
                        if (Factory.instance.i.rules.plugin != null)
                            Factory.instance.i.rules.plugin.Remove((IntelliScraper.Db.plugin)o);
                    }

                }
            }
        }
    }
}
