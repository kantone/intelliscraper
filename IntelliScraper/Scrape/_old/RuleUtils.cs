using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public class RuleUtils
    {

        /// <summary>
        /// Find value in rule or Rule Attribute
        /// </summary>
      /*  public static object findValueByRuleId(Db.RuleEntityCollection rules, string inputId)
        {
            if (inputId.Contains("."))
            {
                string[] ids = inputId.Split('.');
                Db.RuleEntity rule = findRuleById(rules,ids[0]);
                return findRuleAttributesValueById(rule, ids[1]);
            }
            else return findRuleValueById(rules,inputId);
        }

        /// <summary>
        /// Find Rule by Id
        /// </summary>
        public static Db.RuleEntity findRuleById(Db.RuleEntityCollection rules, string inputId)
        {
            var values = from x in rules where x.Id == inputId select x;
            foreach (var val in values)
                return val;
            return null;
        }

        /// <summary>
        /// Find Rule value
        /// </summary>
        public static object findRuleValueById(Db.RuleEntityCollection rules, string inputId)
        {
            var values = from x in rules where x.Id == inputId select x;
            foreach (var val in values)
                return val.FetchedData;
            return null;
        }

        /// <summary>
        /// Find attribute by Rule and attributeId
        /// </summary>
        public static Db.xpathEntity findRuleAttributesById(Db.RuleEntity rules, string attributeId)
        {
            var values = from x in rules.Attributes.attribute where x.id == attributeId select x;
            foreach (var val in values)
                return val;
            return null;
        }

        /// <summary>
        /// Find attribute by Rule and attributeId
        /// </summary>
        public static object findRuleAttributesValueById(Db.RuleEntity rules, string attributeId)
        {
            Db.xpathEntityCollection data = rules.Attributes.attribute;
          

            var values = from x in data where x.id == attributeId select x;
            foreach (var val in values)
                return val.fetchedData;
            return null;
        }*/

    }
}
