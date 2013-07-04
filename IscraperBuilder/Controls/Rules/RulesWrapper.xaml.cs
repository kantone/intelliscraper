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
            cmbRules.Items.Clear();
            foreach (var x in Factory.Instance.getAllRulesId(Factory.Instance.getAllObj()))            
                cmbRules.Items.Add((string)x);
            
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
            MainWindow.main.Status = "";
            if (cmbRules.SelectedValue != null)
                rule = Factory.Instance.LoadRuleFrame((string)cmbRules.SelectedValue, this.frame1,false,null);
        }

        /// <summary>
        /// Ona add new Rule
        /// </summary>
        private void cmbNewRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.main.Status = "";
            if (cmbNewRule.SelectedValue != null)
            {
                Type t = Factory.Instance.GetRuleTypeByName((string)cmbNewRule.SelectedValue);
                rule = Factory.Instance.LoadRuleFrame(null, this.frame1, true, t);
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.rule != null)
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
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
       
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRules.SelectedValue != null)
            {
                Factory.Instance.deleteRuleByTypeAndIndex((string)cmbRules.SelectedValue);
                Factory.Instance.Save();
                MainWindow.main.Status = "Delete " + (string)cmbRules.SelectedValue;
                load();
                frame1.Navigate(null);//// = null;
            }
        }

        private void removeRuleByName()
        {
            List<object> objs =  Factory.Instance.getAllObj();
           
        }
    
    }
}
