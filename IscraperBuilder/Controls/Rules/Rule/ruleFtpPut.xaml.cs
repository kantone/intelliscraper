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

namespace IscraperBuilder.Controls.Rules.Rule
{
    /// <summary>
    /// Logica di interazione per ruleFtpPut.xaml
    /// </summary>
    public partial class ruleFtpPut : Page,IRule
    {
        public ruleFtpPut()
        {
            InitializeComponent();
        }
         public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.ftpPut rule { get; set; }
        public ruleFtpPut(string id)
        {
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.ftpPut where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            load();
        }

        private void load()
        {
            cmbInputType.Items.Clear();

            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.ftpPutInputType)))
                cmbInputType.Items.Add(s);

           
            cmbFtpRuleId.Items.Clear();
            if (Factory.Instance.i.Project.FtpSetting != null)
            {
                foreach (var s in Factory.Instance.i.Project.FtpSetting)
                    cmbFtpRuleId.Items.Add(s.id);
            }
            


            if (rule != null)
            {
                //set selected ftp rule
                if (!string.IsNullOrEmpty(rule.ftpId))
                {
                    if (Factory.Instance.i.Project.FtpSetting != null)
                    {
                        IntelliScraper.Db.intelliScraperProjectFtpSetting ftp = (from x in Factory.Instance.i.Project.FtpSetting where x.id == rule.ftpId select x).SingleOrDefault();
                        cmbFtpRuleId.SelectedValue = ftp.id;
                    }
                }
                cmbInputType.SelectedValue = rule.inputType.ToString();
                cmbInputType_SelectionChanged(this, null);


                txtId.Text = rule.id;
                txtInputAttrKey.Text = rule.inputAttributeKey;

                listViewxFiles.Items.Clear();
                if (rule.file != null)
                {
                    foreach (string f in rule.file)
                        listViewxFiles.Items.Add(f);
                }
            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add File
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFile.Text))
                listViewxFiles.Items.Add(txtFile.Text);
            else MessageBox.Show("File name is mandatory!", "Error");
        }

        /// <summary>
        /// Delete File
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (listViewxFiles.SelectedIndex >= 0)
            {
                listViewxFiles.Items.RemoveAt(listViewxFiles.SelectedIndex);
                rule.file = new string[listViewxFiles.Items.Count];
                for (int i = 0; i < listViewxFiles.Items.Count; i++)
                    rule.file[i] = (string)listViewxFiles.Items[i];

                Factory.Instance.Save();
                load();
                CollectionViewSource.GetDefaultView(rule.file).Refresh();
            }
        }


        /// <summary>
        /// select file
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                txtFile.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button4_Click(object sender, RoutedEventArgs e)
        {

            if(cmbFtpRuleId.SelectedValue != null)
                rule.ftpId = cmbFtpRuleId.SelectedValue.ToString();

            rule.inputAttributeKey = txtInputAttrKey.Text;

            if (cmbInputType.SelectedValue != null)
                rule.inputType = (IntelliScraper.Db.ftpPutInputType)Enum.Parse(typeof(IntelliScraper.Db.ftpPutInputType), (string)cmbInputType.SelectedValue);
           
            rule.file = new string[listViewxFiles.Items.Count];
            for (int i = 0; i < listViewxFiles.Items.Count; i++)
                rule.file[i] = (string)listViewxFiles.Items[i];


            Factory.Instance.Save();
            load();
        }

        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.ftpPutInputType t = (IntelliScraper.Db.ftpPutInputType)Enum.Parse(typeof(IntelliScraper.Db.ftpPutInputType), (string)cmbInputType.SelectedValue);
                if (t == IntelliScraper.Db.ftpPutInputType.customFiles)
                {
                    txtInputAttrKey.IsEnabled = false;
                }
                if (t == IntelliScraper.Db.ftpPutInputType.fromInput)
                {
                    txtInputAttrKey.IsEnabled = true;
                }

            }
        }
    }
}
