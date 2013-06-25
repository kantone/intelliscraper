using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace IntelliScraper.Plugin
{
    public interface IScraperPlugin
    {
        string Name { get; }
        object Run(Data.intelliScraperAction actionInfo,object inputData);
        WebClient setClient();
    }
}
