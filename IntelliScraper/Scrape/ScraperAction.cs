using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntelliScraper.Scrape
{
    public static class ScraperAction
    {
        /// <summary>
        /// Get html
        /// </summary>
        public static string getHtml(string url)
        {
            try{

            //set new proxy if use proxy
            Factory.Instance.setNextWebClientProxy();

            //Set Wait time
            System.Threading.Thread.Sleep(Factory.Instance.i.settings.waitEachRequestMilliseconds);
            
            Stream s = Factory.Instance.client.OpenRead(url); ;
            StreamReader sr = new StreamReader(s);
            string html = sr.ReadToEnd();
            s.Close();
            return html;
            }
            catch(Exception ex)
            {
                //Wait after error - prevent next request
                System.Threading.Thread.Sleep(Factory.Instance.i.settings.waitAfterError);
                Factory.Instance.log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Download image
        /// </summary>
        public static bool downloadImage(string imgUrl,string saveTo)
        {
            using (Factory.Instance.client)
            {
                try
                {
                    //set new proxy if use proxy
                    Factory.Instance.setNextWebClientProxy();

                    //download image
                    Factory.Instance.client.DownloadFile(imgUrl, saveTo);
                    return true;
                }
                catch (Exception ex)
                {
                    Factory.Instance.log.Error(ex);
                    return false;
                }
            }
        }
    }
}
