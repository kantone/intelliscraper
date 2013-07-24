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
    /// Logica di interazione per rulePlugin.xaml
    /// </summary>
    public partial class rulePlugin : Page,IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.plugin rule { get; set; }
        public rulePlugin(string id)
        {
            inizialize(id);
        }

        void inizialize(string id)
        {
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.plugin where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            load();
        }

        private void load()
        {
            cmbInputType.Items.Clear();

            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.pluginInputType)))
                cmbInputType.Items.Add(s);

            cmbPlugins.Items.Clear();
            IntelliScraper.Plugin.PluginManager p = new IntelliScraper.Plugin.PluginManager();
            if (p.plugins != null)
            {
                foreach (var pl in p.plugins)
                    cmbPlugins.Items.Add(pl.Key);
            }

            if (rule != null)
            {
                if (rule.ImgConvertAction == null)
                {
                    rule.ImgConvertAction = new IntelliScraper.Db.imageConvert();
                    rule.ImgConvertAction.id = rule.id + "-im1";
                }

                txtId.Text = rule.id;
                txtCustomInput.Text = rule.customInput;
                imageConvert1.load(rule.ImgConvertAction);
                txtInputAttrKey.Text =  rule.inputAttributeKey;
                cmbInputType.SelectedValue = rule.inputType.ToString();
                cmbPlugins.SelectedValue = rule.pluginName;
                cmbInputType_SelectionChanged(this, null);
            }
        }

        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.pluginInputType t = (IntelliScraper.Db.pluginInputType)Enum.Parse(typeof(IntelliScraper.Db.pluginInputType), (string)cmbInputType.SelectedValue);
                if (t == IntelliScraper.Db.pluginInputType.custom)
                {
                    txtInputAttrKey.IsEnabled = false;
                }
                if (t == IntelliScraper.Db.pluginInputType.fromInput)
                {
                    txtInputAttrKey.IsEnabled = true;
                }

            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlugins.SelectedValue != null)
                rule.pluginName = (string)cmbPlugins.SelectedValue;

            if (cmbInputType.SelectedValue != null)            
                rule.inputType = (IntelliScraper.Db.pluginInputType)Enum.Parse(typeof(IntelliScraper.Db.pluginInputType), (string)cmbInputType.SelectedValue);
            
            rule.customInput = txtCustomInput.Text;
            rule.inputAttributeKey = txtInputAttrKey.Text;

            imageConvert1.save();

            Factory.Instance.Save();
            inizialize(id); 
        }
    }
}
