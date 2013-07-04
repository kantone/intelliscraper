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

            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {                
                isNew = false;
                this.id = id;
                if(Factory.Instance.i.rules.xpathSingle != null)
                    rule = (from x in Factory.Instance.i.rules.xpathSingle where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();



            load();
        }

        private void load()
        {
            foreach (var attr in rule.attributes)
            {
                TreeViewItem i = new TreeViewItem();
                i.Header = attr.id;

                TreeViewItem tmp = new TreeViewItem();
                tmp.Header = string.Format("Xpath : {0}", attr.xpath);
                i.Items.Add(tmp);

                tmp = new TreeViewItem();
                tmp.Header = string.Format("Type : {0}", Enum.GetName(typeof(IntelliScraper.Db.xpathSingleAttributesGetType),attr.getType));
                i.Items.Add(tmp);

                TreeViewItem trigger = new TreeViewItem();
                trigger.Header = "Post Process Data";
                trigger.IsExpanded = true;
                foreach (var trig in attr.postProcessTrigger)
                {
                    TreeViewItem t = new TreeViewItem();                    
                    t.Header = string.Format("Id : {0}", trig.id);

                    tmp = new TreeViewItem();
                    tmp.Header = string.Format("Type : {0}", Enum.GetName(typeof(IntelliScraper.Db.postProcessType),trig.type));
                    t.Items.Add(tmp);

                    trigger.Items.Add(t);
                }

                
                i.Items.Add(trigger);
                i.IsExpanded = true;
                treeView1.Items.Add(i);
                
            }
           
        }


        public string save()
        {
            throw new NotImplementedException();
        }
    }
}
