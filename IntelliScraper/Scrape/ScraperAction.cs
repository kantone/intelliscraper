using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntelliScraper.Scrape
{
    public static class ScraperAction
    {
        /// <summary>
        /// Get html
        /// </summary>
        public static string getHtml(string url,Data.httpHeadersCollection customHttpHeaders)
        {           
            try{

            //set new proxy if use proxy
            Factory.Instance.setWebClientNext();

            //add HttpHeader
            if (customHttpHeaders != null)
            {
                foreach (Data.httpHeaders h in customHttpHeaders)
                    Factory.Instance.addHttpHeaderToClient(h);
            }            

            //Set Wait time
            System.Threading.Thread.Sleep(Factory.Instance.i.settings.waitEachRequestMilliseconds);
            
            Stream s = Factory.Instance.client.OpenRead(url); ;
            StreamReader sr = new StreamReader(s);
            string html = sr.ReadToEnd();
            s.Close();
            return html;
            }
            catch(Exception ex)
            {
                //Wait after error - prevent next request
                System.Threading.Thread.Sleep(Factory.Instance.i.settings.waitAfterError);
                Factory.Instance.log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Download image
        /// </summary>
        public static bool downloadImage(string imgUrl,string saveTo)
        {
            using (Factory.Instance.client)
            {
                try
                {
                    //set new proxy if use proxy
                    Factory.Instance.setWebClientNext();

                    //download image
                    Factory.Instance.client.DownloadFile(imgUrl, saveTo);
                    return true;
                }
                catch (Exception ex)
                {
                    Factory.Instance.log.Error(ex);
                    return false;
                }
            }
        }

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
    }
}
