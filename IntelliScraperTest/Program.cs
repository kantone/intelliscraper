using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntelliScraper;
using IntelliScraper.Scrape;

namespace IntelliScraperTest
{
    public class iData
    {
        public string lastHtml = string.Empty;
        public List<List<KeyValuePair<string, object>>> lastData = new List<List<KeyValuePair<string, object>>>();
        public List<string> urls = new List<string>();
    }

    class Program
    {
      
        static void Main(string[] args)
        {
            bool async = false;
            iData idata = new iData();
            string prj = @"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IscraperBuilder\bin\Debug\projects\VacationHomes.xml";
            IntelliScraper.Factory.Instance.Load(prj, false, "");// = IntelliScraper.Factory.Instance.get(prj, false, "");

            //get page1
           /* IntelliScraper.Scrape.Action.HttpGet h = IntelliScraper.Scrape.ActionHelper.getHttpGetByName(Rules.RULE_1_httpGet);
            string html = h.Run(null, false);*/


            List<string> rules = new List<string>();
            rules.Add("1_httpGet|!");
            rules.Add("2 xpathCollection|!");
            rules.Add("3_getStateEndPage|StateLink|");
            rules.Add("4_loop_link_statePages||a");

            foreach (string _rule in rules)
            {
                List<string> ruleD = new List<string>(_rule.Split('|'));
                string rule = ruleD[0];
                string searchKey = ruleD[1];
                string type = ruleD[2];
                

                System.Reflection.PropertyInfo[] pinfo =  IntelliScraper.Factory.Instance.i.rules.GetType().GetProperties();
                for(int i=0;i<pinfo.Length;i++)
                {              
                    //HttpGet   
                    if (pinfo[i].PropertyType == typeof(IntelliScraper.Db.httpGetCollection))
                    {
                        object val = checkId(pinfo[i], rule);
                        if (val != null)
                        {
                            IntelliScraper.Scrape.Action.HttpGet h = new IntelliScraper.Scrape.Action.HttpGet((IntelliScraper.Db.httpGet)val);
                            idata.lastHtml = h.Run(null, false);
                        }
                    }

                    //XpathCollection 
                    if (pinfo[i].PropertyType == typeof(IntelliScraper.Db.xpathCollectionCollection))
                    {
                        object val = checkId(pinfo[i], rule);
                        if (val != null)
                        {
                            IntelliScraper.Scrape.Action.XPathCollection h = new IntelliScraper.Scrape.Action.XPathCollection((IntelliScraper.Db.xpathCollection)val);
                            if(!string.IsNullOrEmpty(idata.lastHtml))
                            {
                                idata.lastData = h.Run(idata.lastHtml);
                            }
                        }
                    }

                    //XpathSingle
                    if (pinfo[i].PropertyType == typeof(IntelliScraper.Db.xpathSingleCollection))
                    {
                        object val = checkId(pinfo[i], rule);
                        if (val != null)
                        {
                            idata.lastData = new List<List<KeyValuePair<string, object>>>();
                            IntelliScraper.Db.xpathSingle xps = (IntelliScraper.Db.xpathSingle)val;
                            foreach (List<KeyValuePair<string, object>> dt in idata.lastData)
                            {
                                foreach (KeyValuePair<string, object> k in dt)
                                {
                                    if (k.Key == searchKey)
                                    {
                                        string html = HttpUtils.getHtml((string)k.Value, "", null, async);
                                        IntelliScraper.Scrape.Action.XPathSingle h = new IntelliScraper.Scrape.Action.XPathSingle((IntelliScraper.Db.xpathSingle)val, dt);
                                        idata.lastData.Add(h.Run(html));
                                    }
                                }

                            }
                        }

                    }


                    //linksPageGenerator
                    if (pinfo[i].PropertyType == typeof(IntelliScraper.Db.linksPageGeneratorCollection))
                    {
                        object val = checkId(pinfo[i], rule);
                        if (val != null)
                        {
                            IntelliScraper.Scrape.Action.linksPageGenerator h = new IntelliScraper.Scrape.Action.linksPageGenerator((IntelliScraper.Db.linksPageGenerator)val);
                            if (type == "custom") ;
                                idata.urls = h.Run();

                              
                        }
                    }


                }               
            }
            
        }

        static object checkId(System.Reflection.PropertyInfo pinfo, string ruleId)
        {
            IEnumerable<object> data = (IEnumerable<object>)pinfo.GetValue(IntelliScraper.Factory.Instance.i.rules, null);
                       
            foreach (object ox in data)
            {
                string id = (string)ox.GetType().GetProperty("id").GetValue(ox, null);
                if (id == ruleId)
                {
                    return ox;
                }
            }
            return null;
        }

       
    }
}
