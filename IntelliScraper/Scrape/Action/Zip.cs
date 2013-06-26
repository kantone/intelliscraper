using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Packaging;

namespace IntelliScraper.Scrape.Action
{
    public class Zip : IScrapeAction
    {
      
        Db.actionZip d { get; set; }
        public Zip(Db.actionZip d)
        {
            this.d = d;
        }


        public string getName()
        {
            return "Zip";
        }

        public object Run(object input)
        {
            if (d != null)
            {
                List<string> files = new List<string>();
                if (d.inputType ==  Db.actionZipInputType.custom)
                    files.AddRange(d.file);
                if (d.inputType == Db.actionZipInputType.fromInput)
                     files = InputUtils.getListFromInputByType(input, d.inputAttributeKey);

                
               
                string folder = d.folderToSave;
                if (folder.EndsWith("\\"))
                    folder = folder.Substring(0, folder.Length - 1);
                string fullZipName = folder + "\\" + d.zipFileName;

                if (System.IO.File.Exists(fullZipName))
                    System.IO.File.Delete(fullZipName);

                Ionic.Zip.ZipFile z = new Ionic.Zip.ZipFile();
                foreach (string f in files)
                {
                    z.AddFile(f);                       
                }
                z.Save(fullZipName);

                return files;
            }
            return new List<string>();
        }






    }
}
