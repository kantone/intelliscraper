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

        public string getName()
        {
            return "LoopLink";
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
                   return runLinkMultiThreaded(links);
                else return runLinks(links);
            }
            else return runLinks(links);

        }


        /// <summary>
        /// Iterate page multithreaded 
        /// </summary>
        private List<Model.LoopLinkResult> runLinkMultiThreaded(List<string> links)
        {
            List<Model.LoopLinkResult> res = new List<Model.LoopLinkResult>();

            ParallelOptions options = new ParallelOptions();
            if (rule.multiThreadOption.setThreadMaxNumbers)
                options.MaxDegreeOfParallelism = rule.multiThreadOption.ThreadNumbers;

            //Run links multithread
            System.Threading.Tasks.Parallel.For(0, links.Count, options, ii =>
            {
                string html = HttpUtils.getHtml(links[ii], "", null, true);
                if (rule.xpathSingle != null)
                    res.Add(runXpath(html, links[ii]));

            });

            return res;
        }


        /// <summary>
        /// Run xpath Single or Collection
        /// </summary>
        private Model.LoopLinkResult runXpath(string html,string link)
        {
           
            if (!string.IsNullOrEmpty(html))
            {
                //run single
                if (rule.xpathSingle.Count > 0 && rule.xpathType == Db.loop_linkXpathType.single)
                {
                    XPathSingle x = new XPathSingle(rule.xpathSingle[0]);
                    List<KeyValuePair<string, object>> res = ( List<KeyValuePair<string, object>>)x.Run(html);
                    return new Model.LoopLinkResult(link,html,res,null);
                }
                //run collection
                if (rule.xpathType == Db.loop_linkXpathType.collection)
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlAgilityPack.HtmlNode node = doc.DocumentNode.SelectSingleNode(rule.preXpath);
                    if (node != null)
                    {
                        foreach (Db.xpathSingle s in rule.xpathSingle)
                        {
                            XPathSingle x = new XPathSingle(s);
                            List<List<KeyValuePair<string, object>>>  res = ( List<List<KeyValuePair<string, object>>> ) x.Run(node);
                            return new Model.LoopLinkResult(link, html, null, res);
                        }
                    }
                }
            }
            else 
            { 
                Factory.Instance.log.Error(string.Format("Html null or empty for rule {0} and link {1}", rule.id, link));
                return null;
            }
            return null;
        }

        /// <summary>
        /// Iterate pages 
        /// </summary>
        private List<Model.LoopLinkResult> runLinks(List<string> links)
        {
            List<Model.LoopLinkResult> res = new List<Model.LoopLinkResult>();
            foreach (string link in links)
            {
                string html = HttpUtils.getHtml(link, "", null, false);
                res.Add(runXpath(html, link));
            }
            return res;
        }

        /// <summary>
        /// Get Links from Database
        /// </summary>
        private List<string> getUrlFromDatabase()
        {
            List<string> links = new List<string>();
            Db.intelliScraperProjectStoreInfo storeDb = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == rule.databaseInputDefinition.storeId select x).FirstOrDefault();

            if (storeDb != null)
            {
                string query = string.Empty;
                if (rule.databaseInputDefinition.type == Db.loop_linkDatabaseInputDefinitionType.query)
                {
                    query = rule.databaseInputDefinition.query;
                }
                if (rule.databaseInputDefinition.type == Db.loop_linkDatabaseInputDefinitionType.readAllTableColumn)
                {
                    query = string.Format("select * from {1}", rule.databaseInputDefinition.columnName, rule.databaseInputDefinition.tableName);
                }

                if (!string.IsNullOrEmpty(query))
                {
                    Database.DbManager db = new Database.DbManager(storeDb.DatabaseSetting.connection, storeDb.DatabaseSetting.providerName);
                    //ICollection<KeyValuePair<string, object>> res = db.query(query);
                    IEnumerable<object> obj =    db.db.Query<object>(query, null);
                    foreach (object o in obj)
                    {
                         IDictionary<string, object> rows = ( IDictionary<string, object> )o;
                         foreach (KeyValuePair<string,object> row in rows)
                         {
                             if (row.Key == rule.databaseInputDefinition.columnName)
                             {
                                 links.Add((string)row.Value);
                             }
                         }
                       
                       
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
                    {
                        links.Add((string)d.Value);
                        break;
                    }
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
                        {
                            links.Add((string)d.Value);
                            break;
                        }
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
            end = end + rule.StartEndData.appendEndVal;

            if (!rule.StartEndData.getStartPageNumberFromRule)
                start = rule.StartEndData.start;
            else start = getStartEndPage(rule.StartEndData.actionAttributeId_getStartPage);

            start = start + rule.StartEndData.appendStartVal;


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
            Model.ActionResult r = null;
            if (!_id.Contains("."))
                r = (from x in actionResult where x.action.id == _id select x).SingleOrDefault();
            else
            {
                string[] ids = _id.Split('.');
                r = (from x in actionResult where x.action.id == ids[0] select x).SingleOrDefault();
            }

            if (r != null)
            {
                if (r.result.GetType() == typeof(string))
                {
                    return Int32.Parse((string)r.result);
                }

                if (r.result.GetType() == typeof(List<KeyValuePair<string, object>>))
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

                if (r.result.GetType() == typeof(List<List<KeyValuePair<string, object>>>))
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


            return -1;

        }

      
    }
}
