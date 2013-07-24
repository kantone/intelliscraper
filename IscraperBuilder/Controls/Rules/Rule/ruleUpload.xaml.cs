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
    /// Logica di interazione per ruleUpload.xaml
    /// </summary>
    public partial class ruleUpload : Page,IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.upload rule { get; set; }
        public ruleUpload(string id)
        {
            inizialize(id);
        }

        private void inizialize(string id)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.upload where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            //txtH.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            //txtW.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            load();
        }

        private void load()
        {
            cmbMethod.Items.Clear();
            cmbInputType.Items.Clear();

            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.uploadMethod)))
                cmbMethod.Items.Add(s);

            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.uploadInputType)))
                cmbInputType.Items.Add(s);

            if (rule != null)
            {
                txtId.Text = rule.id;
                txtInputAttrKey.Text = rule.inputAttributeKey;
                txtUploadUrl.Text = rule.uploadUrl;
                txtCustomFileToUpd.Text = rule.customFileToUpload;
                cmbMethod.SelectedValue = rule.method.ToString();
                cmbInputType.SelectedValue = rule.inputType.ToString();
                chkUsePostData.IsChecked = rule.usePostData;

                txtCustomUserAgent.Text = rule.customUserAgent;
                if(rule.customHttpHeadersInfo != null)
                    httpHeaderInfo1.load(rule.customHttpHeadersInfo);

                if (rule.postData != null)
                {
                    listViewxPostData.ItemsSource = rule.postData;
                }

                cmbInputType_SelectionChanged(this, null);
            }
        }

        /// <summary>
        /// add post data
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPDKey.Text) && !string.IsNullOrEmpty(txtPDValue.Text))
            {
                IntelliScraper.Db.uploadPostData pd = new IntelliScraper.Db.uploadPostData();
                pd.key = txtPDKey.Text;
                pd.value = txtPDValue.Text;

                if (rule.postData == null)
                    rule.postData = new IntelliScraper.Db.uploadPostDataCollection();

                rule.postData.Add(pd);
                Factory.Instance.Save();
                load();
                CollectionViewSource.GetDefaultView(rule.postData).Refresh();
            }
            else MessageBox.Show("Missing key or value!", "Error");
        }

        /// <summary>
        /// Remove post data
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (listViewxPostData.SelectedIndex >= 0)
            {
                if (rule.postData != null)
                {
                    rule.postData.RemoveAt(listViewxPostData.SelectedIndex);
                    Factory.Instance.Save();
                    load();
                    CollectionViewSource.GetDefaultView(rule.postData).Refresh();
                }
            }
        }

        /// <summary>
        /// Change type
        /// </summary>        
        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.uploadInputType t = (IntelliScraper.Db.uploadInputType)Enum.Parse(typeof(IntelliScraper.Db.uploadInputType), (string)cmbInputType.SelectedValue);
                if (t == IntelliScraper.Db.uploadInputType.custom)
                {
                    txtInputAttrKey.IsEnabled = false;
                }
                if (t == IntelliScraper.Db.uploadInputType.fromInput)
                {
                    txtInputAttrKey.IsEnabled = true;
                }

            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Select file
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                txtCustomFileToUpd.Text = dlg.FileName;
            }

        }

        /// <summary>
        /// Save
        /// </summary>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Status = "";
            if (cmbMethod.SelectedValue != null)
                rule.method = (IntelliScraper.Db.uploadMethod)Enum.Parse(typeof(IntelliScraper.Db.uploadMethod), (string)cmbMethod.SelectedValue);

            if (cmbInputType.SelectedValue != null)
                rule.inputType = (IntelliScraper.Db.uploadInputType)Enum.Parse(typeof(IntelliScraper.Db.uploadInputType), (string)cmbInputType.SelectedValue);

           

            rule.id = txtId.Text;
            rule.inputAttributeKey = txtInputAttrKey.Text;
            rule.uploadUrl = txtUploadUrl.Text;
            rule.customFileToUpload = txtCustomFileToUpd.Text;
            rule.usePostData = (bool)chkUsePostData.IsChecked;
            rule.customUserAgent = txtCustomUserAgent.Text;
            rule.customHttpHeadersInfo = httpHeaderInfo1.getHeaders();

            Factory.Instance.Save();
            inizialize(this.id);

            MainWindow.main.Status = "Saved!";

        }
    }
}
