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
    /// Logica di interazione per ruleZip.xaml
    /// </summary>
    public partial class ruleZip : Page,Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.actionZip rule { get; set; }
        public ruleZip(string id)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.actionZip where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            load();
        }

        private void load()
        {
            cmbInputType.Items.Clear();
            foreach(string s in Enum.GetNames(typeof(IntelliScraper.Db.actionZipInputType)))
                cmbInputType.Items.Add(s);

            if (rule != null)
            {
                txtId.Text = rule.id;
                chkDeleteZips.IsChecked = rule.deleteZippedFiles;
                txtFoldertosave.Text = rule.folderToSave;
                txtZipFname.Text = rule.zipFileName;
                txtInputAttrKey.Text = rule.inputAttributeKey;

                cmbInputType.SelectedValue = rule.inputType.ToString();
                cmbInputType_SelectionChanged(this, null);
            }
            

        }


        public string save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// On input type set
        /// </summary>
        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.actionZipInputType t = (IntelliScraper.Db.actionZipInputType)Enum.Parse(typeof(IntelliScraper.Db.actionZipInputType), (string)cmbInputType.SelectedValue);
                if (t == IntelliScraper.Db.actionZipInputType.custom)
                {
                    txtInputAttrKey.IsEnabled = false;                    
                }
                if (t == IntelliScraper.Db.actionZipInputType.fromInput)
                {
                    txtInputAttrKey.IsEnabled = true;    
                }
              
            }
        }

        /// <summary>
        /// Select dir
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                txtFoldertosave.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            rule.id = txtId.Text;
            rule.deleteZippedFiles = (bool)chkDeleteZips.IsChecked;
            rule.folderToSave = txtFoldertosave.Text;
            rule.zipFileName = txtZipFname.Text;
            rule.inputAttributeKey = txtInputAttrKey.Text;

            if (cmbInputType.SelectedValue != null)
                rule.inputType = (IntelliScraper.Db.actionZipInputType)Enum.Parse(typeof(IntelliScraper.Db.actionZipInputType), (string)cmbInputType.SelectedValue);

            Factory.Instance.Save();
            load();
        }
    }
}
