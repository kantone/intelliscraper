using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public interface IScrapeAction
    {
        object Run(object input);
    }
}
