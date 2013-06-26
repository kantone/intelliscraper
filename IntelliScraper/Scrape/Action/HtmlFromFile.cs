using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape.Action
{
    /// <summary>
    /// Perform httpGet
    /// </summary>
    public class HtmlFromFile : IScrapeAction
    {
        
        public object Run(object input)
        {
            if (input != null)
            {
                string fname = (string)input;
                if (System.IO.File.Exists(fname))
                {
                    return System.IO.File.ReadAllText(fname);
                }
            }
            return null;
        }

       

        public string getName()
        {
            return "HtmlFromFile";
        }

        
    }
}
