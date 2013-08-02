using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntelliScraper;

namespace IntelliScraperTest
{
    public class CodeGen
    {
        /// <summary>
        /// Get all rule id
        /// </summary>
        public static List<string> getRuleIds()
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

                if (Factory.Instance.i.rules.linksPageGenerator != null)
                    ids.AddRange(getAllRulesId(Factory.Instance.i.rules.linksPageGenerator.ToList<object>()));

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
        /// Get id from rule with reflection
        /// </summary>
        public static List<string> getAllRulesId(List<object> coll)
        {
            List<string> ids = new List<string>();
            foreach (var x in coll)
            {
                string id = (string)x.GetType().GetProperty("id").GetValue(x, null);
                ids.Add(id);
            }
            return ids;
        }
    }
}
