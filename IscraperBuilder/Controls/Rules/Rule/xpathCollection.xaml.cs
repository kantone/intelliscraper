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

            lbId.Content = id;
            if (Factory.Instance.i.rules.xpathCollection != null)
                this.rule = (from x in Factory.Instance.i.rules.xpathCollection where x.id == id select x).FirstOrDefault();

            if (this.rule != null)
            {
                txtXpath.Text = rule.xpath;
                xpathSingle1.load(rule.id, true, new IntelliScraper.Db.xpathCollection());
                
            }
        }
        
        public string save()
        {
            throw new NotImplementedException();
        }


       

        /// <summary>
        /// Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            rule.xpath = txtXpath.Text;
            rule.xpathSingle = xpathSingle1.rule;
            Factory.Instance.Save();
        }
                

       
    }
}
