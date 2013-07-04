using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IscraperBuilder.Controls.Rules.Rule
{
    public interface IRule
    {
         string id { get; set; }
         bool isNew { get; set; }
         //IRule(string id);
         string save();
    }
}
