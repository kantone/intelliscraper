using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Plugin
{
    public interface IPlugin
    {
        object Run(object input);
        string getName();
    }
}
