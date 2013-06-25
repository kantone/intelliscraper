using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class LoopLink : IScrapeAction
    {
        Db.loop_link rule { get; set; }
        List<Model.ActionResult> actionResult { get; set; }      
        public LoopLink(Db.loop_link rule,List<Model.ActionResult> actionResult)
        {
            this.rule = rule;
            this.actionResult = actionResult;
        }
       

        public object Run(object input)
        {
            List<string> links = new List<string>();

            //Get all link from input attribute by input type
            if (input != null && rule.type == Db.loop_linkType.fromInputAttributes)            
                links.AddRange(getUrlFromInputByType(input));

            //Get all link from input attribute by input type
            if (rule.type == Db.loop_linkType.customUrl)
                links.AddRange(builLinkCustom());

            if (rule.type == Db.loop_linkType.fromCsv)
            {
                if (rule.csvInputDefinition != null)
                    links.AddRange(getUrlFromCsvFile());
            }

            if (rule.type == Db.loop_linkType.fromDatabase)
            {
                if(rule.databaseInputDefinition != null)
                    links.AddRange(getUrlFromDatabase());
            }

            //EXECUTE links
            if (rule.multiThreadOption != null)
            {
                if (rule.multiThreadOption.enableMultithread)
                    runLinkMultiThreaded(links);

                else runLinks(links);
            }
            else runLinks(links);
            
            return null;
        }


        /// <summary>
        /// Iterate page multithreaded 
        /// </summary>
        private void runLinkMultiThreaded(List<string> links)
        {
            ParallelOptions options = new ParallelOptions();
            if (rule.multiThreadOption.setThreadMaxNumbers)
                options.MaxDegreeOfParallelism = rule.multiThreadOption.ThreadNumbers;

            //Run links multithread
            System.Threading.Tasks.Parallel.For(0, links.Count, options, ii =>
            {
                //run also xpath MultiThread
                if (rule.multiXpathThreadOption != null)
                {
                    if (rule.multiXpathThreadOption.enableMultithread)
                    {                        
                        ParallelOptions options2 = new ParallelOptions();
                        if (rule.multiXpathThreadOption.setThreadMaxNumbers)
                            options.MaxDegreeOfParallelism = rule.multiXpathThreadOption.ThreadNumbers;

                        System.Threading.Tasks.Parallel.For(0, rule.xpathSingle.Count, options, xx =>
                        {
                            string html = HttpUtils.getHtml(links[ii], "", null, false);
                            XPathSingle x = new XPathSingle(rule.xpathSingle[xx]);
                            x.Run(html);
                        });
                    }
                    //no definition run only links threaded
                    else
                    {
                        foreach (Db.xpathSingle s in rule.xpathSingle)
                        {
                            string html = HttpUtils.getHtml(links[ii], "", null, false);
                            XPathSingle x = new XPathSingle(s);
                            x.Run(html);
                        }
                    }
                }
                //run only links threaded
                else
                {
                    foreach (Db.xpathSingle s in rule.xpathSingle)
                    {
                        string html = HttpUtils.getHtml(links[ii], "", null, false);
                        XPathSingle x = new XPathSingle(s);
                        x.Run(html);
                    }
                }
            });
        }

        /// <summary>
        /// Iterate pages 
        /// </summary>
        private void runLinks(List<string> links)
        {
            foreach (string link in links)
            {
                if (rule.xpathSingle != null)
                {
                    foreach (Db.xpathSingle s in rule.xpathSingle)
                    {
                       
                    }
                }
            }
        }

        /// <summary>
        /// Get Links from Database
        /// </summary>
        private List<string> getUrlFromDatabase()
        {
            List<string> links = new List<string>();
            Db.intelliScraperProjectStoreInfo storeDb = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == rule.csvInputDefinition.storeId select x).FirstOrDefault();

            if (storeDb != null)
            {
                string query = string.Empty;
                if (rule.databaseInputDefinition.type == Db.loop_linkDatabaseInputDefinitionType.query)
                {
                    query = rule.databaseInputDefinition.query;
                }
                if (rule.databaseInputDefinition.type == Db.loop_linkDatabaseInputDefinitionType.readAllTableColumn)
                {
                    query = string.Format("select {0} from {1}", rule.databaseInputDefinition.columnName, rule.databaseInputDefinition.tableName);
                }

                if (!string.IsNullOrEmpty(query))
                {
                    Database.DbManager db = new Database.DbManager(storeDb.DatabaseSetting.connection, storeDb.DatabaseSetting.providerName);
                    ICollection<KeyValuePair<string, object>> res = db.query(query);
                    foreach (KeyValuePair<string, object> r in res)
                    {
                        if (r.Key == rule.csvInputDefinition.columnName)
                            links.Add((string)r.Value);
                    }
                }
            }
            return links;
        }

        /// <summary>
        /// Get Links from Csv File
        /// </summary>
        private List<string> getUrlFromCsvFile()
        {
            List<string> links = new List<string>();
            //get store by id
            Db.intelliScraperProjectStoreInfo storeCsv = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == rule.csvInputDefinition.storeId select x).FirstOrDefault();
            System.IO.StreamReader rd = new System.IO.StreamReader(storeCsv.CsvExcelSetting.csvFileSaveTo);

            int i = 0;
            foreach (string h in storeCsv.CsvExcelSetting.csvHeader.Split(storeCsv.CsvExcelSetting.csvSeparator.ToCharArray()))
            {
                if (h == rule.csvInputDefinition.columnName)
                    break;
                i++;
            }

            while (rd.EndOfStream != true)
            {
                string line = rd.ReadLine();
                string[] cols = line.Split(storeCsv.CsvExcelSetting.csvSeparator.ToCharArray());
                links.Add(cols[i]);
            }
            return links;
        }

        /// <summary>
        /// Get links from input by input type
        /// </summary>
        private List<string> getUrlFromInputByType(object input)
        {
            List<string> links = new List<string>();

            //Get all link from input attribute by input type

            if (input.GetType() == typeof(List<KeyValuePair<string, object>>))
            {
                List<KeyValuePair<string, object>> data = (List<KeyValuePair<string, object>>)input;

                foreach (KeyValuePair<string, object> d in data)
                {
                    if (d.Key == rule.inputAttributeKey)
                        links.Add((string)d.Value);
                }
            }
            if (input.GetType() == typeof(List<List<KeyValuePair<string, object>>>))
            {
                List<List<KeyValuePair<string, object>>> dtList = (List<List<KeyValuePair<string, object>>>)input;

                foreach (List<KeyValuePair<string, object>> data in dtList)
                {
                    foreach (KeyValuePair<string, object> d in data)
                    {
                        if (d.Key == rule.inputAttributeKey)
                            links.Add((string)d.Value);
                    }
                }
            }

            return links;
        }

        /// <summary>
        /// Build custom links/pages
        /// </summary>
        private List<string> builLinkCustom()
        {
            List<string> links = new List<string>();

            int start = 0;
            int end = 0;

            //get start end                          
            if (!rule.StartEndData.getEndPageNumberFromRule)
                end = rule.StartEndData.end;
            else end = getStartEndPage(rule.StartEndData.actionAttributeId_getEndPage);


            if (!rule.StartEndData.getStartPageNumberFromRule)
                end = rule.StartEndData.start;
            else end = getStartEndPage(rule.StartEndData.actionAttributeId_getStartPage);


            //Run
            for (int ii = start; ii < end; ii++)
            {
                string fetchUrl = (string.Format(rule.StartEndData.url, ii));
                links.Add(fetchUrl);
            }
            return links;
        }

        /// <summary>
        /// Get start/end page custom 
        /// </summary>
        private int getStartEndPage(string _id)
        {
            if (!_id.Contains("."))
            {
                Model.ActionResult r = (from x in actionResult where x.action.id == _id select x).SingleOrDefault();
                if (r.GetType() == typeof(string))
                {
                    return Int32.Parse((string)r.result);
                }
            }
            else
            {
                string[] ids = _id.Split('.');
                Model.ActionResult r = (from x in actionResult where x.action.id == ids[0] select x).SingleOrDefault();
                if (r != null)
                {
                    if (r.GetType() == typeof(string))
                    {
                        return Int32.Parse((string)r.result);
                    }

                    if (r.GetType() == typeof(List<KeyValuePair<string, object>>))
                    {
                        try
                        {
                            List<KeyValuePair<string, object>> vals = (List<KeyValuePair<string, object>>)r.result;
                            var tmp = (from x in vals where x.Key == rule.inputAttributeKey select x).FirstOrDefault();
                            return Int32.Parse((string)tmp.Value);
                        }
                        catch (Exception ex)
                        {
                            Factory.Instance.log.Error(ex);
                        }
                    }

                    if (r.GetType() == typeof(List<List<KeyValuePair<string, object>>>))
                    {
                        try
                        {
                            List<List<KeyValuePair<string, object>>> _vals = (List<List<KeyValuePair<string, object>>>)r.result;
                            foreach (List<KeyValuePair<string, object>> vals in _vals)
                            {
                                var tmp = (from x in vals where x.Key == rule.inputAttributeKey select x).FirstOrDefault();
                                return Int32.Parse((string)tmp.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            Factory.Instance.log.Error(ex);
                        }
                    }
                }
            }
            return -1;

        }

      
    }
}
