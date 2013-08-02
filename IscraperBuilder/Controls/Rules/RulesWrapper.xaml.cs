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
using IscraperBuilder.Controls.Rules.Rule;

namespace IscraperBuilder.Controls.Rules
{
    /// <summary>
    /// Logica di interazione per RulesWrapper.xaml
    /// </summary>
    public partial class RulesWrapper : Page
    {
        IRule rule { get; set; }
        public RulesWrapper()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
           
            cmbNewRule.Items.Clear();
           
            string[] types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionType));
            foreach (string t in types)
            {
                if(t != "loadHtmlFromFile" && t != "none")
                    cmbNewRule.Items.Add(t);
            }

         
        }

        /// <summary>
        /// On Select a Rule
        /// </summary>
        private void cmbRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /* MainWindow.main.Status = "";
            if (cmbRules.SelectedValue != null)
                rule = Factory.Instance.LoadRuleFrame((string)cmbRules.SelectedValue, this.frame1,false,null);*/
        }

        /// <summary>
        /// Ona add new Rule
        /// </summary>
        private void cmbNewRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /* MainWindow.main.Status = "";
            if (cmbNewRule.SelectedValue != null)
            {
                Type t = Factory.Instance.GetRuleTypeByName((string)cmbNewRule.SelectedValue);
                rule = Factory.Instance.LoadRuleFrame(null, this.frame1, true, t);
            }*/
        }

        /// <summary>
        /// Add
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (cmbNewRule.SelectedValue != null && !string.IsNullOrEmpty(txtId.Text))
            {
                IntelliScraper.Db.intelliScraperActionType t = (IntelliScraper.Db.intelliScraperActionType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperActionType), (string)cmbNewRule.SelectedValue);
                if (t == IntelliScraper.Db.intelliScraperActionType.download)
                    frame1.Navigate(new Controls.Rules.Rule.ruleDownload(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.ftpPut)
                    frame1.Navigate(new Controls.Rules.Rule.ruleFtpPut(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.httpGet)
                    frame1.Navigate(new Controls.Rules.RuleHttpGet(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.httpPost)
                    frame1.Navigate(new Controls.Rules.Rule.RuleHttpPost(txtId.Text));

               /* if (t == IntelliScraper.Db.intelliScraperActionType.loadHtmlFromFile)
                    frame1.Navigate(new Controls.Rules.(txtId.Text));*/

                if (t == IntelliScraper.Db.intelliScraperActionType.linksPageGenerator)
                    frame1.Navigate(new Controls.Rules.Rule.RuleLinksPageGenerator(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.plugin)
                    frame1.Navigate(new Controls.Rules.Rule.rulePlugin(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.save)
                    frame1.Navigate(new Controls.Rules.Rule.ruleSave(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.screenShot)
                    frame1.Navigate(new Controls.Rules.Rule.ruleScreenShot(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.upload)
                    frame1.Navigate(new Controls.Rules.Rule.ruleUpload(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.xpathCollection)
                    frame1.Navigate(new Controls.Rules.Rule.xpathCollection(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.xpathSingle)
                    frame1.Navigate(new Controls.Rules.Rule.RulexpathSingle(txtId.Text));

                if (t == IntelliScraper.Db.intelliScraperActionType.zip)
                    frame1.Navigate(new Controls.Rules.Rule.ruleZip(txtId.Text));
            }
            else MessageBox.Show("Id & type are mandatory!", "Error");

            /*if (this.rule != null)
            {
                try
                {
                    MainWindow.main.Status = this.rule.save();
                    load();
                }
                catch (Exception ex)
                {
                    MainWindow.main.Status = "Error : " + ex.Message;
                }
            }*/
        }

        /// <summary>
        /// Delete
        /// </summary>
       
        private void button2_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void removeRuleByName()
        {
            List<object> objs =  Factory.Instance.getAllObj();
           
        }
    
    }
}
