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
        ActRuleTabFrame parentT;
        IntelliScraper.Db.intelliScraperAction action;
        private void LoadAction(bool isNew, IntelliScraper.Db.intelliScraperActionType type, ActionRules parent, ActRuleTabFrame parentT, IntelliScraper.Db.intelliScraperAction action)
        {
            this.parent = parent;
            this.parentT = parentT;
            this.isNew = isNew;
            this.type = type;
            this.action = action;
            InitializeComponent();
            load();
        }
        public Action(bool isNew, IntelliScraper.Db.intelliScraperActionType type, ActionRules parent, IntelliScraper.Db.intelliScraperAction action)
        {
            LoadAction(isNew, type, parent, null, action);           
        }
        public Action(bool isNew, IntelliScraper.Db.intelliScraperActionType type, ActRuleTabFrame parentT, IntelliScraper.Db.intelliScraperAction action)
        {
            LoadAction(isNew, type, null, parentT, action);           
        }
        public Action(bool isNew, IntelliScraper.Db.intelliScraperActionType type,  IntelliScraper.Db.intelliScraperAction action)
        {
            LoadAction(isNew, type, null, null, action);
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
          /*  cmbRuleId.Items.Clear();
            foreach (var x in Factory.Instance.getRuleIds())
                cmbRuleId.Items.Add(x);*/
           

            //Load Rule Typesù
            /*cmbRuleType.Items.Clear();
            string[] _types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionType));
            foreach (var t in _types)
            {               
                cmbRuleType.Items.Add(t);
            }*/
           

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
            else
            {
                if (this.action != null)
                {
                    cmbInputActionId.SelectedValue = this.action.id;
                    cmbInputActionId.IsEnabled = false;
                }
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
            //Create new Rule
            object g = null;
            Type t = Type.GetType("IntelliScraper.Db." + (string)cmbType.SelectedValue + ", IntelliScraper");
           

            if (t == typeof(IntelliScraper.Db.httpGet))
            {                
                g = new IntelliScraper.Db.httpGet();
                if (Factory.Instance.i.rules.httpGet == null)
                    Factory.Instance.i.rules.httpGet = new IntelliScraper.Db.httpGetCollection();
                Factory.Instance.i.rules.httpGet.Add(g as IntelliScraper.Db.httpGet);
            }

            if (t == typeof(IntelliScraper.Db.httpPost))
            {
                g = new IntelliScraper.Db.httpPost();
                if (Factory.Instance.i.rules.httpPost == null)
                    Factory.Instance.i.rules.httpPost = new IntelliScraper.Db.httpPostCollection();
                Factory.Instance.i.rules.httpPost.Add(g as IntelliScraper.Db.httpPost);
           
            }

            if (t == typeof(IntelliScraper.Db.xpathSingle))
            {
                g = new IntelliScraper.Db.xpathSingle();
                if (Factory.Instance.i.rules.xpathSingle == null)
                    Factory.Instance.i.rules.xpathSingle = new IntelliScraper.Db.xpathSingleCollection();              
                Factory.Instance.i.rules.xpathSingle.Add(g as IntelliScraper.Db.xpathSingle);
            }

            if (t == typeof(IntelliScraper.Db.xpathCollection))
            {
                g = new IntelliScraper.Db.xpathCollection();
                if (Factory.Instance.i.rules.xpathCollection == null)
                    Factory.Instance.i.rules.xpathCollection = new IntelliScraper.Db.xpathCollectionCollection();
                Factory.Instance.i.rules.xpathCollection.Add(g as IntelliScraper.Db.xpathCollection);
            }

            if (t == typeof(IntelliScraper.Db.loop_link))
            {
                g = new IntelliScraper.Db.loop_link();
                if (Factory.Instance.i.rules.loop_link == null)
                    Factory.Instance.i.rules.loop_link = new IntelliScraper.Db.loop_linkCollection();
                Factory.Instance.i.rules.loop_link.Add(g as IntelliScraper.Db.loop_link);
            }

            if (t == typeof(IntelliScraper.Db.download))
            {
                g = new IntelliScraper.Db.download();
                if (Factory.Instance.i.rules.download == null)
                    Factory.Instance.i.rules.download = new IntelliScraper.Db.downloadCollection();
                Factory.Instance.i.rules.download.Add(g as IntelliScraper.Db.download);
            }

            if (t == typeof(IntelliScraper.Db.upload))
            {
                g = new IntelliScraper.Db.upload();
                if (Factory.Instance.i.rules.upload == null)
                    Factory.Instance.i.rules.upload = new IntelliScraper.Db.uploadCollection();
                Factory.Instance.i.rules.upload.Add(g as IntelliScraper.Db.upload);
            }

            if (t == typeof(IntelliScraper.Db.ftpPut))
            {
                g = new IntelliScraper.Db.ftpPut();
                if (Factory.Instance.i.rules.ftpPut == null)
                    Factory.Instance.i.rules.ftpPut = new IntelliScraper.Db.ftpPutCollection();
                Factory.Instance.i.rules.ftpPut.Add(g as IntelliScraper.Db.ftpPut);
            }

            if (t == typeof(IntelliScraper.Db.actionZip))
            {
                g = new IntelliScraper.Db.actionZip();
                if (Factory.Instance.i.rules.actionZip == null)
                    Factory.Instance.i.rules.actionZip = new IntelliScraper.Db.actionZipCollection();
                Factory.Instance.i.rules.actionZip.Add(g as IntelliScraper.Db.actionZip);
            }

            if (t == typeof(IntelliScraper.Db.save))
            {
                g = new IntelliScraper.Db.save();
                if (Factory.Instance.i.rules.save == null)
                    Factory.Instance.i.rules.save = new IntelliScraper.Db.saveCollection();
                Factory.Instance.i.rules.save.Add(g as IntelliScraper.Db.save);
            }

            if (t == typeof(IntelliScraper.Db.screenShot))
            {
                g = new IntelliScraper.Db.screenShot();
                if (Factory.Instance.i.rules.screenShot == null)
                    Factory.Instance.i.rules.screenShot = new IntelliScraper.Db.screenShotCollection();
                Factory.Instance.i.rules.screenShot.Add(g as IntelliScraper.Db.screenShot);
            }

            if (t == typeof(IntelliScraper.Db.plugin))
            {
                g = new IntelliScraper.Db.plugin();
                if (Factory.Instance.i.rules.plugin == null)
                    Factory.Instance.i.rules.plugin = new IntelliScraper.Db.pluginCollection();
                Factory.Instance.i.rules.plugin.Add(g as IntelliScraper.Db.plugin);
            }

            string id = g.GetType().Name + "-" + Utils.RandomUtil.RandomString(4);
            g.GetType().GetProperty("id").SetValue(g, id, null);

            

            Factory.Instance.Save();
            cmbRuleId.Items.Add(id);
            cmbRuleId.SelectedValue = id;

            if (this.parent != null)
            {
                Factory.Instance.LoadRuleFrame(id, this.parent.frame2, false, null);
                this.parent.tabItem2.Visibility = System.Windows.Visibility.Visible;
                this.parent.tabControl1.SelectedIndex = 1;
                this.parent.tabItem2.Header = id;
                this.parent.tabControl1.SelectedIndex = 1;
                button1_Click_1(sender, e);

            }
            if (this.parentT != null)
            {
                Factory.Instance.LoadRuleFrame(id, this.parentT.frame2, false, null);
                this.parentT.tabItem2.Visibility = System.Windows.Visibility.Visible;
                this.parentT.tabItem2.Header = id;
                this.parentT.tabControl1.SelectedIndex = 1;
                button1_Click_1(sender, e);
            }

            MainWindow.main.loadActionTree();
            MainWindow.main.loadRules();
            MainWindow.main.loadPostProcessData();

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

                //check if action exist
                bool exist = (from x in Factory.Instance.i.actions where x.id == a.id select x).Any();

                if (!exist)
                {
                    if (string.IsNullOrEmpty(a.id))
                    {
                        MessageBox.Show("Action 'id' is mandatory", "Error");
                        return;
                    }
                    else Factory.Instance.i.actions.Add(getAction(null));
                    msg = "Added!";

                    MainWindow.main.loadActionTree();
                    MainWindow.main.loadRules();
                    MainWindow.main.loadPostProcessData();

                }
                else msg = string.Format("Action with name '{0}' already exist! Change name/id.", a.id);
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
            MainWindow.main.loadActionTree();
        }


        /// <summary>
        /// Set rule
        /// </summary>
        private void cmbRuleId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRuleId.SelectedValue != null)
            {
                if (this.parent != null)
                {
                    this.parent.LoadRuleFrame((string)cmbRuleId.SelectedValue);
                    this.parent.tabItem2.Visibility = System.Windows.Visibility.Visible;
                }
                if (this.parentT != null)
                {
                    Factory.Instance.LoadRuleFrame((string)cmbRuleId.SelectedValue, this.parentT.frame2, false, null);

                    this.parentT.tabItem2.Header = (string)cmbRuleId.SelectedValue;
                    this.parentT.tabItem2.Visibility = System.Windows.Visibility.Visible;
                }
               
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            cmbRuleId.IsEnabled = true;
            cmbInputActionId.IsEnabled = true;
            cmbInputType.IsEnabled = true;
            cmbType.IsEnabled = true;
            txtCustomVal.IsEnabled = true;
            cmbInputType.SelectedValue = true;
            btnSelectFile.IsEnabled = true;
            btnAddRule.IsEnabled = true;           
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           string val = ((string)cmbType.SelectedValue).ToLower();
            cmbRuleId.Items.Clear();
            foreach (object o in Factory.Instance.getAllObj())
            {
                string typeName = o.GetType().Name.ToLower();
                if (typeName.Contains(val))
                {
                    cmbRuleId.Items.Add(o.GetType().GetProperty("id").GetValue(o, null));
                   
                }

            }
        }
    }
}
