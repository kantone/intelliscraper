using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape.Action
{
   
    public class HtmlFromFile 
    {
        /// <summary>
        /// Load html from file
        /// </summary>
        /// <param name="fname"></param>
        /// <returns>Html file content</returns>
        public string Run(string fname)
        {
            if (!string.IsNullOrEmpty(fname))
            {
                if (System.IO.File.Exists(fname))
                {
                    return System.IO.File.ReadAllText(fname);
                }
            }
            return null;
        }
    }
}
