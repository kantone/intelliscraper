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
using System.Diagnostics;

namespace IscraperBuilder.Controls.Project
{



    /// <summary>
    /// Logica di interazione per PrjBrowser2.xaml
    /// </summary>
    public partial class PrjBrowser2 : Page
    {
        public PrjBrowser2()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            if (Factory.Instance.i.Project.ScrapingSetting != null)
            {
                txtWait.Text = Factory.Instance.i.Project.ScrapingSetting.waitEachRequestMilliseconds.ToString();
                txtErrorWait.Text = Factory.Instance.i.Project.ScrapingSetting.waitAfterError.ToString();
                txtDownloadfolder.Text = Factory.Instance.i.Project.ScrapingSetting.imageFolderDownload;

                if (Factory.Instance.i.Project.ScrapingSetting.credential != null)
                {
                    chkUseCredential.IsChecked = Factory.Instance.i.Project.ScrapingSetting.credential.useNetworkCredential;
                    txtCredDomain.Text = Factory.Instance.i.Project.ScrapingSetting.credential.domain;
                    txtCredPass.Text = Factory.Instance.i.Project.ScrapingSetting.credential.password;
                    txtCredUserName.Text = Factory.Instance.i.Project.ScrapingSetting.credential.username;
                }

                chkRotateUserAgent.IsChecked = Factory.Instance.i.Project.ScrapingSetting.rotateUserAgents;
                txtDefaultUserAgent.Text = Factory.Instance.i.Project.ScrapingSetting.defaultAgent;
                if (Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo != null)
                {
                    chkUserAgentFromFile.IsChecked = Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.loadAgentsFromFile;
                    txtUserAgentFile.Text = Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile;
                }

                if (Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo != null)
                {
                    var sequence = Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo;
                    listViewHttpHeaders.ItemsSource = sequence;
                }

                if (Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo != null)
                {
                    if (Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue != null)
                    {
                        object[] x = new object[Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue.Length];

                        int i = 0;
                        foreach (string a in Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue)
                        {
                            x[i] = new { agentValue = a };
                            i++;
                        }
                        listViewUserAgent.ItemsSource = x;
                    }
                }

                if (string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.defaultAgent))
                    txtDefaultUserAgent.Text = System.Configuration.ConfigurationManager.AppSettings["defaultUserAgent"];
            }
            else
            {
                Factory.Instance.i.Project.ScrapingSetting = new IntelliScraper.Db.intelliScraperProjectScrapingSetting();
                txtDefaultUserAgent.Text = System.Configuration.ConfigurationManager.AppSettings["defaultUserAgent"];
                txtWait.Text = System.Configuration.ConfigurationManager.AppSettings["defaultWaitRequest"];
               txtErrorWait.Text = System.Configuration.ConfigurationManager.AppSettings["defaultWaitRequestAfterError"];
 
            }

          

        }

        /// <summary>
        /// Add Http Header
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHname.Text) && !string.IsNullOrEmpty(txtHValue.Text))
            {
                IntelliScraper.Db.HttpHeadersInfo i = new IntelliScraper.Db.HttpHeadersInfo();
                i.name = txtHname.Text;
                i.value = txtHValue.Text;
                if (Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo == null)
                    Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo = new IntelliScraper.Db.HttpHeadersInfoCollection();
                Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo.Add(i);
                loadData();
                CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo).Refresh();
            }
            else MessageBox.Show("Name & Value Mandatory!", "Error");
        }

        /// <summary>
        /// Delete Header
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if(listViewHttpHeaders.SelectedIndex >= 0){              
               Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo.RemoveAt(listViewHttpHeaders.SelectedIndex);
               loadData();
               CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo).Refresh();
            }
        }

        /// <summary>
        /// Remove User Agent
        /// </summary>
        private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (listViewUserAgent.SelectedIndex >= 0)
            {
                string val  =  (string)Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue[listViewUserAgent.SelectedIndex];               
                string[] vals = Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue.Where(b => b != val).ToArray();
                Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue = vals;
                loadData();
                CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue).Refresh();
            }
        }

        /// <summary>
        /// Add User Agent
        /// </summary>
        private void btnAddUAgent_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCustmUserAgent.Text))
            {
                object val = new { agentValue = txtCustmUserAgent.Text };
                string[] dt = new string[listViewUserAgent.Items.Count+1];
                int i = 0;
                foreach (var o in listViewUserAgent.Items)
                {
                    dt[i] = (string)o.GetType().GetProperty("agentValue").GetValue(o,null);
                    i++;                    
                }
                dt[listViewUserAgent.Items.Count] = txtCustmUserAgent.Text;
                if (Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo == null)
                    Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo = new IntelliScraper.Db.intelliScraperProjectScrapingSettingGlobalUserAgentsInfo();
                Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue = dt;
                loadData();
                CollectionViewSource.GetDefaultView(Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.agentValue).Refresh();
            }
            else MessageBox.Show("User Agent value is Mandatory!", "Error");
        }

        Utils.PopUp p = new Utils.PopUp();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            p.hide();
            if (validate())
            {
                Factory.Instance.i.Project.ScrapingSetting.waitEachRequestMilliseconds = Int32.Parse(txtWait.Text);
                Factory.Instance.i.Project.ScrapingSetting.waitAfterError = Int32.Parse(txtErrorWait.Text);
                Factory.Instance.i.Project.ScrapingSetting.imageFolderDownload = txtDownloadfolder.Text;

                if (Factory.Instance.i.Project.ScrapingSetting.credential == null)
                    Factory.Instance.i.Project.ScrapingSetting.credential = new IntelliScraper.Db.intelliScraperProjectScrapingSettingCredential();

                Factory.Instance.i.Project.ScrapingSetting.credential.useNetworkCredential = (bool)chkUseCredential.IsChecked;
                Factory.Instance.i.Project.ScrapingSetting.credential.domain = txtCredDomain.Text;
                Factory.Instance.i.Project.ScrapingSetting.credential.password = txtCredPass.Text;
                Factory.Instance.i.Project.ScrapingSetting.credential.username = txtCredUserName.Text;


                Factory.Instance.i.Project.ScrapingSetting.rotateUserAgents = (bool)chkRotateUserAgent.IsChecked;
                Factory.Instance.i.Project.ScrapingSetting.defaultAgent = txtDefaultUserAgent.Text;

                if (Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo == null)
                    Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo = new IntelliScraper.Db.intelliScraperProjectScrapingSettingGlobalUserAgentsInfo();

                

                Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.loadAgentsFromFile = (bool)chkUserAgentFromFile.IsChecked;
                Factory.Instance.i.Project.ScrapingSetting.GlobalUserAgentsInfo.customUserAgentFile = txtUserAgentFile.Text;



                Factory.Instance.Save();
                MainWindow.main.Status = "Saved!";
            }
            else p.show("Errors ( cannot save! )", button1);
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(txtWait.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtWait);
                return false;
            }

            if (string.IsNullOrEmpty(txtWait.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtErrorWait);
                return false;
            }

            if (string.IsNullOrEmpty(txtWait.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtDefaultUserAgent);
                return false;
            }

            if ((bool)chkUseCredential.IsChecked)
            {
                if (string.IsNullOrEmpty(txtCredUserName.Text))
                {
                    Utils.PopUp.showPopUpError("mandatory", txtCredUserName);
                    return false;
                }
                if (string.IsNullOrEmpty(txtCredPass.Text))
                {
                    Utils.PopUp.showPopUpError("mandatory", txtCredPass);
                    return false;
                }
            }
            
            

            return true;
        }

        private void navigate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(System.Configuration.ConfigurationManager.AppSettings["UserAgentLink"]);
        }

        /// <summary>
        /// Select user agent File
        /// </summary>
        private void btnUAgentSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                txtUserAgentFile.Text = dlg.FileName;
            }

        }

        /// <summary>
        /// Select download Folder
        /// </summary>
        private void dwnFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            txtDownloadfolder.Text = dialog.SelectedPath;
        }

       
    }
}

