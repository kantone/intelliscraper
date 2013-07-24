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
using IscraperBuilder.Controls.Rules;

namespace IscraperBuilder.Controls.Common
{
    /// <summary>
    /// Logica di interazione per xpathSingle.xaml
    /// </summary>
    public partial class xpathSingle : UserControl
    {
        IntelliScraper.Db.xpathSingle rule { get; set; }
       // bool isNew = false;
        string id { get; set; }
        bool isFromCollection { get; set; }
        IntelliScraper.Db.xpathCollection coll { get; set; }
        public xpathSingle()
        {
            InitializeComponent();
        }

        public void load(string id,bool isFromCollection,IntelliScraper.Db.xpathCollection coll)
        {
            this.id = id;
            this.isFromCollection = isFromCollection;
            this.coll = coll;
            treeView1.Items.Clear();
            if (!string.IsNullOrEmpty(id))
            {
                this.id = id;
                if (!isFromCollection)
                {
                    if (Factory.Instance.i.rules.xpathSingle != null)
                        this.rule = (from x in Factory.Instance.i.rules.xpathSingle where x.id == id select x).FirstOrDefault();
                }
                else
                {
                    if(coll != null && coll.xpathSingle != null)
                         this.rule  =  (from x in coll.xpathSingle where x.id == id select x).FirstOrDefault();
                }
            }

            if (this.rule != null)
            {
                
                this.ruleName.Text = rule.id;
                TreeViewItem attributesT = new TreeViewItem();
                attributesT.Header = "Attributes";
                attributesT.IsExpanded = true;


                if (rule.attributes != null)
                {
                    foreach (IntelliScraper.Db.xpathSingleAttributes attr in rule.attributes)
                    {
                        TreeViewItem i = new TreeViewItem();
                        i.Header = attr.id;
                        attributesT.Items.Add(i);
                        i.MouseUp += new MouseButtonEventHandler(i_MouseUp);
                        i.ContextMenu = Resources["cMenu"] as ContextMenu;

                    }                    
                }
                treeView1.Items.Add(attributesT);

                
                if (rule.postProcessTriggerGroup != null)
                {
                    TreeViewItem ppGr = new TreeViewItem();
                    ppGr.Header = "Post Process Group Rule";
                    ppGr.IsExpanded = true;
                    foreach (var r in rule.postProcessTriggerGroup)
                    {
                        TreeViewItem piT = new TreeViewItem();
                        piT.Header = r.id;
                        piT.Tag = "groupPP";
                        ppGr.Items.Add(piT);
                        piT.MouseUp += new MouseButtonEventHandler(piT_MouseUp);
                        piT.ContextMenu = Resources["grMenu"] as ContextMenu;
                    }
                    treeView1.Items.Add(ppGr);
                }
               
            }

            //Load Process data
            List<string> ppId = new List<string>();
            foreach (var o in Factory.Instance.getAllPostProcessObj())
            {
                string _id = (string)o.GetType().GetProperty("id").GetValue(o, null);
              
            }

            
        }

        void piT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem i = e.Source as TreeViewItem;
            string id = (string)i.Header;
            xpathAttributeGroupPP attr = new xpathAttributeGroupPP(id, this.rule);
            this.frame1.Navigate(attr);
        }

        void i_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem i = e.Source as TreeViewItem;
            string id = (string)i.Header;
            xpathAttribute attr = new xpathAttribute(id, this.rule, isFromCollection, coll);
            this.frame1.Navigate(attr);

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (this.rule.attributes == null)
                    this.rule.attributes = new IntelliScraper.Db.xpathSingleAttributesCollection();

                IntelliScraper.Db.xpathSingleAttributes attr = new IntelliScraper.Db.xpathSingleAttributes();
                attr.id = textBox1.Text;
                this.rule.attributes.Add(attr);
                Factory.Instance.Save();
                load(this.id, isFromCollection, coll);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            if (mi != null)
            {
                 ContextMenu cm = mi.CommandParameter as ContextMenu;
                 if (cm != null)
                 {
                     TreeViewItem h = cm.PlacementTarget as TreeViewItem;
                     if (h != null)
                     {
                         string attrId = (string)h.Header;
                         int i=0;
                         foreach(var x in this.rule.attributes){
                             if(x.id == attrId){
                                 this.rule.attributes.RemoveAt(i);
                                 Factory.Instance.Save();
                                 load(id, isFromCollection, coll);
                                 break;
                             }
                             i++;
                         }
                     }
                 }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (this.rule.postProcessTriggerGroup == null)
                    this.rule.postProcessTriggerGroup = new IntelliScraper.Db.xpathSinglePostProcessTriggerGroupCollection();

                IntelliScraper.Db.xpathSinglePostProcessTriggerGroup gr = new  IntelliScraper.Db.xpathSinglePostProcessTriggerGroup();
                gr.id = textBox2.Text;
                this.rule.postProcessTriggerGroup.Add(gr);
                Factory.Instance.Save();
                load(id, isFromCollection, coll);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TreeViewItem h = cm.PlacementTarget as TreeViewItem;
                    if (h != null)
                    {
                        string id = (string) h.Header;

                        //check if this rule is not used
                        if (this.rule.attributes != null)
                        {
                            bool isUsed = (from x in this.rule.attributes where x.postProcessGroupId != null && x.postProcessGroupId.Contains(id) select x).Any();
                            if (isUsed)
                            {
                                MessageBox.Show(string.Format("Cannot delete! {0} is used by one or more attributes!", id), "Error");
                                return;
                            }

                            else
                            {
                                if (MessageBox.Show(string.Format("Delete {0}", id), "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    foreach (var r in this.rule.postProcessTriggerGroup)
                                    {
                                        if (r.id == id)
                                        {
                                            this.rule.postProcessTriggerGroup.Remove(r);
                                            Factory.Instance.Save();
                                            load(this.id, isFromCollection, coll);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
