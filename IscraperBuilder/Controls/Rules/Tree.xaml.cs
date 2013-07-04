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
    /// Logica di interazione per Tree.xaml
    /// </summary>
    public partial class Tree : Page
    {
        public Tree()
        {
            InitializeComponent();
            LoadTree();
        }


        private void LoadTree()
        {
            TreeviewH actions = new TreeviewH("Actions");
          
            if (Factory.Instance.i.actions != null)
            {
                List<object> rules = Factory.Instance.getAllObj();                  

                foreach (var a in Factory.Instance.i.actions)
                {
                    TreeviewH singleAction = new TreeviewH(a.id);
                    singleAction.FontWeight = FontWeights.Bold;

                    string inputId = "none";
                    if (a.input != null)
                    {                        
                        if (!string.IsNullOrEmpty(a.input.actionId))
                            inputId = a.input.actionId;
                    }
                    singleAction.Items.Add(TreeviewH.getTreeWithImage(string.Format("Type ({0})", Enum.GetName(typeof(IntelliScraper.Db.intelliScraperActionType), a.type)), TreeviewH.TreeType.action, "043.png", a.id));
                    singleAction.Items.Add(TreeviewH.getTreeWithImage(string.Format("Input data ({0})", inputId), TreeviewH.TreeType.actionInput, "050.png", a.id));
                                      
                    
                    if(!string.IsNullOrEmpty(a.ruleId))
                    {
                        StackPanel p = TreeviewH.getTreeWithImage(string.Format("Rule '{0}'", a.ruleId), TreeviewH.TreeType.rule, "051.png", a.id);
                       
                       

                        foreach (object o in rules)
                        {
                            string id = (string)o.GetType().GetProperty("id").GetValue(o, null);
                            if (!string.IsNullOrEmpty(id))
                            {
                                if (id == a.ruleId)
                                {
                                    if(o.GetType() == typeof(IntelliScraper.Db.httpGet))
                                    {
                                        IntelliScraper.Db.httpGet i = (IntelliScraper.Db.httpGet)o;
                                        p.Children.Add(new TreeviewH(i.id));
                                    }
                                    if (o.GetType() == typeof(IntelliScraper.Db.xpathCollection))
                                    {
                                        IntelliScraper.Db.xpathCollection i = (IntelliScraper.Db.xpathCollection)o;
                                        p.Children.Add(new TreeviewH(i.id));
                                    }

                                    break;
                                }
                            }
                        }
                        singleAction.Items.Add(p);
                        /*
                          if (Factory.Instance.i.rules.actionZip != null)
                ids.AddRange(Factory.Instance.i.rules.actionZip.ToList<object>());

            if (Factory.Instance.i.rules.download != null)
                ids.AddRange(Factory.Instance.i.rules.download.ToList<object>());

            if (Factory.Instance.i.rules.ftpPut != null)
                ids.AddRange(Factory.Instance.i.rules.ftpPut.ToList<object>());

            if (Factory.Instance.i.rules.httpGet != null)
                ids.AddRange(Factory.Instance.i.rules.httpGet.ToList<object>());

            if (Factory.Instance.i.rules.httpPost != null)
                ids.AddRange(Factory.Instance.i.rules.httpPost.ToList<object>());

            if (Factory.Instance.i.rules.loop_link != null)
                ids.AddRange(Factory.Instance.i.rules.loop_link.ToList<object>());

            if (Factory.Instance.i.rules.plugin != null)
                ids.AddRange(Factory.Instance.i.rules.plugin.ToList<object>());

            if (Factory.Instance.i.rules.save != null)
                ids.AddRange(Factory.Instance.i.rules.save.ToList<object>());

            if (Factory.Instance.i.rules.screenShot != null)
                ids.AddRange(Factory.Instance.i.rules.screenShot.ToList<object>());

            if (Factory.Instance.i.rules.upload != null)
                ids.AddRange(Factory.Instance.i.rules.upload.ToList<object>());

            if (Factory.Instance.i.rules.xpathCollection != null)
                ids.AddRange(Factory.Instance.i.rules.xpathCollection.ToList<object>());

            if (Factory.Instance.i.rules.xpathSingle != null)
                ids.AddRange(Factory.Instance.i.rules.xpathSingle.ToList<object>());*/

                        IscraperBuilder.Controls.Rules.Rule.IRule rule = Factory.Instance.getRuleById(a.ruleId,false,null);
                        if (rule != null)
                        {
                            Type x =   rule.GetType();
                        }
                    }

                    actions.Items.Add(singleAction);
                }
            }

            treeView1.Items.Add(actions);
        }
    }
}
