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
    /// Logica di interazione per Action.xaml
    /// </summary>
    public partial class Action : Page
    {
        bool isNew;
        IntelliScraper.Db.intelliScraperActionType type;
        ActionRules parent;
        IntelliScraper.Db.intelliScraperAction action;
        public Action(bool isNew, IntelliScraper.Db.intelliScraperActionType type, ActionRules parent, IntelliScraper.Db.intelliScraperAction action)
        {
            this.parent = parent;
            this.isNew = isNew;
            this.type = type;
            this.action = action;
            InitializeComponent();
            load();
        }

        public IntelliScraper.Db.intelliScraperAction getAction(IntelliScraper.Db.intelliScraperAction a)
        {
            if (a == null)            
                a = new IntelliScraper.Db.intelliScraperAction();

            if (a.input == null)
                a.input = new IntelliScraper.Db.intelliScraperActionInput();

            
            a.id = txtId.Text;

            if(cmbInputActionId.SelectedValue != null)
                a.input.actionId = (string)cmbInputActionId.SelectedValue;
            a.input.customInputValue = txtCustomVal.Text;

            if(cmbInputType.SelectedValue !=null)
                a.input.type = (IntelliScraper.Db.intelliScraperActionInputType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperActionInputType),(string)cmbInputType.SelectedValue);
           
            if(cmbRuleId.SelectedValue != null)
                a.ruleId = (string)cmbRuleId.SelectedValue;

            if(cmbType.SelectedValue != null)
                a.type = (IntelliScraper.Db.intelliScraperActionType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperActionType), (string)cmbType.SelectedValue);

            return a;
        }

        private void load()
        {
            //Load Types
            string[] types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionType));
            cmbType.Items.Clear();
            for (int i = 0; i < types.Length; i++)
            {
                cmbType.Items.Add(types[i]);

                IntelliScraper.Db.intelliScraperActionType t = (IntelliScraper.Db.intelliScraperActionType)Enum.Parse(typeof(IntelliScraper.Db.intelliScraperActionType), types[i]);
                if (t == this.type)
                    cmbType.SelectedIndex = i;
            }

            //Load input Types
            string[] InputTypes = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionInputType));
            cmbInputType.Items.Clear();
            foreach (string s in InputTypes)
                cmbInputType.Items.Add(s);

            //Load Action ids
            cmbInputActionId.Items.Clear();
            foreach (var x in Factory.Instance.i.actions)
            {
                if (isNew)
                    cmbInputActionId.Items.Add(x.id);
                else
                {
                    if(x.id != this.action.id)
                        cmbInputActionId.Items.Add(x.id);
                }
            }

            //Load rules
            cmbRuleId.Items.Clear();
            foreach (var x in Factory.Instance.getRuleIds())
                cmbRuleId.Items.Add(x);

            //Load Rule Typesù
            cmbRuleType.Items.Clear();
            string[] _types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionType));
            foreach (var t in _types)
            {               
                cmbRuleType.Items.Add(t);
            }
           

            //Enable disable items by type
            if (isNew)
                setFormByType(false,IntelliScraper.Db.intelliScraperActionType.none);
            else setFormByType(true, this.action.type);

            //set value for current action
            if (!this.isNew && this.action != null)
            {
                if (!string.IsNullOrEmpty(this.action.id))
                    txtId.Text = this.action.id;

                if (!string.IsNullOrEmpty(this.action.ruleId))
                    cmbRuleId.SelectedValue = this.action.ruleId;


                cmbType.SelectedValue = Enum.GetName(typeof(IntelliScraper.Db.intelliScraperActionType), this.action.type);

                if (this.action.input != null)
                {
                    cmbInputType.SelectedValue = Enum.GetName(typeof(IntelliScraper.Db.intelliScraperActionInputType), this.action.input.type);

                    if (!string.IsNullOrEmpty(this.action.input.actionId))
                        cmbInputActionId.SelectedValue = this.action.input.actionId;
                }

                txtId.IsEnabled = false;
                txtCustomVal.Text = this.action.input.customInputValue;
            }            
        }

      


        /// <summary>
        /// Set form type
        /// </summary>
        private void setFormByType(bool useCustomType,IntelliScraper.Db.intelliScraperActionType customType)
        {
            IntelliScraper.Db.intelliScraperActionType type = this.type;
            if (useCustomType)
                type = customType;
            
            if (type == IntelliScraper.Db.intelliScraperActionType.loadHtmlFromFile)            
                setActionControls(false, false, false, false, true, "custom", "Html File full path", true, true, false,false);
                          
            if (type == IntelliScraper.Db.intelliScraperActionType.httpGet)            
                setActionControls(true, false, false, false, true, "httpGet", "Url", false, true, false,true);
            
            if (type == IntelliScraper.Db.intelliScraperActionType.httpPost)
                setActionControls(true, true, true, false, false, "httpPost", "Custom Value", false, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.download)
                setActionControls(true, true, true, false, true, "download", "Custom file (http)", false, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.ftpPut)
                setActionControls(true, true, true, false, true, "ftpPut", "Custom file", true, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.loop_link)
                setActionControls(true, true, true, false, false, "loop_link", "Custom Value", false, true, true, true);


            if (type == IntelliScraper.Db.intelliScraperActionType.plugin)
                setActionControls(true, true, true, false, true, "plugin", "Plugin Name", true, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.save)
                setActionControls(true, true, true, false, false, "save", "Custom file", false, true, true, true);
            
            if (type == IntelliScraper.Db.intelliScraperActionType.screenShot)
                setActionControls(true, true, true, false, true, "screenShot", "Custom http page", true, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.upload)
                setActionControls(true, true, true, false, true, "upload", "Custom http page", true, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.xpathCollection)
                setActionControls(true, true, true, false, false, "xpathCollection", "Custom Value", false, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.xpathSingle)
                setActionControls(true, true, true, false, false, "xpathSingle", "Custom Value", false, true, true, true);

            if (type == IntelliScraper.Db.intelliScraperActionType.zip)
                setActionControls(true, true, true, false, false, "zip", "Custom Value", false, true, true, true);
        }


        /// <summary>
        /// Enable disable or hide/show some controls
        /// </summary>
        private void setActionControls(bool ruleIdEnabled,bool inputActionIdEnabled,bool inputTypeEnable,bool typeEnabled,bool customEnabled,string selectedValue,string lbcustomValueTxt,bool selectFileButtonEnabled,bool showTabItemAction,bool showTabItemRule,bool showButtonNewRule)
        {
            cmbRuleId.IsEnabled = ruleIdEnabled;
            cmbInputActionId.IsEnabled = inputActionIdEnabled;
            cmbInputType.IsEnabled = inputTypeEnable;
            cmbType.IsEnabled = typeEnabled;
            txtCustomVal.IsEnabled = customEnabled;
            cmbInputType.SelectedValue = selectedValue;
            lbCustomValue.Content = lbcustomValueTxt;
            btnSelectFile.IsEnabled = selectFileButtonEnabled;
            btnAddRule.IsEnabled = showButtonNewRule;
            cmbRuleType.IsEnabled = showButtonNewRule;

            if (this.parent != null)
            {
                if (!showTabItemAction)
                    this.parent.tabItem1.Visibility = System.Windows.Visibility.Hidden;
                else this.parent.tabItem1.Visibility = System.Windows.Visibility.Visible;

                if (!showTabItemRule)
                    this.parent.tabItem2.Visibility = System.Windows.Visibility.Hidden;
                else this.parent.tabItem2.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Select file
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                txtCustomVal.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// Add new Rule
        /// </summary>
        private void btnAddRule_Click(object sender, RoutedEventArgs e)
        {          
            this.parent.tabItem2.Visibility = System.Windows.Visibility.Visible;
            this.parent.tabControl1.SelectedIndex = 1;            
        }


        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            string msg = "Saved!";
            if (isNew)
            {
                IntelliScraper.Db.intelliScraperAction a = getAction(null);
                if (string.IsNullOrEmpty(a.id))
                {
                    MessageBox.Show("Action 'id' is mandatory","Error");
                    return;
                }
                else Factory.Instance.i.actions.Add(getAction(null));
                msg = "Added!";
            }
            else
            {
                IntelliScraper.Db.intelliScraperAction a = (from x in Factory.Instance.i.actions where x.id == txtId.Text select x).FirstOrDefault();
                if (a != null)
                    a = getAction(a);

                if (string.IsNullOrEmpty(a.id))
                {
                    MessageBox.Show("Action 'id' is mandatory", "Error");
                    return;
                }

            }

            

            Factory.Instance.Save();
            MainWindow.main.Status =msg;
        }


        /// <summary>
        /// Set rule
        /// </summary>
        private void cmbRuleId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbRuleId.SelectedValue != null)
            this.parent.LoadRuleFrame((string)cmbRuleId.SelectedValue);
        }
    }
}
