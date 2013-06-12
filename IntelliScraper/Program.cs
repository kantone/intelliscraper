using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace IntelliScraper
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fname = @"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\example.xml";
            Factory.Instance.Run(fname);
        }

        
    }
}
