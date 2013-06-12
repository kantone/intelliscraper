using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public static class ScrapeRuleDef
    {
        public const string getHtml = "httpGet";
        public const string xpathSingle = "xpath-single";
        public const string xpathLoop = "xpath-loop";

        public const string xpathLoopStepText = "text";
        public const string xpathLoopStepAttribute = "attribute";

    }
}
