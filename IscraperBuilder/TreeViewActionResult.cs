using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IscraperBuilder.Controls.Rules;

namespace IscraperBuilder
{
    public class TreeViewActionResult
    {
        public TreeviewH treeView { get; set; }
        public IntelliScraper.Db.intelliScraperAction action { get; set; }
    }
}
