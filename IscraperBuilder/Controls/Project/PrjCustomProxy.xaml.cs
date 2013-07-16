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

namespace IscraperBuilder.Controls.Project
{
    /// <summary>
    /// Logica di interazione per PrjCustomProxy.xaml
    /// </summary>
    public partial class PrjCustomProxy : Page
    {
        public PrjCustomProxy()
        {
            InitializeComponent();
            loadProxies();
        }

        private void loadProxies()
        {
            if (Factory.Instance.i.Project.ProxyInfo != null)
            {
                if (Factory.Instance.i.Project.ProxyInfo.proxies != null)
                {
                    var sequence = Factory.Instance.i.Project.ProxyInfo.proxies;
                    listViewProxies.ItemsSource = sequence;     
                    
                }
            }

            
        }

        /// <summary>
        /// Add
        /// </summary>      
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try{
                IntelliScraper.Db.intelliScraperProjectProxyInfoProxies p = new IntelliScraper.Db.intelliScraperProjectProxyInfoProxies();
                p.autenticate = (bool)checkAuthenticate.IsChecked;
                p.domain = txtDomain.Text;
                p.ip = txtIp.Text;
                p.password = txtPass.Text;
                p.port = txtPort.Text;
                p.userName = txtUser.Text;
                if (Factory.Instance.i.Project.ProxyInfo.proxies == null)
                    Factory.Instance.i.Project.ProxyInfo.proxies = new IntelliScraper.Db.intelliScraperProjectProxyInfoProxiesCollection();
                Factory.Instance.i.Project.ProxyInfo.proxies.Add(p);
                loadProxies();
                CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ProxyInfo.proxies).Refresh();

            }
            catch(Exception ex){
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }

        /// <summary>
        /// remove
        /// </summary>    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = listViewProxies.SelectedIndex;
            if (index >= 0)
            {
                Factory.Instance.i.Project.ProxyInfo.proxies.RemoveAt(index);
                loadProxies();
                CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ProxyInfo.proxies).Refresh();
            }
            else MainWindow.main.Status = "Warn : cannot remove! Select a row before remove item";
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Factory.Instance.Save();
            MainWindow.main.Status = "Saved!";
        }
    }
}
