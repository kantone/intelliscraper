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
    /// Logica di interazione per RulexpathSingle.xaml
    /// </summary>
    public partial class RulexpathSingle : Page,Rule.IRule
    {
         public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.xpathSingle rule { get; set; }
        public RulexpathSingle(string id)
        {
            InitializeComponent();
            this.xpathSingle1.load(id,false,null);

        }

        public string save()
        {
            throw new NotImplementedException();
        }
    }
}
