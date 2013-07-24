using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IscraperBuilder.Controls.Tools
{
    /// <summary>
    /// Logica di interazione per Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                //load proxy
                if (Factory.Instance.i.Project.ProxyInfo.useProxy)
                {
                    if (Factory.Instance.i.Project.ProxyInfo.type == IntelliScraper.Db.intelliScraperProjectProxyInfoType.file)
                    {
                        List<IntelliScraper.Scrape.ProxyModel> proxies = IntelliScraper.Scrape.ProxyManager.GetProxyList(Factory.Instance.i.Project.ProxyInfo.proxyFile);
                        IntelliScraper.Factory.Instance.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                    }
                    else
                    {
                        List<IntelliScraper.Scrape.ProxyModel> proxies = new List<IntelliScraper.Scrape.ProxyModel>();
                        foreach (IntelliScraper.Db.intelliScraperProjectProxyInfoProxies proxy in Factory.Instance.i.Project.ProxyInfo.proxies)
                        {
                            proxies.Add(new IntelliScraper.Scrape.ProxyModel(proxy.ip, proxy.port.ToString(), proxy.userName, proxy.password, proxy.domain));
                        }
                       IntelliScraper.Factory.Instance.proxyManager = new IntelliScraper.Scrape.ProxyManager(proxies);
                    }
                }

                IntelliScraper.Factory.Instance.i = IntelliScraper.Xml.Serialization.Serialize(Factory.Instance.openedFileProject);
                //IntelliScraper.Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo = new IntelliScraper.Db.HttpHeadersInfoCollection();
               string html =  IntelliScraper.Scrape.HttpUtils.getHtml(textBox1.Text, Factory.Instance.i.Project.ScrapingSetting.defaultAgent,new IntelliScraper.Db.HttpHeadersInfoCollection(), false);
               richTextBox1.AppendText(html);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                richTextBox2.Document.Blocks.Clear();
                string html = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                HtmlAgilityPack.HtmlNode n = doc.DocumentNode.SelectSingleNode(textBox2.Text);
                if (n != null)
                {
                    richTextBox2.AppendText(n.InnerHtml);
                }
                else richTextBox2.AppendText("null");
            }
           
        }
    }
}
