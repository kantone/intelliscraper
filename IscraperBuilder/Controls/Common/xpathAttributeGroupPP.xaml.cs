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
    /// Logica di interazione per xpathAttributeGroupPP.xaml
    /// </summary>
    public partial class xpathAttributeGroupPP : Page
    {
        string attrId { get; set; }
        IntelliScraper.Db.xpathSingle rule { get; set; }
        IntelliScraper.Db.xpathSingleAttributes attr { get; set; }
        public xpathAttributeGroupPP(string attrId,IntelliScraper.Db.xpathSingle rule)       
        {
            this.attrId = attrId;
            this.rule = rule;
            InitializeComponent();
            load();
        }

        public void load()
        {
            listBox1.Items.Clear();
            textBlock1.Text = attrId;

            //Load Process data
            List<string> ppId = new List<string>();
            foreach (var o in Factory.Instance.getAllPostProcessObj())
            {
                string _id = (string)o.GetType().GetProperty("id").GetValue(o, null);
                string type = (string)o.GetType().Name;
                string val = _id + " (" + type + ")";
                ppId.Add(val);
            }


            if (this.rule != null)
            {
                               
                if (this.rule.postProcessTriggerGroup != null)
                {
                    var gr = (from x in this.rule.postProcessTriggerGroup where x.id == this.attrId select x).FirstOrDefault();
                    if (gr != null)
                    {
                        if (gr.postProcessTrigger != null)
                        {
                            foreach (var pp in gr.postProcessTrigger)
                            {
                                listBox1.Items.Add(pp.id + " (" + pp.type.ToString() + ")");
                                string val = pp.id + " (" + pp.type.ToString() + ")";
                                ppId.Remove(val);
                            }
                        }
                    }
                }
            }

            cmbPp.Items.Clear();
            foreach (string p in ppId)
                cmbPp.Items.Add(p);
          

        }
        
        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string val = (string)listBox1.Items.GetItemAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                cmbPp.Items.Add(val);
            }
        }

        private void cmbPp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPp.SelectedIndex >= 0 && cmbPp.SelectedValue != null)
            {
                listBox1.Items.Add((string)cmbPp.SelectedValue);

                cmbPp.Items.RemoveAt(cmbPp.SelectedIndex);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (listBox1.Items.Count > 0)
            {
                //this.rule.postProcessTriggerGroup = new IntelliScraper.Db.xpathSinglePostProcessTriggerGroupCollection();
                IntelliScraper.Db.xpathSinglePostProcessTriggerGroup gr = (from x in this.rule.postProcessTriggerGroup where x.id == this.attrId select x).FirstOrDefault();
                gr.postProcessTrigger = new IntelliScraper.Db.postProcessCollection();
                foreach (string i in listBox1.Items)
                {
                    int index = i.IndexOf('(');
                    if (index > 0)
                    {
                        string _type = i.Substring(index).Replace("(", "").Replace(")", "").Trim();
                        string id = i.Substring(0, index).Replace("(", "").Replace(")", "").Trim();

                        IntelliScraper.Db.postProcess pp = new IntelliScraper.Db.postProcess();
                        pp.id = id;

                        if (_type.ToLower() == "regularexpression")
                            _type = "regex";
                        pp.type = (IntelliScraper.Db.postProcessType)Enum.Parse(typeof(IntelliScraper.Db.postProcessType), _type);

                        gr.postProcessTrigger.Add(pp);
                    }
                }
              

                Factory.Instance.Save();
            }
        }
    }
}
