using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class linksPageGenerator
    {

        public Db.linksPageGenerator rule { get; set; }
        public linksPageGenerator(Db.linksPageGenerator rule)
        {
            this.rule = rule;

        }

        public List<string> Run(string customUrlFormat, List<KeyValuePair<string, object>> input,string attributeName)
        {
            return Limit(getUrlFromInputByType(input, attributeName));
        }

        public List<string> Run(string customUrlFormat, List<List<KeyValuePair<string, object>>> input, string attributeName)
        {
            return Limit(getUrlFromInputByType(input, attributeName));
        }
       
        public List<string> Run(string customUrlFormat,int start,int end)
        {
             List<string> links = new List<string>();

             try
             {
                 for (int i = start; i < end; i++)
                 {
                     links.Add(string.Format(customUrlFormat, end.ToString()));
                 }
             }
             catch { }

             return Limit(links);
        }
        
        /// <summary>
        /// Get input from databases or files
        /// </summary>
        /// <returns></returns>
        public List<string> Run()
        {
            List<string> links = new List<string>();
                    


            //Input from csv file
            if (rule.type == Db.linksPageGeneratorType.fromCsv)
            {
                if (rule.csvInputDefinition != null)
                    links.AddRange(getUrlFromCsvFile());
            }

            //Input from excel file
            if (rule.type == Db.linksPageGeneratorType.fromExcel)
            {
                if (rule.excelInputDefinition != null)
                    links.AddRange(getUrlFromExcelFile());
            }

            //Input from database
            if (rule.type == Db.linksPageGeneratorType.fromDatabase)
            {
                if(rule.databaseInputDefinition != null)
                    links.AddRange(getUrlFromDatabase());
            }






            return Limit(links);
        }

        private List<string> Limit(List<string> links)
        {
            //limit links
            if (rule.enableLimit)
            {
                links = links.Take(rule.limit).ToList();
            }
            return links;
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
                if (rule.databaseInputDefinition.type == Db.linksPageGeneratorDatabaseInputDefinitionType.query)
                {
                    query = rule.databaseInputDefinition.query;
                }
                if (rule.databaseInputDefinition.type == Db.linksPageGeneratorDatabaseInputDefinitionType.readAllTableColumn)
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
        /// Get Links from excel File
        /// </summary>
        private List<string> getUrlFromExcelFile()
        {
            List<string> links = new List<string>();
            //get store by id
            Db.intelliScraperProjectStoreInfo storeInfo = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == rule.excelInputDefinition.storeId select x).FirstOrDefault();
            FileInfo newFile = new FileInfo(storeInfo.CsvExcelSetting.csvFileSaveTo);
            ExcelPackage book = new ExcelPackage(newFile);

            for (int i = 1; i < book.Workbook.Worksheets[rule.excelInputDefinition.sheetName].Dimension.End.Row + 1; i++)
            {
                links.Add((string)book.Workbook.Worksheets[rule.excelInputDefinition.sheetName].Cells[i, rule.excelInputDefinition.columnIndex].Value);
            }

            return links;
        }

        /// <summary>
        /// Get links from input by input type
        /// </summary>
        private List<string> getUrlFromInputByType(object input,string attributeName)
        {
            List<string> links = new List<string>();

            //Get all link from input attribute by input type

            if (input.GetType() == typeof(List<KeyValuePair<string, object>>))
            {
                List<KeyValuePair<string, object>> data = (List<KeyValuePair<string, object>>)input;

                foreach (KeyValuePair<string, object> d in data)
                {
                    if (d.Key == attributeName)
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
                        if (d.Key == attributeName)
                        {
                            links.Add((string)d.Value);
                            break;
                        }
                    }
                }
            }

            return links;
        }

    
       
      
    }
}
