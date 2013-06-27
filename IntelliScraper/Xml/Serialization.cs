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
        public static IntelliScraper.Db.intelliScraper Serialize(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IntelliScraper.Db.intelliScraper));
            IntelliScraper.Db.intelliScraper i;
            using (TextReader reader = new StreamReader(file))
            {
                i = (IntelliScraper.Db.intelliScraper)serializer.Deserialize(reader);
            }
            return i;
        }

        /// <summary>
        /// Serialize string xml
        /// </summary>
        /// <returns></returns>
        public static IntelliScraper.Db.intelliScraper SerializeFromString(string xmlText)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IntelliScraper.Db.intelliScraper));
            IntelliScraper.Db.intelliScraper i;
            using (TextReader reader = new StringReader(xmlText))
            {
                i = (IntelliScraper.Db.intelliScraper)serializer.Deserialize(reader);
            }
            return i;
        }
    }
}
