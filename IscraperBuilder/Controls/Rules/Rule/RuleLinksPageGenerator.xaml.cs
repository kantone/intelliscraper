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
    /// Logica di interazione per RuleLinksPageGenerator.xaml
    /// </summary>
    public partial class RuleLinksPageGenerator : Page, Rule.IRule
    {
        public bool isNew { get; set; }
        IntelliScraper.Db.linksPageGenerator rule { get; set; }
        public string id { get; set; }
        public RuleLinksPageGenerator(string id)
        {
            
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.linksPageGenerator where x.id == id select x).FirstOrDefault();
            }
        }
                

        public string save()
        {
            throw new NotImplementedException();
        }

      
    }
}
