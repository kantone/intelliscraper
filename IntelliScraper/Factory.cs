﻿using System;
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
        public  IntelliScraper.Db.intelliScraper i { get; set; }
        public IntelliScraper.Scrape.ProxyManager proxyManager { get; set; }
        public IntelliScraper.Scrape.UserAgentManager userAgentManager { get; set; }
        private System.IO.StreamWriter csvWriter { get; set; }
        public Plugin.PluginManager pluginManager { get; set; }
        public CookieAwareWebClient lastClient { get; set; }
        public bool isRunning = false;

        

        //Add custom logger info
        private static readonly log4net.Core.Level notifyLevel = new log4net.Core.Level(50001, "I-INFO");

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
            this.i = null;

            //Load plugins
            iInfo("Loading plugins");
            pluginManager = new Plugin.PluginManager();
            iInfo(string.Format("Loaded {0} plugins",pluginManager.plugins.Count.ToString()));
            
        }

        //Custom Logging
        public void iInfo(string message)
        {
            if (this.i != null)
            {
                if (this.i.Project.ProjectInfo.logIscraperInfo)
                {
                    log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, notifyLevel, message, null);                 
                }
            }
            
        }

        /// <summary>
        /// Csv Writer
        /// </summary>
        public System.IO.StreamWriter openCsvFileWriter(Db.intelliScraperProjectStoreInfo s)
        {

            bool append = true;
            if (s.CsvExcelSetting.csvFileClear)
                append = false;
            this.csvWriter = new System.IO.StreamWriter(s.CsvExcelSetting.csvFileSaveTo, append);
            return this.csvWriter;
        }

        /// <summary>
        /// Add Line to csv
        /// </summary>
        public bool appendCsvLine(string[] data, Db.intelliScraperProjectStoreInfo ss)
        {
            try
            {
                System.IO.StreamWriter rw = openCsvFileWriter(ss);
                string line = string.Empty;
                foreach (string s in data)
                {
                    line += string.Format("{0}{1}", s, ss.CsvExcelSetting.csvSeparator);
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
        public void Load(string xmlInput, bool crypted, string xmlDecriptPass)
        {
            try
            {
                if (crypted)
                {
                    string uncrypt = IntelliScraper.Crypto.DecryptStringAES(System.IO.File.ReadAllText(xmlInput), xmlDecriptPass);
                    this.i = Xml.Serialization.SerializeFromString(uncrypt);
                }
                else  this.i = Xml.Serialization.Serialize(xmlInput);


                if (i.Project.ProjectInfo.showInitialMessage)
                {
                    Console.WriteLine(i.Project.ProjectInfo.initialMessage);
                    Console.WriteLine(string.Empty);
                }

                //load proxy
                if (i.Project.ProxyInfo.useProxy)
                {
                    if (i.Project.ProxyInfo.type == Db.intelliScraperProjectProxyInfoType.file)
                    {
                        this.log.Info(string.Format("Proxy support enabled, load proxy list from {0}", i.Project.ProxyInfo.proxyFile));
                        List<Scrape.ProxyModel> proxies = IntelliScraper.Scrape.ProxyManager.GetProxyList(i.Project.ProxyInfo.proxyFile);
                        this.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                    }
                    else
                    {
                        this.log.Info(string.Format("Proxy support enabled, load {0} custom proxies", i.Project.ProxyInfo.proxies.Count.ToString()));
                        List<Scrape.ProxyModel> proxies = new List<Scrape.ProxyModel>();
                        foreach (Db.intelliScraperProjectProxyInfoProxies proxy in i.Project.ProxyInfo.proxies)
                        {
                            proxies.Add(new Scrape.ProxyModel(proxy.ip, proxy.port.ToString(), proxy.userName, proxy.password, proxy.domain));
                        }
                        this.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                    }
                }

                //Load user agent
                if (this.userAgentManager == null)
                    this.userAgentManager = new Scrape.UserAgentManager(new List<string>());

                this.userAgentManager.UserAgents.Add(i.Project.ScrapingSetting.defaultAgent);
                if (i.Project.ScrapingSetting.rotateUserAgents)
                {
                    //Load custom agents
                    if (i.Project.ScrapingSetting.GlobalUserAgentsInfo != null)
                    {
                        if (i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue != null)
                        {
                            foreach (string agent in i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue)
                                this.userAgentManager.UserAgents.Add(agent);
                        }
                    }

                    //Load user agents from file              
                    if (i.Project.ScrapingSetting.GlobalUserAgentsInfo.loadAgentsFromFile)
                    {
                        if (System.IO.File.Exists(i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile))
                        {
                            this.log.Info(string.Format("Loading user agent rotator from file {0}", i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile));
                            List<string> agents = IntelliScraper.Scrape.UserAgentManager.GetUserAgentList(i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile);
                            this.userAgentManager = new Scrape.UserAgentManager(agents);
                        }
                        else this.log.Info(string.Format("Cannot load user agents from file {0} (not exist)", i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile));
                    }
                }

            }
            catch (Exception ex)
            {
                this.log.Error(ex);
            }
            
            
        }

        /// <summary>
        /// Set next proxy
        /// </summary>
        public CookieAwareWebClient getWebClientNext(string customUserAgent,Db.HttpHeadersInfoCollection httpHeaders)
        {
            if (lastClient == null)
                lastClient = new CookieAwareWebClient();
            CookieAwareWebClient client = new CookieAwareWebClient();
            lastClient.CopyValues(client); ;
            
            System.Net.ServicePointManager.Expect100Continue = false;
            //User Agent
            if (!string.IsNullOrEmpty(customUserAgent))
            {
                client.Headers.Add("user-agent", customUserAgent);
            }
            else
            {
                if(this.userAgentManager != null)
                    client.Headers.Add("user-agent", this.userAgentManager.getNext());               
            }

            //Global httpHeader
            if (i.Project.ScrapingSetting != null)
            {
                foreach (Db.HttpHeadersInfo h in i.Project.ScrapingSetting.GlobalHttpHeadersInfo)
                    client = addHttpHeaderToClient(client, h);
            }

            //Custom httpHeader
            if (httpHeaders != null)
            {
                foreach (Db.HttpHeadersInfo h in httpHeaders)
                {
                    if (h != null && !string.IsNullOrEmpty(h.value))
                    {
                        if (h.value.ToLower() == "keepalive" || h.value.ToLower() == "keep-alive")
                            client.setKeepAliveOn();
                        else if (h.value.ToLower() == "close")
                            client.setKeepAliveOff();
                        else client = addHttpHeaderToClient(client, h);
                    }
                }
            }
            

            //Proxy
            if (i.Project.ProxyInfo.useProxy)
            {
                //Disable certificate validation
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                Scrape.ProxyModel proxy = proxyManager.getNext();
                WebProxy p = new WebProxy(proxy.Ip, Int32.Parse(proxy.Port));

                if (!string.IsNullOrEmpty(proxy.Username) && !string.IsNullOrEmpty(proxy.Password))
                {
                    if (!string.IsNullOrEmpty(proxy.Domain))
                    {
                        if (!string.IsNullOrEmpty(proxy.Username) &&   !string.IsNullOrEmpty(proxy.Password))
                        {
                            p.Credentials = new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain);
                            client.Credentials = new NetworkCredential(proxy.Username, proxy.Password, proxy.Domain);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(proxy.Username) && !string.IsNullOrEmpty(proxy.Password))
                        {
                            p.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
                            client.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
                        }
                    }

                }
                
                client.Proxy = p;
            }
            lastClient = client;
            return client;
        }
        
        /// <summary>
        /// Add custom httpHeader to client
        /// </summary>
        public CookieAwareWebClient addHttpHeaderToClient(CookieAwareWebClient client, Db.HttpHeadersInfo h)
        {
            if (!string.IsNullOrEmpty(h.name) && !string.IsNullOrEmpty(h.value))
                client.Headers.Add(h.name, h.value);
            return client;
        }
    }
}
