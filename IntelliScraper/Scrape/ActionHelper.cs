using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntelliScraper.Scrape.Action;

namespace IntelliScraper.Scrape
{
    public class ActionHelper
    {
        public static object getRuleObject(string name, IntelliScraper.Db.intelliScraperActionType type)
        {
            List<object> o = new List<object>();
            if (type == IntelliScraper.Db.intelliScraperActionType.httpGet)
                o = IntelliScraper.Factory.Instance.i.rules.httpGet.ToList<object>();

            if (o != null)
                return (from x in o where (string)x.GetType().GetProperty("id").GetValue(x, null) == name select x).FirstOrDefault();
            
            return null;
        }

        public static HttpGet getHttpGetByName(string name)
        {
            object o = getRuleObject(name, Db.intelliScraperActionType.httpGet);
            return new HttpGet((IntelliScraper.Db.httpGet)o);
        }
       
    }
}
