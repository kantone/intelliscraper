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

namespace IscraperBuilder.Controls.Common
{
    /// <summary>
    /// Logica di interazione per xpathAttribute.xaml
    /// </summary>
    public partial class xpathAttribute : Page
    {
        string attrId { get; set; }
        IntelliScraper.Db.xpathSingle rule { get; set; }
        IntelliScraper.Db.xpathSingleAttributes attr { get; set; }
        bool isFromXpathCollection { get; set; }
        IntelliScraper.Db.xpathCollection coll { get; set; }
        public xpathAttribute(string attrId,IntelliScraper.Db.xpathSingle rule,bool isFromXpathCollection,IntelliScraper.Db.xpathCollection coll)
        {
            this.isFromXpathCollection = isFromXpathCollection;
            this.coll = coll;
            this.rule = rule;
            this.attrId = attrId;
            InitializeComponent();
            load();
        }

        public void load()
        {
            cmbGrIds.Items.Clear();
            cmbGetType.Items.Clear();
            listBox2.Items.Clear();
            foreach (string v in Enum.GetNames(typeof(IntelliScraper.Db.xpathSingleAttributesGetType)))
                cmbGetType.Items.Add(v);

            //Load Process data
            List<string> ppId = new List<string>();
            foreach (var o in Factory.Instance.getAllPostProcessObj())
            {
                string _id = (string)o.GetType().GetProperty("id").GetValue(o, null);
                string type = (string)o.GetType().Name;
                string val = _id + " (" + type + ")";
                ppId.Add(val);
            }

            //Load Attributes Details
            if (this.rule != null)
            {
                if(rule.attributes != null)
                {
                    this.attr = (from x in rule.attributes where x.id == this.attrId select x).SingleOrDefault();
                    if (this.attr != null)
                    {
                        txtAttrName.Text = attr.attributeName;
                        txtId.Text = attr.id;
                        txtXpath.Text = attr.xpath;
                        cmbGetType.SelectedValue = attr.getType.ToString();
                        if (attr.getType.ToString() == "attribute")
                            txtAttrName.IsEnabled = true;

                        //remove alredy assigned rules and add to list
                        listBox1.Items.Clear();
                        if (attr.postProcessTrigger != null)
                        {
                            foreach (var pp in attr.postProcessTrigger)
                            {
                                listBox1.Items.Add(pp.id + " (" + pp.type.ToString() + ")");
                                string val = pp.id + " (" + pp.type.ToString() + ")";
                                ppId.Remove(val);
                            }
                        }

                        //Add Groups
                        if (attr.postProcessGroupId != null)
                        {
                            List<string> aviableGroupPpRules = new List<string>();
                            foreach (var r in rule.postProcessTriggerGroup)
                            {
                                cmbGrIds.Items.Add(r.id);
                            }


                            foreach (var s in attr.postProcessGroupId)
                            {
                                cmbGrIds.Items.Remove(s);
                                listBox2.Items.Add(s);
                            }
                           
                        }


                    }
                   
               }
            }

            cmbPp.Items.Clear();
            foreach (string p in ppId)
                cmbPp.Items.Add(p);

        }

        private void cmbGetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cmbGetType.SelectedValue == "attribute")
                txtAttrName.IsEnabled = true;
            else txtAttrName.IsEnabled = false;
        }

        private void cmbPp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPp.SelectedIndex >= 0 && cmbPp.SelectedValue != null)
            {
                listBox1.Items.Add((string)cmbPp.SelectedValue);

                cmbPp.Items.RemoveAt(cmbPp.SelectedIndex);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string val = (string)listBox1.Items.GetItemAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                cmbPp.Items.Add(val);
            }
        }

        private void cmbGrIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGrIds.SelectedIndex >= 0 && cmbGrIds.SelectedValue != null)
            {
                listBox2.Items.Add((string)cmbGrIds.SelectedValue);
                cmbGrIds.Items.RemoveAt(cmbGrIds.SelectedIndex);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                string val = (string)listBox2.SelectedValue;
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                cmbGrIds.Items.Add(val);
            }
        }

      
        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.attr.getType = (IntelliScraper.Db.xpathSingleAttributesGetType)Enum.Parse(typeof(IntelliScraper.Db.xpathSingleAttributesGetType), (string)cmbGetType.SelectedValue);
            this.attr.xpath = txtXpath.Text;
            this.attr.attributeName = txtAttrName.Text;


            if (listBox1.Items.Count > 0)
            {
                this.attr.postProcessTrigger = new IntelliScraper.Db.postProcessCollection();
                foreach (string i in listBox1.Items)
                {
                    int index = i.IndexOf('(');
                    if (index > 0)
                    {
                        string _type = i.Substring(index).Replace("(", "").Replace(")", "").Trim();
                        string id = i.Substring(0, index).Replace("(", "").Replace(")", "").Trim();

                        IntelliScraper.Db.postProcess pp = new IntelliScraper.Db.postProcess();
                        pp.id = id;
                        pp.type = (IntelliScraper.Db.postProcessType)Enum.Parse(typeof(IntelliScraper.Db.postProcessType),_type);
                        this.attr.postProcessTrigger.Add(pp);                       
                    }
                }
            }

            if (listBox2.Items.Count > 0)
            {
                this.attr.postProcessGroupId = new string[listBox2.Items.Count];
                for (int i = 0; i < listBox2.Items.Count; i++)
                    this.attr.postProcessGroupId[i] = (string)listBox2.Items[i];

            }

            foreach (var a in this.rule.attributes)
            {
                if (a.id == attr.id)
                {
                    attr = a;
                    break;
                }
            }

            if (isFromXpathCollection)
            {
                var val = (from x in coll.xpathSingle where x.id == rule.id select x).SingleOrDefault();
                if (val != null)
                    coll.xpathSingle.Remove(val);
                coll.xpathSingle.Add(this.rule);
            }

            Factory.Instance.Save();
            load();


        }
              
    }

    public class attrCombo
    {
        public string id { get; set; }
        public string type { get; set; }
    }
}
