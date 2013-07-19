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
    /// Logica di interazione per ruleScreenShot.xaml
    /// </summary>
    public partial class ruleScreenShot : Page,IRule
    {      
         public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.screenShot rule { get; set; }
        public ruleScreenShot(string id)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.screenShot where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            txtH.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtW.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            load();
        }

        /// <summary>
        /// Only Numbers
        /// </summary>       
        void txtThreadNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }


        private void load()
        {
            cmbInputType.Items.Clear();
            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.screenShotInputType)))
                cmbInputType.Items.Add(s);

            if (rule != null)
            {
                txtId.Text = rule.id;
                txtW.Text = rule.width.ToString();
                txtH.Text = rule.height.ToString();
                txtInputAttrKey.Text = rule.inputAttributeKey;
                txtCustomUrl.Text = rule.customUrl;
                txtSaveTo.Text = rule.saveTo;

                if(rule.ImgConvertAction != null)
                    imageConvert1.load(rule.ImgConvertAction);

                cmbInputType.SelectedValue = rule.inputType.ToString();
                cmbInputType_SelectionChanged(this, null);
            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }


        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.screenShotInputType t = (IntelliScraper.Db.screenShotInputType)Enum.Parse(typeof(IntelliScraper.Db.screenShotInputType), (string)cmbInputType.SelectedValue);
                if (t == IntelliScraper.Db.screenShotInputType.custom)
                {
                    txtInputAttrKey.IsEnabled = false;
                }
                if (t == IntelliScraper.Db.screenShotInputType.fromInput)
                {
                    txtInputAttrKey.IsEnabled = true;
                }

            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            rule.id = txtId.Text;
            rule.width = Int32.Parse(txtW.Text);
            rule.height = Int32.Parse(txtH.Text);
            rule.inputAttributeKey = txtInputAttrKey.Text;
            rule.customUrl = txtCustomUrl.Text;
            rule.saveTo = txtSaveTo.Text;

            if (cmbInputType.SelectedValue != null)
                rule.inputType = (IntelliScraper.Db.screenShotInputType)Enum.Parse(typeof(IntelliScraper.Db.screenShotInputType), (string)cmbInputType.SelectedValue);

            imageConvert1.save();
            Factory.Instance.Save();
            load();

        }
    }
}
