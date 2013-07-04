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

namespace IscraperBuilder.Controls.Rules
{
    /// <summary>
    /// Logica di interazione per RuleHttpGet.xaml
    /// </summary>
    public partial class RuleHttpGet : Page,Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.httpGet rule { get; set; }
        public RuleHttpGet(string id)
        {
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.httpGet where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();
            


            load();
        }


        private void load()
        {
            //Load types
            string[] types = Enum.GetNames(typeof(IntelliScraper.Db.httpGetUrlType)); 
            cmbUrlType.Items.Clear();
            foreach (string t in types)
                cmbUrlType.Items.Add(t);

            if (!isNew && rule != null)
            {
                httpHeaderInfo1.load(rule.customHttpHeadersInfo);
                txtId.Text = rule.id;
                txtId.IsEnabled = false;
                txtCustomUrl.Text = rule.customUrl;
                txtCustUserAgent.Text = rule.customUserAgent;
                cmbUrlType.SelectedValue = Enum.GetName(typeof(IntelliScraper.Db.httpGetUrlType), rule.urlType);
               /* if (rule.urlType == IntelliScraper.Db.httpGetUrlType.fromInput)
                    txtCustomUrl.IsEnabled = false;
                else txtCustomUrl.IsEnabled = true;*/
            }
        }

        private void cmbUrlType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUrlType.SelectedValue != null)
            {
                IntelliScraper.Db.httpGetUrlType t = (IntelliScraper.Db.httpGetUrlType)Enum.Parse(typeof(IntelliScraper.Db.httpGetUrlType), (string)cmbUrlType.SelectedValue);
                if (t == IntelliScraper.Db.httpGetUrlType.fromInput)
                    txtCustomUrl.IsEnabled = false;
                else txtCustomUrl.IsEnabled = true;
            }
        }

        public string save()
        {
            if (isNew)
            {
                IntelliScraper.Db.httpGet r = new IntelliScraper.Db.httpGet();
                r.id = txtId.Text;
                r.customHttpHeadersInfo = httpHeaderInfo1.getHeaders();
                r.customUserAgent = txtCustUserAgent.Text;
                r.urlType = (IntelliScraper.Db.httpGetUrlType)Enum.Parse(typeof(IntelliScraper.Db.httpGetUrlType), (string)cmbUrlType.SelectedValue);
                r.customUrl = txtCustomUrl.Text;

                if (Factory.Instance.i.rules.httpGet != null)
                    Factory.Instance.i.rules.httpGet = new IntelliScraper.Db.httpGetCollection();

                Factory.Instance.i.rules.httpGet.Add(r);
                Factory.Instance.Save();
                return "Added " + txtId.Text;
            }
            else
            {
                string oldId = this.rule.id;
                this.rule.id = txtId.Text;
                this.rule.customHttpHeadersInfo = httpHeaderInfo1.getHeaders();
                this.rule.customUserAgent = txtCustUserAgent.Text;
                this.rule.urlType = (IntelliScraper.Db.httpGetUrlType)Enum.Parse(typeof(IntelliScraper.Db.httpGetUrlType), (string)cmbUrlType.SelectedValue);
                this.rule.customUrl = txtCustomUrl.Text;
                return "Updated " + oldId;
            }           
        }
    }
}
