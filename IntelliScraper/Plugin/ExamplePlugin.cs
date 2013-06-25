using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Plugin
{
    public class ExamplePlugin : IScraperPlugin
    {
        public string Name
        {
            get { return "test1"; }
        }

        public System.Net.WebClient setClient()
        {
            return null;
        }

        object IScraperPlugin.Run(Data.intelliScraperAction actionInfo, object inputData)
        {            
            return null;
        }
    }
}
