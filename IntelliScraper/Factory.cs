using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Net;


namespace IntelliScraper
{
    public class Factory
    {
        //SIngleTon
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


        public ILog log { get; set; }
        public string  appDirectory { get; set; }
        public WebClient client { get; set; }
        public  IntelliScraper.Data.intelliScraper i { get; set; }
        public IntelliScraper.Scrape.ProxyManager proxyManager { get; set; }
        /// <summary>
        /// Inizialize Intelliscraper
        /// </summary>
        private Factory()
        {
            //get app directory
            this.appDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //log4Net 
            XmlConfigurator.Configure();
            this.log = LogManager.GetLogger("IntelliScraper");

            //Inizializate
            this.client = new WebClient();
            this.i = null;

            //Disable certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
        }


        /// <summary>
        /// Load some data and Run scraping
        /// </summary>
        public void Run(string xmlInput)
        {
            try
            {
                this.log.Info(string.Format("Load scraping rules from {0}", xmlInput));
                this.i = Xml.Serialization.Serialize(xmlInput);

                //load proxy
                if (i.settings.useProxy)
                {
                    this.log.Info(string.Format("Proxy support enabled, load proxy list from {0}", i.settings.proxyFile));
                    List<Scrape.ProxyModel> proxies =  IntelliScraper.Scrape.ProxyManager.GetProxyList(i.settings.proxyFile);
                    this.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                }

                Scraper s = new Scraper();
                s.RunScraping(i);
            }
            catch (Exception ex)
            {
                this.log.Error(ex);
            }
        }

        /// <summary>
        /// Set next proxy
        /// </summary>
        public void setNextWebClientProxy()
        {
            if (i.settings.useProxy)
            {
                Scrape.ProxyModel proxy = proxyManager.getNext();
                WebProxy p = new WebProxy(proxy.Ip, Int32.Parse(proxy.Port));
                Factory.Instance.client.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
                Factory.Instance.client.Proxy = p;
            }
        }
    }
}
