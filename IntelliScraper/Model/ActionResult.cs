using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Model
{
    public class ActionResult
    {
        public Db.intelliScraperAction action { get; set; }
        public object result{ get; set; }
        public Type resulType { get; set; }
        public ActionResult(Db.intelliScraperAction action, object result, Type resulType)
        {
            this.action = action;
            this.result = result;
            this.resulType = resulType;
        }
    }
}
