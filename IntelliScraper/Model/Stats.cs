using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Model
{
    public class Stats
    {
        public DateTime startProcess;
        public DateTime endProcess;
        public int PercentProcessedRules = 0;
        public int PercentProcessedPages = 0;
        public int TotalProcessedPages = 0;
    }
}
