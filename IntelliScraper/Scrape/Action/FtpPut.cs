using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IntelliScraper.Scrape.Action
{
    public class FtpPut : IScrapeAction
    {
        
        Db.ftpPut d { get; set; }
        public FtpPut(Db.ftpPut d)
        {
            this.d = d;
        }

        public string getName()
        {
            return "FtpPut";
        }

        public object Run(object input)
        {
            if (d != null)
            {
                List<string> files = new List<string>();
                if (d.inputType == Db.ftpPutInputType.customFiles)
                    files.AddRange(d.file);
                if(d.inputType ==  Db.ftpPutInputType.fromInput)
                     files = InputUtils.getListFromInputByType(input, d.inputAttributeKey);

                Db.intelliScraperProjectFtpSetting ftpInfo = (from x in Factory.Instance.i.Project.FtpSetting where x.id == d.ftpId select x).FirstOrDefault();
                if (ftpInfo != null)
                {
                    FtpManager f = new FtpManager(ftpInfo.ftpServerString, ftpInfo.authenticate, ftpInfo.user, ftpInfo.pass,ftpInfo.domain);
                    foreach (string file in files)
                        f.upload(file);
                }
                return files;
            }
            return new List<string>();
        }


    }
}
