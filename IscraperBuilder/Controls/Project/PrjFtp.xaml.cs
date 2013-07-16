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
    /// Logica di interazione per PrjFtp.xaml
    /// </summary>
    public partial class PrjFtp : Page
    {
        private bool isNew = false;
        private IntelliScraper.Db.intelliScraperProjectFtpSetting currentFtp { get; set; }
        public PrjFtp()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            if (Factory.Instance.i.Project.FtpSetting != null)
            {
                comboBox1.Items.Clear();
                foreach (IntelliScraper.Db.intelliScraperProjectFtpSetting f in Factory.Instance.i.Project.FtpSetting)
                    comboBox1.Items.Add(f.id);

                if (Factory.Instance.i.Project.FtpSetting.Count >= 0)
                    comboBox1.SelectedIndex = 0;

            }
            else Factory.Instance.i.Project.FtpSetting = new IntelliScraper.Db.intelliScraperProjectFtpSettingCollection();
        }

        /// <summary>
        /// set as new
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            comboBox1.SelectedIndex = -1;
            txtDomain.Text = string.Empty;
            txtFptServerString.Text = string.Empty;
            txtNewId.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Text = string.Empty;
            chkAuthenticate.IsChecked = false;
        }

        /// <summary>
        /// Selection changed
        /// </summary>
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isNew = false;
            try
            {
                string id = (string)comboBox1.SelectedValue;
                IntelliScraper.Db.intelliScraperProjectFtpSetting f = (from x in Factory.Instance.i.Project.FtpSetting where x.id == id select x).FirstOrDefault();
                currentFtp = f;
                if (f != null)
                {
                    txtDomain.Text = f.domain;
                    txtFptServerString.Text = f.ftpServerString;
                    txtNewId.Text = f.id;
                    txtPassword.Text = f.pass;
                    txtUserName.Text = f.user;
                    chkAuthenticate.IsChecked = f.authenticate;
                }
            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }

        /// <summary>
        /// Add or update
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              

                //Add
                if (isNew)
                {
                    if (!string.IsNullOrEmpty(txtNewId.Text) && !string.IsNullOrEmpty(txtFptServerString.Text))
                    {

                        IntelliScraper.Db.intelliScraperProjectFtpSetting f = new IntelliScraper.Db.intelliScraperProjectFtpSetting();
                        f.domain = txtDomain.Text;
                        f.ftpServerString = txtFptServerString.Text;
                        f.id = txtNewId.Text;
                        f.pass = txtPassword.Text;
                        f.user = txtUserName.Text;
                        f.authenticate = (bool)chkAuthenticate.IsChecked;

                        Factory.Instance.i.Project.FtpSetting.Add(f);
                        Factory.Instance.Save();
                        load();
                         MainWindow.main.Status = "Added!";
                    }
                    else
                    {
                        MessageBox.Show("Id & FptServerString mandatory!");
                        return;
                    }
                }
                //update
                else
                {
                    string id = (string)comboBox1.SelectedValue;
                    IntelliScraper.Db.intelliScraperProjectFtpSetting f = (from x in Factory.Instance.i.Project.FtpSetting where x.id == id select x).FirstOrDefault();

                    f.domain = txtDomain.Text;
                    f.ftpServerString = txtFptServerString.Text;
                    f.id = txtNewId.Text;
                    f.pass = txtPassword.Text;
                    f.user = txtUserName.Text;
                    f.authenticate = (bool)chkAuthenticate.IsChecked;
                    Factory.Instance.Save();
                    load();
                    MainWindow.main.Status = "Updated!";
                }
               
            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }

        /// <summary>
        /// Test Connection
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IntelliScraper.FtpManager f = new IntelliScraper.FtpManager(txtFptServerString.Text, (bool)chkAuthenticate.IsChecked, txtUserName.Text, txtPassword.Text, txtDomain.Text);
                if(f.testConnection())
                    MainWindow.main.Status = "Connected!";
                else MainWindow.main.Status = "Cannot connect!";
            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }
    }
}
