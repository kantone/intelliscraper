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

namespace IscraperBuilder.Controls.PostProcess
{
    /// <summary>
    /// Logica di interazione per SubString.xaml
    /// </summary>
    public partial class SubString : Page
    {
        IntelliScraper.Db.substring rule { get; set; }
        public SubString(IntelliScraper.Db.substring rule)
        {
            this.rule = rule;
            InitializeComponent();
            load();

            txtSSimple.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSfrom.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSTo.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
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


        void load()
        {
            if (rule != null)
            {
                txtId.Text = rule.id;

                cmbType.Items.Clear();
                foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.substringType)))
                    cmbType.Items.Add(s);

                txtSSimple.Text = rule.substring1.ToString();
                txtSfrom.Text = rule.substringFrom.ToString();
                txtSTo.Text = rule.substringTo.ToString();
                txtSsearch.Text = rule.substringSearch;

                chkCheclLast.IsChecked = rule.searchLast;

                cmbType.SelectedValue = rule.type.ToString();

                cmbType_SelectionChanged(this, null);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbType.SelectedValue != null)
            {
                txtSSimple.IsEnabled = false;
                txtSfrom.IsEnabled = false;
                txtSTo.IsEnabled = false;
                txtSsearch.IsEnabled = false;

                IntelliScraper.Db.substringType t = (IntelliScraper.Db.substringType)Enum.Parse(typeof(IntelliScraper.Db.substringType), (string)cmbType.SelectedValue);
                if (t == IntelliScraper.Db.substringType.fromTo)
                {
                    txtSfrom.IsEnabled = true;
                    txtSTo.IsEnabled = true;
                }

                if (t == IntelliScraper.Db.substringType.simple)
                {
                    txtSSimple.IsEnabled = true;
                }

                if (t == IntelliScraper.Db.substringType.searchTo)
                {
                    txtSsearch.IsEnabled = true;
                    txtSfrom.IsEnabled = true;
                    txtSTo.IsEnabled = true;
                }

                if (t == IntelliScraper.Db.substringType.search)
                {
                    txtSsearch.IsEnabled = true;                  
                }

                if (t == IntelliScraper.Db.substringType.searchFrom)
                {
                    txtSsearch.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            rule.searchLast = (bool)chkCheclLast.IsChecked;
            rule.substring1= Int32.Parse( txtSSimple.Text);
            rule.substringFrom= Int32.Parse( txtSfrom.Text);
            rule.substringTo = Int32.Parse(txtSTo.Text);
            rule.substringSearch = txtSsearch.Text;
            if(cmbType.SelectedValue != null)
                rule.type = (IntelliScraper.Db.substringType)Enum.Parse(typeof(IntelliScraper.Db.substringType), (string)cmbType.SelectedValue);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            richTextBox2.Document.Blocks.Clear();
            string myText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;
            string txt = IntelliScraper.Scrape.PostProcessUtils.substring(myText, rule);
            richTextBox2.AppendText(txt);
        }
    }
}
