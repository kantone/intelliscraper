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
    /// Logica di interazione per RuleHttpPost.xaml
    /// </summary>
    public partial class RuleHttpPost : Page, Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.httpPost rule { get; set; }
        private string placeHolderPostData { get; set; }
        public RuleHttpPost(string id)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.httpPost where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();

            load();
            this.placeHolderPostData = txtPostDataVal.Text;
            txtPostDataVal.GotFocus += new RoutedEventHandler(txtPostDataVal_GotFocus);
            txtPostDataVal.LostFocus += new RoutedEventHandler(txtPostDataVal_LostFocus);
        }

        /// <summary>
        /// PlaceHolder set
        /// </summary>       
        void txtPostDataVal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPostDataVal.Text == this.placeHolderPostData || txtPostDataVal.Text.Trim() == string.Empty)
            {
                txtPostDataVal.Text = placeHolderPostData;
                txtPostDataVal.Opacity = 0.5;
            }
        }

        /// <summary>
        /// PlaceHolder remove
        /// </summary>    
        void txtPostDataVal_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPostDataVal.Text = string.Empty;
            txtPostDataVal.Opacity = 1;
        }

        public void load()
        {
            cmbInputType.Items.Clear();          
            foreach (string val in Enum.GetNames(typeof(IntelliScraper.Db.httpPostUrlType)))            
                cmbInputType.Items.Add(val);
              
            
            cmbPostDataInputType.Items.Clear();
            foreach (string val in Enum.GetNames(typeof(IntelliScraper.Db.httpPostPostDataType)))
                 cmbPostDataInputType.Items.Add(val);

            if(this.rule != null){
                txtUId.Text = this.id;
                txtCustomUrl.Text = this.rule.customUrl;
                txtCustomUserAgent.Text = this.rule.customUserAgent;
                cmbInputType.SelectedValue = this.rule.urlType.ToString();

                if(this.rule.customHttpHeadersInfo != null)
                    httpHeaderInfo1.load(this.rule.customHttpHeadersInfo);

                if (this.rule.urlType.ToString() == "custom")
                {
                    txtInputAttrKey.IsEnabled = false;
                    txtCustomUrl.IsEnabled = true;
                }
                if (this.rule.urlType.ToString() == "fromInput")
                {
                    txtInputAttrKey.IsEnabled = true;
                    txtCustomUrl.IsEnabled = false;
                }

                if (this.rule.postData != null)
                {
                    listViewPostData.ItemsSource = this.rule.postData;
                }
            }

        }


        public string save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// On select input Type
        /// </summary>
        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                if ((string)cmbInputType.SelectedValue == "custom")
                {
                    txtInputAttrKey.IsEnabled = false;
                    txtCustomUrl.IsEnabled = true;
                }
                if ((string)cmbInputType.SelectedValue == "fromInput")
                {
                    txtInputAttrKey.IsEnabled = true;
                    txtCustomUrl.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Add post Data
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPostDataKey.Text) && !string.IsNullOrEmpty(txtPostDataVal.Text) && cmbPostDataInputType.SelectedValue != null && txtPostDataVal.Text != this.placeHolderPostData)
            {
                IntelliScraper.Db.httpPostPostData pd = new IntelliScraper.Db.httpPostPostData();
                if ((string)cmbPostDataInputType.SelectedValue == "custom")
                    pd.customValue = txtPostDataVal.Text;
                if ((string)cmbPostDataInputType.SelectedValue == "fromInput")
                    pd.inputAttributeKey = txtPostDataVal.Text;                
                pd.key = txtPostDataKey.Text;
                pd.type = (IntelliScraper.Db.httpPostPostDataType)Enum.Parse(typeof(IntelliScraper.Db.httpPostPostDataType), (string)cmbPostDataInputType.SelectedValue);
                this.rule.postData.Add(pd);
                CollectionViewSource.GetDefaultView(rule.postData).Refresh();
                Factory.Instance.Save();
                load();

                
            }
            else MessageBox.Show("All inputs are mandatory!","Error");
        }

        /// <summary>
        /// Remove selected
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {

            if (listViewPostData.SelectedIndex >= 0)
            {
                rule.postData.RemoveAt(listViewPostData.SelectedIndex);
                Factory.Instance.Save();
                load();
                CollectionViewSource.GetDefaultView(rule.postData).Refresh();               
            }
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.rule.customHttpHeadersInfo = httpHeaderInfo1.getHeaders();
            this.rule.customUrl = txtCustomUrl.Text;
            this.rule.customUserAgent = txtCustomUserAgent.Text;
            this.rule.inputUrlAttributeKey = txtInputAttrKey.Text;
            if(cmbInputType.SelectedValue == null){
                MessageBox.Show("Input Source type is mandatory","Error");
                return;
            }
            this.rule.urlType = (IntelliScraper.Db.httpPostUrlType)Enum.Parse(typeof(IntelliScraper.Db.httpPostUrlType),(string)cmbInputType.SelectedValue);
            
            Factory.Instance.Save();
            load();
            CollectionViewSource.GetDefaultView(rule.postData).Refresh();
            MainWindow.main.Status = string.Format("{0} Saved", this.rule.id);
        }
    }
}
