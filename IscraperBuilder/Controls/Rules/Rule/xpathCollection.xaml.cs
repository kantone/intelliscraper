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
    /// Logica di interazione per xpathCollection.xaml
    /// </summary>
    public partial class xpathCollection : Page, Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        public IntelliScraper.Db.xpathCollection rule { get; set; }
        public xpathCollection(string id)
        {
            this.id = id;
            InitializeComponent();
            load();
        }

        public void load()
        {

            txtId.Text = id;
            if (Factory.Instance.i.rules.xpathCollection != null)
                this.rule = (from x in Factory.Instance.i.rules.xpathCollection where x.id == id select x).FirstOrDefault();

            if (this.rule != null)
            {
                cmbXpath.Items.Clear();
                foreach (string xpath in this.rule.xpath)                
                    cmbXpath.Items.Add(xpath);

                if (cmbXpath.Items.Count >= 0)
                    cmbXpath.SelectedIndex = 0;

                comboBox1.Items.Clear();
                foreach (var single in rule.xpathSingle)                
                    comboBox1.Items.Add(single.id);

                if (comboBox1.Items.Count >= 0)
                    comboBox1.SelectedIndex = 0;
                
            }
        }
        
        public string save()
        {
            throw new NotImplementedException();
        }

        private void cmbXpath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbXpath.SelectedValue != null)
                txtXpath.Text = (string) cmbXpath.SelectedValue;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                Controls.Common.xpathSingle s = new Common.xpathSingle();

                this.xpathSingle1.load((string)comboBox1.SelectedValue,true,rule);
                exp2.IsExpanded = true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddId.Text != string.Empty)
            {
                IntelliScraper.Db.xpathSingle s = new IntelliScraper.Db.xpathSingle();
                s.id = txtAddId.Text;
                rule.xpathSingle.Add(s);
                Factory.Instance.Save();
                load();
                comboBox1.SelectedValue = txtAddId.Text;
            }
            else MessageBox.Show("Please insert id!", "Error");
        }

        /// <summary>
        /// Add new Xpath
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (txtXpathNew.Text != string.Empty)
            {
                List<string> l = this.rule.xpath.ToList();
                l.Add(txtXpathNew.Text);
                this.rule.xpath = l.ToArray();
                Factory.Instance.Save();
                load();
                cmbXpath.SelectedValue = txtXpathNew.Text;
                txtXpath.Text = txtXpathNew.Text;                     
            }
            else MessageBox.Show("Please insert xpath value!", "Error");
        }


        /// <summary>
        /// Save xpath
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (txtXpath.Text != string.Empty)
            {
                List<string> xp = new List<string>();
                string oldXpathVal = (string) cmbXpath.SelectedValue;
                foreach (string s in rule.xpath)
                {
                    if(s == oldXpathVal)
                        xp.Add(txtXpath.Text);
                    else xp.Add(s);
                }

                rule.xpath = xp.ToArray();
                Factory.Instance.Save();
                load();
                cmbXpath.SelectedValue = txtXpath.Text;
                txtXpath.Text = txtXpath.Text;
            }
        }
    }
}
