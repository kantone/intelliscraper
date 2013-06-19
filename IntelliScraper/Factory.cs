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
        public IntelliScraper.Scrape.UserAgentManager userAgentManager { get; set; }
        private System.IO.StreamWriter csvWriter { get; set; }
        //Add custom logger info
        private static readonly log4net.Core.Level notifyLevel = new log4net.Core.Level(50000, "I-INFO");
       
       

        /// <summary>
        /// Inizialize Intelliscraper
        /// </summary>
        private Factory()
        {
            
            //get app directory
            this.appDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //log4Net 
            log4net.LogManager.GetRepository().LevelMap.Add(notifyLevel);
            XmlConfigurator.Configure();
            this.log = LogManager.GetLogger("IntelliScraper");

            //Inizializate
            this.client = new WebClient();
            this.i = null;

            
        }

        //Custom Logging
        public void iInfo(string message)
        {
            if (this.i != null)
            {
                if (this.i.settings.logIscraperInfo)
                {
                    log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, notifyLevel, message, null);                 
                }
            }
            
        }

        /// <summary>
        /// Csv Writer
        /// </summary>
        public System.IO.StreamWriter openCsvFileWriter()
        {

            bool append = true;
            if (this.i.storeInfo.csvFileClear)
                append = false;
            this.csvWriter = new System.IO.StreamWriter(this.i.storeInfo.csvFileSaveTo, append);
            return this.csvWriter;
        }

        /// <summary>
        /// Add Line to csv
        /// </summary>
        public bool appendCsvLine(string[] data)
        {
            try
            {
                System.IO.StreamWriter rw = openCsvFileWriter();
                string line = string.Empty;
                foreach (string s in data)
                {
                    line += string.Format("{0}{1}", s,this.i.storeInfo.csvSeparator);
                }
                rw.WriteLine(line);
                rw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Factory.instance.log.Error(ex);
                return false;
            }
        }


        /// <summary>
        /// Load some data and Run scraping
        /// </summary>
        public void Run(string xmlInput)
        {
            try
            {
               // this.log.Info(string.Format("Load scraping rules from {0}", xmlInput));
                this.i = Xml.Serialization.Serialize(xmlInput);

                if (i.settings.showInitialMessage)
                {
                    Console.WriteLine(i.settings.initialMessage);
                    Console.WriteLine(string.Empty);
                }

                //load proxy
                if (i.settings.useProxy)
                {
                    this.log.Info(string.Format("Proxy support enabled, load proxy list from {0}", i.settings.proxyFile));
                    List<Scrape.ProxyModel> proxies =  IntelliScraper.Scrape.ProxyManager.GetProxyList(i.settings.proxyFile);
                    this.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                }

                //Load user agents
                if (i.settings.useCustomUserAgentFileList)
                {
                    this.log.Info(string.Format("Loading user agent rotator from file {0}", i.settings.customUserAgentFile));
                    List<string> agents =  IntelliScraper.Scrape.UserAgentManager.GetUserAgentList(i.settings.customUserAgentFile);
                    this.userAgentManager = new Scrape.UserAgentManager(agents);
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
        public void setWebClientNext()
        {
            //User Agent
            if (i.settings.useCustomUserAgentFileList == false)
            {
                if (string.IsNullOrEmpty(i.settings.userAgent))
                    this.client.Headers.Add("user-agent", i.settings.userAgent);
            }
            else  this.client.Headers.Add("user-agent", this.userAgentManager.getNext());
            
            //Custom httpHeader
            if (i.settings.globalHttpHeader.Count > 0)
            {
                foreach (Data.httpHeaders h in i.settings.globalHttpHeader)
                    addHttpHeaderToClient(h);
            }

            //Proxy
            if (i.settings.useProxy)
            {
                //Disable certificate validation
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                Scrape.ProxyModel proxy = proxyManager.getNext();
                WebProxy p = new WebProxy(proxy.Ip, Int32.Parse(proxy.Port));
                
                if (!string.IsNullOrEmpty(proxy.Username) && !string.IsNullOrEmpty(proxy.Password))
                {
                    if (!string.IsNullOrEmpty(proxy.Domain))
                    {
                        p.Credentials = new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain);
                        Factory.Instance.client.Credentials = new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain);
                    }
                    else
                    {
                        p.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
                        Factory.Instance.client.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
                    }

                }
                Factory.Instance.client.Proxy = p;
            }
        }

        /// <summary>
        /// Add custom httpHeader to client
        /// </summary>
        public void addHttpHeaderToClient(Data.httpHeaders h)
        {
            if(!string.IsNullOrEmpty(h.name) && !string.IsNullOrEmpty(h.value))
                this.client.Headers.Add(h.name, h.value);
        }
    }
}
