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
using System.Reflection;

namespace IscraperBuilder
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow main;
        RoutedEvent evt { get; set; }
        RoutedEventArgs evtArg { get; set; }
        internal string Status
        {
            get { return currentMessage.Content.ToString(); }
            set { currentMessage.Content = value; /*Dispatcher.Invoke(new Action(() => { currentMessage.Content = value; }));*/ }
        }
        public MainWindow()
        {         
            InitializeComponent();
            main = this;

            //Load Projects
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += "\\projects";
            foreach (string f in System.IO.Directory.GetFiles(path))
            {
                System.IO.FileInfo finfo = new System.IO.FileInfo(f);
                cmbPrjts.Items.Add(finfo.Name);
            }

            this.evt =  EventManager.RegisterRoutedEvent("keyEnterEvt", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBox));
            this.evtArg = new RoutedEventArgs(this.evt);
        }

        #region loaders
        public void loadRules()
        {
            if (Factory.Instance.i != null)
            {
                if (Factory.Instance.i.rules != null)
                {
                    

                    rules.Items.Clear();
                    int c=0;
                    foreach (string r in Factory.Instance.getRuleIds())
                    {
                        TextBox i = new TextBox();
                        i.Text = r;
                        i.BorderThickness = new Thickness(0, 0, 0, 0);
                        i.KeyDown += new KeyEventHandler(saveRuleNameEnter);
                        i.ToolTip = "Inline edit : write and press enter to change rule name!";
                        i.Tag = r;
                        i.MouseUp +=  new System.Windows.Input.MouseButtonEventHandler(ruleClick);
                        i.ContextMenu = Resources["RuleContextMenu"] as ContextMenu;
                        rules.Items.Add(i);
                        c++;
                    }
                    trules.Text = string.Format("Rules ({0})", c.ToString());
                }
            }
        }

        public void loadPostProcessData()
        {
            if (Factory.Instance.i != null)
            {
                if (Factory.Instance.i.postProcess != null)
                {
                    int ii = 0;

                    //Append
                    if (Factory.Instance.i.postProcess.append != null)
                    {
                        tAppend.Text = string.Format("Append ({0})", Factory.Instance.i.postProcess.append.Count);
                        ii += Factory.Instance.i.postProcess.append.Count;
                        pAppend.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.append)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "append";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pAppend.Items.Add(i);
                        }
                    }

                    //pHtmlEncodeDecode
                    if (Factory.Instance.i.postProcess.htmlEncodeDecode != null)
                    {
                        tHtmlEncodeDecode.Text = string.Format("Html Encode/Decode ({0})", Factory.Instance.i.postProcess.htmlEncodeDecode.Count);
                        ii += Factory.Instance.i.postProcess.htmlEncodeDecode.Count;
                        pHtmlEncodeDecode.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.htmlEncodeDecode)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "htmlEncodeDecode";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pHtmlEncodeDecode.Items.Add(i);
                        }
                    }

                    //regularExpression
                    if (Factory.Instance.i.postProcess.regularExpression != null)
                    {
                        tRegex.Text = string.Format("Regular expression ({0})", Factory.Instance.i.postProcess.regularExpression.Count);
                        ii += Factory.Instance.i.postProcess.regularExpression.Count;
                        pRegex.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.regularExpression)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "regularExpression";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pRegex.Items.Add(i);
                        }
                    }

                    //replace
                    if (Factory.Instance.i.postProcess.replace != null)
                    {
                        tReplace.Text = string.Format("Replace ({0})", Factory.Instance.i.postProcess.replace.Count);
                        ii += Factory.Instance.i.postProcess.replace.Count;
                        pReplace.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.replace)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "replace";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pReplace.Items.Add(i);
                        }
                    }

                    //startEndWith
                    if (Factory.Instance.i.postProcess.startEndWith != null)
                    {
                        tStartEndWith.Text = string.Format("Start/End With ({0})", Factory.Instance.i.postProcess.startEndWith.Count);
                        ii += Factory.Instance.i.postProcess.startEndWith.Count;
                        pStartEndWith.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.startEndWith)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "startEndWith";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pStartEndWith.Items.Add(i);
                        }
                    }

                    //startEndWith
                    if (Factory.Instance.i.postProcess.substring != null)
                    {
                        tSubstring.Text = string.Format("Substring ({0})", Factory.Instance.i.postProcess.substring.Count);
                        ii += Factory.Instance.i.postProcess.substring.Count;
                        pSubstring.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.substring)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "substring";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pSubstring.Items.Add(i);
                        }
                    }

                    //startEndWith
                    if (Factory.Instance.i.postProcess.trim != null)
                    {
                        tTrim.Text = string.Format("Trim ({0})", Factory.Instance.i.postProcess.trim.Count);
                        ii += Factory.Instance.i.postProcess.trim.Count;                       
                        pTrim.Items.Clear();
                        foreach (var r in Factory.Instance.i.postProcess.trim)
                        {
                            TreeViewItem i = new TreeViewItem();
                            i.Header = r.id;
                            i.Tag = "trim";
                            i.MouseUp += new System.Windows.Input.MouseButtonEventHandler(postProcessClick);
                            pTrim.Items.Add(i);
                        }
                    }

                    tPpData.Text = string.Format("Post Process Data ({0})", ii);
                }

            }
        }
        #endregion

        #region Window 	behavior
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);         
            this.DragMove();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            this.DragMove();
        }

        private void menu1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            this.DragMove();
        } 

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);            
        }

       
        private void maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
                this.WindowState = System.Windows.WindowState.Normal;
            else this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if(this.WindowState == System.Windows.WindowState.Maximized)
                    this.WindowState = System.Windows.WindowState.Normal;

                if (this.WindowState == System.Windows.WindowState.Normal)
                    this.WindowState = System.Windows.WindowState.Minimized;

            }
        }

        public void ruleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem i = e.Source as TreeViewItem;
            if (i != null)
            {
                Factory.Instance.LoadRuleFrame((string)i.Header,this.frame1,false,null);
            }
        }

        public void postProcessClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        #endregion 

        #region Top Menu
        /// <summary>
        /// Open Project
        /// </summary>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem i = e.Source as MenuItem;
            switch (i.Name)
            {
                case "openPrj":
                    {
                        // Create OpenFileDialog 
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        Nullable<bool> result = dlg.ShowDialog();
                        // Get the selected file name and display in a TextBox 
                        if (result == true)
                        {
                            // Open document 
                            Factory.Instance.i = IntelliScraper.Xml.Serialization.Serialize(dlg.FileName);
                            Factory.Instance.openedFileProject = dlg.FileName;
                            this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                            this.prjId.Text = Factory.Instance.i.Project.ProjectInfo.projectName;
                            loadActionTree();
                            loadRules();
                            loadPostProcessData();
                        }
                        
                        break;
                    }
            }
        }

        /// <summary>
        /// create New
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controls.PrjNew p = new Controls.PrjNew()
            {
                ShowInTaskbar = false,               // don't show the dialog on the taskbar
                Topmost = true,                      // ensure we're Always On Top
                ResizeMode = ResizeMode.NoResize,    // remove excess caption bar buttons
                Owner = Application.Current.MainWindow,
            };
            p.ShowDialog();

            string prjName = p.projectName;
            if (!string.IsNullOrEmpty(prjName))
            {
                string path = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location);
                path += "\\projects";
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                IntelliScraper.Db.intelliScraper i = new IntelliScraper.Db.intelliScraper();
                i.actions = new IntelliScraper.Db.intelliScraperActionCollection();
                i.postProcess = new IntelliScraper.Db.intelliScraperPostProcess();
                i.Project = new IntelliScraper.Db.intelliScraperProject();
                i.Project.ProjectInfo = new IntelliScraper.Db.intelliScraperProjectProjectInfo();
                i.Project.ProjectInfo.projectName = prjName;
                i.rules = new IntelliScraper.Db.intelliScraperRules();
                Factory.Instance.i = i;

                string file = path + "\\" + prjName + ".xml";
                System.IO.FileStream fs =  System.IO.File.Create(file);
                fs.Close();
                IntelliScraper.Xml.Serialization.DeSerialize(Factory.Instance.i, file);
                Factory.Instance.openedFileProject = file;
                

                this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                this.prjId.Text = Factory.Instance.i.Project.ProjectInfo.projectName;

                loadActionTree();

                cmbPrjts.Items.Clear();
                foreach (string f in System.IO.Directory.GetFiles(path))
                {
                    System.IO.FileInfo finfo = new System.IO.FileInfo(f);
                    cmbPrjts.Items.Add(finfo.Name);
                    
                }
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                cmbPrjts.SelectedValue = fi.Name;
            }
            else MainWindow.main.Status = "Cannot create Project";
        }

      

        /// <summary>
        /// Exit
        /// </summary>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Select existing project
        /// </summary>
        private void cmbPrjts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox i = e.Source as ComboBox;
            if (i.SelectedIndex > 0)
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                path += "\\projects\\";
                string prjName = path + (string)i.SelectedValue;
                if (System.IO.File.Exists(prjName))
                {
                    // Open document 
                    Factory.Instance.i = IntelliScraper.Xml.Serialization.Serialize(prjName);
                    Factory.Instance.openedFileProject = prjName;
                    this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                    this.prjId.Text = Factory.Instance.i.Project.ProjectInfo.projectName;
                    loadActionTree();
                    prjSetting.IsSelected = true;
                    loadRules();
                    loadPostProcessData();
                }
            }
        }


        #endregion

        #region Project Setting

        /// <summary>
        /// On click ACtion/Rules tree
        /// </summary>
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

           
            if (Factory.Instance.i != null)
            {
                string name = string.Empty;
                if (e.Source.GetType() == typeof(StackPanel))
                    name = (e.Source as StackPanel).Name;
                if (e.Source.GetType() == typeof(TextBlock))
                    name = (e.Source as TextBlock).Text;
                /* if (e.Source.GetType() == typeof(Image))
                     name = (e.Source as Image).Name;*/
                
                switch (name)
                {
                    case "sPrjSetting":
                    case "Project Settings":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                            break;
                        }
                    case "Info":
                    case "sPrjInfo":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                            break;
                        }
                    case "Proxies":
                    case "sPrjProxy":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjProxy.xaml", UriKind.Relative);
                            break;
                        }
                    case "Custom Proxies":
                        case "sCustomPrjProxy":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjCustomProxy.xaml", UriKind.Relative);
                            break;
                        }
                        case "sPrjBrowser":
                        case "Browser Client":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjBrowser2.xaml", UriKind.Relative);
                            break;
                        }
                    case "sPrjStore":
                    case "Save/Read from/to...":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\AddStorage.xaml", UriKind.Relative);
                            break;
                        }
                    case "sPrjFtp":
                    case "Ftp":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjFtp.xaml", UriKind.Relative);
                            break;
                        }
                    case "Action":
                    case "Actions/Rules":
                        {

                            this.frame1.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                            this.frame1.Source = new Uri("Controls\\Rules\\ActionRules.xaml", UriKind.Relative);                      
                            break;
                        }
                    case "Rules":
                    case "sRules":
                        {
                            this.frame1.Source = new Uri("Controls\\Rules\\RulesWrapper.xaml", UriKind.Relative);
                            break;
                        }
                   
                }

                if (name.Contains("Actions/Rules"))
                {
                    this.frame1.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                    this.frame1.Source = new Uri("Controls\\Rules\\ActionRules.xaml", UriKind.Relative);
                 
                }
            }
            else MessageBox.Show("No project opened/created!!!");
            
        }

        /// <summary>
        /// On click on action
        /// </summary>
        private void sRules_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel_MouseLeftButtonDown(sender, e);
        }

        #endregion
        
        #region TreeView
        /// <summary>
        /// Load action TreeView
        /// </summary>
        public void loadActionTree()
        {
            if (Factory.Instance.i.actions != null)
            {
                List<object> rules = Factory.Instance.getAllObj();

              
                List<TreeviewH> actions = new List<TreeviewH>();

                var tmp = from x in Factory.Instance.i.actions where  
                              (x.input != null && string.IsNullOrEmpty(x.input.actionId)) 
                          
                          select x;
               // int c = tmp.Count();
                TreeViewAction.Items.Clear();
                foreach (var a in tmp)
                {
                    TreeViewActionResult r = getActiontreeView(a);                   
                    TreeViewAction.Items.Add(r.treeView);
                }

                tTreeViewAction.Text = string.Format("Actions/Rules ({0})", Factory.Instance.i.actions.Count);
  
            }
           
        }

        /// <summary>
        /// Get single parent node with Childs (recursive)
        /// </summary>
        private TreeViewActionResult getActiontreeView(IntelliScraper.Db.intelliScraperAction a)
        {
            TreeviewH singleAction = new TreeviewH(a.id,string.Format("{0} - ({1})",a.id,a.ruleId));
           
            singleAction.FontWeight = FontWeights.Bold;
            if (string.IsNullOrEmpty(a.ruleId))
                a.ruleId = "none";

         
            //Recursive get childs
            var tmp2 = from x in Factory.Instance.i.actions where (x.input != null && !string.IsNullOrEmpty(x.input.actionId) && x.input.actionId == a.id) select x;
            if (tmp2 != null)
            {
                foreach (var t in tmp2)
                {
                    IntelliScraper.Db.intelliScraperAction childAction = (from x in Factory.Instance.i.actions where x.id == t.id select x).SingleOrDefault();

                    if (childAction != null)
                    {
                        TreeViewActionResult child = getActiontreeView(childAction);
                        singleAction.Items.Add(child.treeView);
                    }
                }
            }

            TreeViewActionResult res = new TreeViewActionResult();
            res.treeView = singleAction;
            res.action = a;

            return res;
        }
              
        /// <summary>
        /// On treeview Item Click
        /// </summary>       
        public static void actionClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string _actionId = string.Empty;
            string header = string.Empty;
            if (e.Source.GetType() == typeof(TreeviewH))
            {
                TreeViewItem i = e.Source as TreeViewItem;
                _actionId = (string)i.Tag;
                header = (string)i.Header;
            }
            if (e.Source.GetType() == typeof(TextBlock))
            {
                TextBlock p = e.Source as TextBlock;
                _actionId = (string)p.Tag;
                header = p.Text;
            }

            if (e.Source.GetType() == typeof(Image))
            {
                Image p = e.Source as Image;
                _actionId = (string)p.Tag;
            }
           
            if (e.Source.GetType() == typeof(StackPanel))
            {
                StackPanel p = e.Source as StackPanel;
                _actionId = (string)p.Tag;
                
            }

            if(!string.IsNullOrEmpty(_actionId))
            {
                var act = (from x in Factory.Instance.i.actions where x.id == _actionId select x).SingleOrDefault();


                ActRuleTabFrame tbf = new ActRuleTabFrame();
                Controls.Rules.Action a = new Controls.Rules.Action(false, act.type, tbf,act);


                
                tbf.setFrame1(a);

                string ruleId = string.Empty;
                int index = header.IndexOf("-");
                if (index > 0)
                {
                    ruleId = header.Substring(index).Replace("-", "").Replace("(", "").Replace(")","").Trim();
                    tbf.tabItem2.Header = ruleId;
                }

                Factory.Instance.LoadRuleFrame(ruleId, tbf.frame2, false, null);
                

                MainWindow.main.frame1.Navigate(tbf);
            }

        }

        /// <summary>
        /// Context menu click (on action)
        /// </summary>
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TreeviewH h = cm.PlacementTarget as TreeviewH;
                    if (h != null)
                    {
                        string id = (string)h.Tag;
                        ActionRules ar = new ActionRules();
                        ar.setParentId(id);
                        frame1.Navigate(ar);
                        h.IsSelected = true;
                    }
                }
            }

        }

        /// <summary>
        /// Context menu click Remove Action
        /// </summary>
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TreeviewH h = cm.PlacementTarget as TreeviewH;
                    string actionId = (string)h.Header;
                    int index = actionId.LastIndexOf('-');
                    if(index > 0)
                        actionId = actionId.Substring(0,index).Trim();

                    //Check if this action have child
                    bool haveChild = (from x in Factory.Instance.i.actions where x.input != null && x.input.actionId == actionId select x).Any();
                    if (haveChild)
                        MessageBox.Show(string.Format("Cannot remove action {0}!\nRemove childs first!",actionId), "Delete Action - Error");

                    else if (MessageBox.Show(string.Format("Delete action '{0}'", actionId), "Delete action", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        int i =0;
                        foreach (IntelliScraper.Db.intelliScraperAction a in Factory.Instance.i.actions)
                        {
                            if (a.id == actionId)
                                break;
                            i++;
                        }
                        Factory.Instance.i.actions.RemoveAt(i);
                        Factory.Instance.Save();
                        loadActionTree();
                        loadRules();
                        loadPostProcessData();
                        
                    }

                }
            }
        }

        /// <summary>
        /// Remove Rule Context Menu
        /// </summary>
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;           
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TextBox h = cm.PlacementTarget as TextBox;
                    string ruleId = (string)h.Text;
                    
                    //check if some action use this rule
                    bool ruleIsUsed = (from x in Factory.Instance.i.actions where x.ruleId == ruleId select x).Any();

                    if (ruleIsUsed)
                        MessageBox.Show(string.Format("Cannot remove!\nThe rule {0} id used by an Action", ruleId), "Error");
                    else if (MessageBox.Show(string.Format("Remove rule {0}?", ruleId), "Remove rule", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                       

                        foreach (object r in Factory.Instance.getAllObj())
                        {
                            string id = (string)r.GetType().GetProperty("id").GetValue(r, null);
                            if (id == ruleId)
                            {
                                if (r.GetType() == typeof(IntelliScraper.Db.httpGet))
                                    Factory.Instance.i.rules.httpGet.Remove((IntelliScraper.Db.httpGet)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.httpPost))
                                    Factory.Instance.i.rules.httpPost.Remove((IntelliScraper.Db.httpPost)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.xpathSingle))
                                    Factory.Instance.i.rules.xpathSingle.Remove((IntelliScraper.Db.xpathSingle)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.xpathCollection))
                                    Factory.Instance.i.rules.xpathCollection.Remove((IntelliScraper.Db.xpathCollection)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.loop_link))
                                    Factory.Instance.i.rules.loop_link.Remove((IntelliScraper.Db.loop_link)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.download))
                                    Factory.Instance.i.rules.download.Remove((IntelliScraper.Db.download)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.upload))
                                    Factory.Instance.i.rules.upload.Remove((IntelliScraper.Db.upload)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.ftpPut))
                                    Factory.Instance.i.rules.ftpPut.Remove((IntelliScraper.Db.ftpPut)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.actionZip))
                                    Factory.Instance.i.rules.actionZip.Remove((IntelliScraper.Db.actionZip)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.save))
                                    Factory.Instance.i.rules.save.Remove((IntelliScraper.Db.save)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.screenShot))
                                    Factory.Instance.i.rules.screenShot.Remove((IntelliScraper.Db.screenShot)r);

                                if (r.GetType() == typeof(IntelliScraper.Db.plugin))
                                    Factory.Instance.i.rules.plugin.Remove((IntelliScraper.Db.plugin)r);
                            }
                        }
                        Factory.Instance.Save();
                        loadActionTree();
                        loadRules();
                        loadPostProcessData();
                       
                    }
                }
            }
        }

        /// <summary>
        /// Rule Change Name
        /// </summary>
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            TextBox h = null;
            if (e.Source.GetType() == typeof(MenuItem))
            {
                MenuItem mi = e.Source as MenuItem;

                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                        h = cm.PlacementTarget as TextBox;
                }
            }
            if (e.Source.GetType() == typeof(TextBox))
                h = (TextBox)e.Source;



            if (h != null)
            {

                string ruleId = (string)h.Tag;

                bool changed = false;
                foreach (object r in Factory.Instance.getAllObj())
                {
                    string id = (string)r.GetType().GetProperty("id").GetValue(r, null);
                    if (id == ruleId)
                    {
                        if (r.GetType() == typeof(IntelliScraper.Db.httpGet))
                        {
                            var rule = (from x in Factory.Instance.i.rules.httpGet where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.xpathSingle))
                        {
                            var rule = (from x in Factory.Instance.i.rules.xpathSingle where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.httpPost))
                        {
                            var rule = (from x in Factory.Instance.i.rules.httpPost where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.xpathCollection))
                        {
                            var rule = (from x in Factory.Instance.i.rules.xpathCollection where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.loop_link))
                        {
                            var rule = (from x in Factory.Instance.i.rules.loop_link where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.download))
                        {
                            var rule = (from x in Factory.Instance.i.rules.download where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.upload))
                        {
                            var rule = (from x in Factory.Instance.i.rules.upload where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.ftpPut))
                        {
                            var rule = (from x in Factory.Instance.i.rules.ftpPut where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.actionZip))
                        {
                            var rule = (from x in Factory.Instance.i.rules.actionZip where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.save))
                        {
                            var rule = (from x in Factory.Instance.i.rules.save where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.screenShot))
                        {
                            var rule = (from x in Factory.Instance.i.rules.screenShot where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.plugin))
                        {
                            var rule = (from x in Factory.Instance.i.rules.plugin where x.id == h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }


                    }
                }

                if (changed)
                {
                    var actions = from x in Factory.Instance.i.actions where x.ruleId == (string)h.Tag select x;
                    foreach (var a in actions)
                    {
                        a.ruleId = h.Text;
                    }

                    Factory.Instance.Save();
                    loadActionTree();
                    loadRules();
                    loadPostProcessData();
                }


            }
        }

        /// <summary>
        /// On enter change rule name
        /// </summary>
        private void saveRuleNameEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox mi = e.Source as TextBox;
                this.evtArg.Source = mi;
               /* RoutedEventArgs e1 = new RoutedEventArgs();
                e1.RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler),mi.GetType());
                e1.Source =mi;*/
                MenuItem_Click_6(sender, this.evtArg);
            }
            
        }


        #endregion

       

     
       

        
    }

    public class TreeViewActionResult
    {
        public TreeviewH treeView { get; set; }
        public IntelliScraper.Db.intelliScraperAction action { get; set; }
    }
}
