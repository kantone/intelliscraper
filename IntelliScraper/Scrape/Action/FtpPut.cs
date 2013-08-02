using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IntelliScraper.Scrape.Action
{
    public class FtpPut 
    {
        
        Db.ftpPut d { get; set; }
        public FtpPut(Db.ftpPut d)
        {
            this.d = d;
        }

        /// <summary>
        /// Execute Ftp
        /// </summary>
        /// <param name="input">list of files </param>
        /// <returns>list of response message</returns>
        public List<string> Run(List<string> files)
        {
            List<string> responses = new List<string>();
            if (d != null)
            {
                Db.intelliScraperProjectFtpSetting ftpInfo = (from x in Factory.Instance.i.Project.FtpSetting where x.id == d.ftpId select x).FirstOrDefault();
                if (ftpInfo != null)
                {
                    FtpManager f = new FtpManager(ftpInfo.ftpServerString, ftpInfo.authenticate, ftpInfo.user, ftpInfo.pass,ftpInfo.domain);
                    foreach (string file in files)
                        responses.Add(f.upload(file));
                }
                return responses;
            }
            return responses;
        }


    }
}
