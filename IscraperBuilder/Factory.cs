using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IscraperBuilder
{
    public class Factory
    {  //SIngleTon
        private static Factory instance;
        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }

        public IntelliScraper.Db.intelliScraper i { get; set; }
        public string openedFileProject { get; set; }

        /// <summary>
        /// Save project
        /// </summary>
        public void Save()
        {            
            System.IO.File.Delete(Factory.Instance.openedFileProject);
            System.IO.FileStream s = System.IO.File.Create(Factory.Instance.openedFileProject);
            s.Close();
            IntelliScraper.Xml.Serialization.DeSerialize(Factory.Instance.i, Factory.Instance.openedFileProject);
            this.i = IntelliScraper.Xml.Serialization.Serialize(openedFileProject);
        }
    }
}
