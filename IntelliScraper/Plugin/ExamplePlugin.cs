using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Plugin
{
    public class ExamplePlugin 
    {
        public object Run(object input)
        {
            try
            {
                System.IO.File.WriteAllText("c:\\test.txt", "test");
            }
            catch (Exception ex)
            {
                Factory.Instance.log.Error(ex);
            }
            return null;
        }
        
    }
}
