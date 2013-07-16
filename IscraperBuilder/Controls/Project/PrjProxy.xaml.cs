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
    /// Logica di interazione per PrjProxy.xaml
    /// </summary>
    public partial class PrjProxy : Page
    {
        public PrjProxy()
        {
            InitializeComponent();
            loadSetting();
        }

        private void loadSetting()
        {
            if (Factory.Instance.i.Project.ProxyInfo != null)
            {
                this.useProxy.IsChecked = Factory.Instance.i.Project.ProxyInfo.useProxy;
                this.proxyFilePath.Text = Factory.Instance.i.Project.ProxyInfo.proxyFile;
                if (Factory.Instance.i.Project.ProxyInfo.type == IntelliScraper.Db.intelliScraperProjectProxyInfoType.file)
                    this.proxyType.SelectedIndex = 0;
                if (Factory.Instance.i.Project.ProxyInfo.type == IntelliScraper.Db.intelliScraperProjectProxyInfoType.custom)
                    this.proxyType.SelectedIndex = 1;
            }
            else Factory.Instance.i.Project.ProxyInfo = new IntelliScraper.Db.intelliScraperProjectProxyInfo();
        }

        private void setConfig()
        {
            Factory.Instance.i.Project.ProxyInfo.useProxy= (bool)this.useProxy.IsChecked;
            Factory.Instance.i.Project.ProxyInfo.proxyFile= this.proxyFilePath.Text;
            Factory.Instance.i.Project.ProxyInfo.type = IntelliScraper.Db.intelliScraperProjectProxyInfoType.file;
            if (this.proxyType.SelectedIndex == 1)
                 Factory.Instance.i.Project.ProxyInfo.type = IntelliScraper.Db.intelliScraperProjectProxyInfoType.custom;
        }

        /// <summary>
        /// Open proxy file
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                proxyFilePath.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Validate Proxy File
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(this.proxyFilePath.Text)){
                    IntelliScraper.Scrape.ProxyManager.GetProxyList(this.proxyFilePath.Text);
                    MainWindow.main.Status = "Proxy file is VALID!";
                }
                else MainWindow.main.Status = "Error : not proxy file selected!!";
            }
            catch(Exception ex)
            {
                MainWindow.main.Status = "Error " + ex.Message;
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            setConfig();
            Factory.Instance.Save();
            MainWindow.main.Status = "Saved!";
        }
    }
}
