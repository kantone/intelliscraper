using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace IntelliScraper.Scrape
{
    public class ConverterUtils
    {
        
        /// <summary>
        /// Create rows to be stored into Cvs
        /// </summary>
        /// <param name="mappings">Mapping configuration</param>
        /// <param name="xpathCollection">All xpath action</param>
        /// <param name="cols">columns</param>
        /// <param name="ruleId">rule id</param>
        /// <param name="data">Fethed data by rule</param>
        /// <returns></returns>
        public static List<string[]> mapCsvToRows(Data.SaveEntityMapCollection mappings, Data.xpathEntityCollection xpathCollection, string[] cols, string ruleId, List<KeyValuePair<string, object>> data)
        {
            List<string[]> res = new List<string[]>();
                                    
            int count = 0;
            foreach (Data.xpathEntity _xpathEntity in xpathCollection)
            {
                string[] row = new string[cols.Length];  
                foreach (Data.SaveEntityMap map in mappings)
                {

                    //foreach (Data.SaveEntityMap map in r.Save.mapping)
                    foreach (Data.xpathEntity xpathEntity in xpathCollection)
                    {
                        string id = ruleId + "." + xpathEntity.id;
                        if (map.refId == id)
                        {
                            int pos = 0;
                            foreach (string col in cols)
                            {
                                if (col == map.mapColName)
                                {
                                    row[pos] = (string)data[count].Value;
                                }
                                pos++;
                            }
                        }
                    }
                    count++;
                }
               
                res.Add(row);         
            }
            return res;
        }


        /// <summary>
        /// Create rows to be stored into Sql Database
        /// </summary>
        /// <param name="mappings">Mapping configuration</param>
        /// <param name="xpathCollection">All xpath action</param>
        /// <param name="cols">columns</param>
        /// <param name="ruleId">rule id</param>
        /// <param name="data">Fethed data by rule</param>
        /// <returns></returns>
        public static List<List<KeyValuePair<string, object>>> mapTable(Data.SaveEntityMapCollection mappings, Data.xpathEntityCollection xpathCollection, string[] cols, string ruleId, List<KeyValuePair<string, object>> data)
        {
            List<List<KeyValuePair<string, object>>> res = new List<List<KeyValuePair<string, object>>>();

            int count = 0;
            foreach (Data.xpathEntity _xpathEntity in xpathCollection)
            {
                List<KeyValuePair<string, object>> row = new List<KeyValuePair<string, object>>();
                foreach (Data.SaveEntityMap map in mappings)
                {

                    //foreach (Data.SaveEntityMap map in r.Save.mapping)
                    foreach (Data.xpathEntity xpathEntity in xpathCollection)
                    {
                        string id = ruleId + "." + xpathEntity.id;
                        if (map.refId == id)
                        {
                            int pos = 0;
                            foreach (string col in cols)
                            {
                                if (col == map.mapColName)
                                {
                                    KeyValuePair<string,object> rw = new KeyValuePair<string,object>( map.mapColName,(string)data[count].Value);   
                                    row.Add(rw);
                                }
                                pos++;
                            }
                        }
                    }
                    count++;
                }

                res.Add(row);
            }
            return res;
        }
    }
}
