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
    /// Logica di interazione per AddStorage.xaml
    /// </summary>
    public partial class AddStorage : Page
    {
        public AddStorage()
        {
            InitializeComponent();
            load(); 
        }

        private void load()
        {
            cmbType.Items.Clear();
            string[] types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperProjectStoreInfoType));
            foreach (string t in types)
            {
                cmbType.Items.Add(t);
            }
            cmbType.SelectedIndex = 0;

            if (Factory.Instance.i.Project.StoreInfo != null)
            {
               txtNewId.Text = (Factory.Instance.i.Project.StoreInfo.Count + 1).ToString();

                cmbIds.Items.Clear();
                foreach (IntelliScraper.Db.intelliScraperProjectStoreInfo st in Factory.Instance.i.Project.StoreInfo)
                    cmbIds.Items.Add(st.Id);

                if(Factory.Instance.i.Project.StoreInfo.Count >=0)
                    cmbIds.SelectedIndex = 0;

            }
            else txtNewId.Text = "1";
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtProviderName.Text = (string)((ComboBoxItem)comboBox2.SelectedValue).Content;
        }

        /// <summary>
        /// Change Storage Selection
        /// </summary>
        private void cmbIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.main.Status = "";
            try
            {
                string id = (string)cmbIds.SelectedValue;
                if (id != null)
                {
                    IntelliScraper.Db.intelliScraperProjectStoreInfo st = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == id select x).FirstOrDefault();
                    if (st != null)
                    {
                        if (st.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.csv || st.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.excel)
                        {
                            tabControl2.SelectedIndex = 0;
                            tabDB.Visibility = System.Windows.Visibility.Hidden;
                            tabCsv.Visibility = System.Windows.Visibility.Visible;
                            txtCsvSeparator.Text = st.CsvExcelSetting.csvSeparator;
                            txtHeaders.Text = st.CsvExcelSetting.csvHeader;
                            txtCsvSaveTo.Text = st.CsvExcelSetting.csvFileSaveTo;
                            chkCsvClearFile.IsChecked = st.CsvExcelSetting.csvFileClear;
                            txtWorkSheet.Text = st.CsvExcelSetting.workSheetName;
                        }
                        else
                        {
                            tabControl2.SelectedIndex = 1;
                            tabCsv.Visibility = System.Windows.Visibility.Hidden;
                            tabDB.Visibility = System.Windows.Visibility.Visible;

                            txtConnectionString.Text = st.DatabaseSetting.connection;
                            txtProviderName.Text = st.DatabaseSetting.providerName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error " + ex.Message;
            }
        }

        /// <summary>
        /// Add Storage
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewId.Text))
            {
                string type = (string)cmbType.SelectedValue;
                IntelliScraper.Db.intelliScraperProjectStoreInfoType t = (IntelliScraper.Db.intelliScraperProjectStoreInfoType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperProjectStoreInfoType), type);
                if (t == IntelliScraper.Db.intelliScraperProjectStoreInfoType.xml || t == IntelliScraper.Db.intelliScraperProjectStoreInfoType.json)
                {
                    MessageBox.Show("Not supported Yet");
                }
                else
                {
                   IntelliScraper.Db.intelliScraperProjectStoreInfo st =  new IntelliScraper.Db.intelliScraperProjectStoreInfo();
                    st.Id = txtNewId.Text;
                    st.type = (IntelliScraper.Db.intelliScraperProjectStoreInfoType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperProjectStoreInfoType), type);
                    st.CsvExcelSetting = new IntelliScraper.Db.intelliScraperProjectStoreInfoCsvExcelSetting();
                    st.DatabaseSetting = new IntelliScraper.Db.intelliScraperProjectStoreInfoDatabaseSetting();
                    st.JsonSetting = new IntelliScraper.Db.intelliScraperProjectStoreInfoJsonSetting();
                    st.XmlSetting = new IntelliScraper.Db.intelliScraperProjectStoreInfoXmlSetting();
                    Factory.Instance.i.Project.StoreInfo.Add(st);
                    Factory.Instance.Save();
                    load();
                }
            }
            else MessageBox.Show("Id cannot be empty");
        }


        /// <summary>
        /// Csv Select file to save
        /// </summary>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                txtCsvSaveTo.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Delete Storage
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (cmbIds.SelectedIndex >= 0)
            {
                Factory.Instance.i.Project.StoreInfo.RemoveAt(cmbIds.SelectedIndex);
                Factory.Instance.Save();
                load();
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Status = "";
            try
            {
                string id = (string)cmbIds.SelectedValue;
                 IntelliScraper.Db.intelliScraperProjectStoreInfo st = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == id select x).FirstOrDefault();
                 if (st != null)
                 {
                     if (st.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.csv || st.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.excel)
                     {
                         st.CsvExcelSetting.csvSeparator= txtCsvSeparator.Text;
                         st.CsvExcelSetting.csvHeader= txtHeaders.Text;
                         st.CsvExcelSetting.csvFileSaveTo= txtCsvSaveTo.Text;
                         st.CsvExcelSetting.csvFileClear= (bool)chkCsvClearFile.IsChecked;
                         st.CsvExcelSetting.workSheetName = txtWorkSheet.Text;
                     }
                     else
                     {
                         st.DatabaseSetting.connection = txtConnectionString.Text;
                         st.DatabaseSetting.providerName  = txtProviderName.Text;
                     }
                     Factory.Instance.Save();
                     load();
                     MainWindow.main.Status = "Saved!";

                 }
            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }
    }
}
