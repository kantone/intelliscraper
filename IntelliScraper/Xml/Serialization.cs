using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace IntelliScraper.Xml
{
    public static class Serialization
    {
        /// <summary>
        /// Serialize input xml file
        /// </summary>
        /// <returns></returns>
        public static IntelliScraper.Data.intelliScraper Serialize(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IntelliScraper.Data.intelliScraper));
            IntelliScraper.Data.intelliScraper i;
            using (TextReader reader = new StreamReader(file))
            {
                i = (IntelliScraper.Data.intelliScraper)serializer.Deserialize(reader);
            }
            return i;
        }
    }
}
