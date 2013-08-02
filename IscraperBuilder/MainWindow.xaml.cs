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

            string log = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +"\\log";
            if (!System.IO.Directory.Exists(log))
                System.IO.Directory.CreateDirectory(log);
             
        }

      

        #region loaders

        /// <summary>
        /// Load rules tree
        /// </summary>
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
                        i.GotFocus += new RoutedEventHandler(i_GotFocus);
                        i.LostFocus += new RoutedEventHandler(i_LostFocus);
                        i.ContextMenu = Resources["RuleContextMenu"] as ContextMenu;
                        rules.Items.Add(i);
                        c++;
                    }
                    trules.Text = string.Format("Rules ({0})", c.ToString());
                }
            }
        }


        /// <summary>
        /// Load Post Proces sData tree
        /// </summary>
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
                            pSubstring.Items.Add(i);
                        }
                    }

                    //trim
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
                            i.ContextMenu = Resources["PpContextMenu"] as ContextMenu;
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

        /// <summary>
        /// On click post process rule
        /// </summary>
        public void postProcessClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem h =  e.Source as TreeViewItem;
            string ruleId = (string)h.Header;
            TreeViewItem parent = ItemsControl.ItemsControlFromItemContainer(h) as TreeViewItem;
            string typeId = ((TextBlock)((StackPanel)parent.Header).Children[1]).Text;
            int index = typeId.LastIndexOf("(");
            if (index > 0)
                typeId = typeId.Substring(0, index).Trim();

            foreach (object o in Factory.Instance.getAllPostProcessObj())
            {
                Type t = o.GetType();
                string _id = (string)t.GetProperty("id").GetValue(o, null);

                if(_id == ruleId){
                //find id
                    if (t.Name.Replace(" ", "").ToLower() == typeId.Replace(" ", "").Replace("/","").ToLower())
                    {
                        //Replace
                        if (t == typeof(IntelliScraper.Db.replace))
                        {
                            IntelliScraper.Db.replace r = (IntelliScraper.Db.replace)o;
                            frame1.Navigate(new Controls.PostProcess.Replace(r));
                            setTitle("Post process data : Replace", "img/004.png"); 
                            break;
                        }

                        //append
                        if (t == typeof(IntelliScraper.Db.append))
                        {
                            IntelliScraper.Db.append r = (IntelliScraper.Db.append)o;
                            frame1.Navigate(new Controls.PostProcess.Append(r));
                            setTitle("Post process data : Append", "img/016.png"); 
                            break;
                        }

                        //htmlEncodeDecode
                        if (t == typeof(IntelliScraper.Db.htmlEncodeDecode))
                        {
                            IntelliScraper.Db.htmlEncodeDecode r = (IntelliScraper.Db.htmlEncodeDecode)o;
                            frame1.Navigate(new Controls.PostProcess.HtmlEncodeDecode(r));
                            setTitle("Post process data : Html encode/decode", "img/027.png");
                            break;
                        }

                        //regularExpression
                        if (t == typeof(IntelliScraper.Db.regularExpression))
                        {
                            IntelliScraper.Db.regularExpression r = (IntelliScraper.Db.regularExpression)o;
                             frame1.Navigate(new Controls.PostProcess.Regex(r));
                             setTitle("Post process data : Regex", "img/007.png"); 
                            break;
                        }

                        //startEndWith
                        if (t == typeof(IntelliScraper.Db.startEndWith))
                        {
                            IntelliScraper.Db.startEndWith r = (IntelliScraper.Db.startEndWith)o;
                             frame1.Navigate(new Controls.PostProcess.StartEndWith(r));
                             setTitle("Post process data : Start/End with", "img/017.png"); 
                            break;
                        }

                        //substring
                        if (t == typeof(IntelliScraper.Db.substring))
                        {
                            IntelliScraper.Db.substring r = (IntelliScraper.Db.substring)o;
                            frame1.Navigate(new Controls.PostProcess.SubString(r));
                            setTitle("Post process data : Substring", "img/009.png"); 
                            break;
                        }

                        //trim
                        if (t == typeof(IntelliScraper.Db.trim))
                        {
                            IntelliScraper.Db.trim r = (IntelliScraper.Db.trim)o;
                            frame1.Navigate(new Controls.PostProcess.Trim(r));
                            setTitle("Post process data : Trim", "img/012a.png"); 
                            break;
                        }
                    }
                }
            }
        }

       

        /// <summary>
        /// Rule Focus lost change color
        /// </summary>
        void i_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox mi = e.Source as TextBox;
            mi.Background = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Rule Focus open rule editor and change color
        /// </summary>
        void i_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox mi = e.Source as TextBox;
            mi.Background = new SolidColorBrush(Colors.LightGreen);
            string id = mi.Text;
            Factory.Instance.LoadRuleFrame(id, this.frame1, false, null);

            setTitle("Rule : " + id, "img/051.png");
        }


        /// <summary>
        /// Open xml File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Factory.Instance.openedFileProject != null)
                System.Diagnostics.Process.Start(Factory.Instance.openedFileProject);
        }

        /// <summary>
        /// refresh
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {          
            loadRules();
            loadPostProcessData();
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
                            try
                            {
                                // Open document 
                                Factory.Instance.i = IntelliScraper.Xml.Serialization.Serialize(dlg.FileName);
                                Factory.Instance.openedFileProject = dlg.FileName;
                                this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                                this.prjId.Text = Factory.Instance.i.Project.ProjectInfo.projectName;
                                                              loadRules();
                                loadPostProcessData();
                            }
                            catch { MessageBox.Show("Project invalid");}
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
                    try
                    {
                        // Open document 
                        Factory.Instance.i = IntelliScraper.Xml.Serialization.Serialize(prjName);
                        Factory.Instance.openedFileProject = prjName;
                        this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                        this.prjId.Text = Factory.Instance.i.Project.ProjectInfo.projectName;
                                                prjSetting.IsSelected = true;
                        loadRules();
                        loadPostProcessData();
                    }
                    catch
                    {
                        MessageBox.Show("Project invalid");
                    }
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
            setTitle(string.Empty, null);
           
            if (Factory.Instance.i != null)
            {
                string name = string.Empty;
                if (e.Source.GetType() == typeof(StackPanel))
                    name = (e.Source as StackPanel).Name;
                if (e.Source.GetType() == typeof(TextBlock))
                    name = (e.Source as TextBlock).Text;
                               
                switch (name)
                {
                    case "sPrjSetting":
                    case "Project Settings":
                        {
                            setTitle("Project info", "img/024.png");
                            this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                            break;
                        }
                    case "Info":
                    case "sPrjInfo":
                        {
                            setTitle("Project info", "img/024.png");
                            this.frame1.Source = new Uri("Controls\\Project\\PrjInfo.xaml", UriKind.Relative);
                            break;
                        }
                    case "Proxies":
                    case "sPrjProxy":
                        {
                            setTitle("Proxy", "img/030.png");
                            this.frame1.Source = new Uri("Controls\\Project\\PrjProxy.xaml", UriKind.Relative);
                            break;
                        }
                    case "Custom Proxies":
                        case "sCustomPrjProxy":
                        {
                            setTitle("Custom Proxy", "img/002.png");
                            this.frame1.Source = new Uri("Controls\\Project\\PrjCustomProxy.xaml", UriKind.Relative);
                            break;
                        }
                        case "sPrjBrowser":
                        case "Browser Client":
                        {
                            setTitle("Browser client setting", "img/026.png");
                            this.frame1.Source = new Uri("Controls\\Project\\PrjBrowser2.xaml", UriKind.Relative);
                            break;
                        }
                    case "sPrjStore":
                    case "Save/Read from/to...":
                        {
                            setTitle("Input & output storage", "img/048.png");
                            this.frame1.Source = new Uri("Controls\\Project\\AddStorage.xaml", UriKind.Relative);
                            break;
                        }
                    case "sPrjFtp":
                    case "Ftp":
                        {
                            setTitle("Ftp", "img/037.png");                          
                            this.frame1.Source = new Uri("Controls\\Project\\PrjFtp.xaml", UriKind.Relative);
                            break;
                        }
                    case "Action":
                    case "Actions/Rules":
                        {
                            setTitle("Add Action/rule", "img/075.png"); 
                            this.frame1.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                            this.frame1.Source = new Uri("Controls\\Rules\\ActionRules.xaml", UriKind.Relative);                      
                            break;
                        }
                    case "Rules":
                    case "sRules":
                        {
                            setTitle("Rules", "img/051.png"); 
                            this.frame1.Source = new Uri("Controls\\Rules\\RulesWrapper.xaml", UriKind.Relative);
                            return;
                        }
                    case "sRun":
                    case "Run":
                        {
                            setTitle("Run project", "img/035a.png"); 
                            this.frame1.Source = new Uri("Controls\\Execute\\Run.xaml", UriKind.Relative);
                            break;
                        }
                    case "xPathTest":
                        {
                            setTitle("Test xpath", "img/017a.png");
                            this.frame1.Source = new Uri("Controls\\Tools\\Page1.xaml", UriKind.Relative);
                            break;
                        }
                    case "Preset Actions/Rules":
                        {
                            setTitle("Preset Actions/Rules", "img/014.png");
                            this.frame1.Source = new Uri("Controls\\Rules\\PresetManager.xaml", UriKind.Relative);
                            return;
                        }
                  
                   
                }

               
                if (name.Contains("Post Process Data"))
                {
                    setTitle("Post Process Data", "img/028.png"); 
                    this.frame1.Source = new Uri("Controls\\PostProcess\\PostProcessData.xaml", UriKind.Relative);

                }

                if (name.Contains("Rules ("))
                {
                    setTitle("Rules", "img/051.png");
                    this.frame1.Source = new Uri("Controls\\Rules\\RulesWrapper.xaml", UriKind.Relative);
                    
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

                            if (r.GetType() == typeof(IntelliScraper.Db.linksPageGeneratorType))
                                Factory.Instance.i.rules.linksPageGenerator.Remove((IntelliScraper.Db.linksPageGenerator)r);

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
                    loadRules();
                    loadPostProcessData();

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
                            var rule = (from x in Factory.Instance.i.rules.httpGet where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.xpathSingle))
                        {
                         
                            var rule = (from x in Factory.Instance.i.rules.xpathSingle where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.httpPost))
                        {
                            var rule = (from x in Factory.Instance.i.rules.httpPost where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.xpathCollection))
                        {
                            var rule = (from x in Factory.Instance.i.rules.xpathCollection where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.linksPageGenerator))
                        {
                            var rule = (from x in Factory.Instance.i.rules.linksPageGenerator where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.download))
                        {
                            var rule = (from x in Factory.Instance.i.rules.download where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.upload))
                        {
                            var rule = (from x in Factory.Instance.i.rules.upload where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.ftpPut))
                        {
                            var rule = (from x in Factory.Instance.i.rules.ftpPut where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.actionZip))
                        {
                            var rule = (from x in Factory.Instance.i.rules.actionZip where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.save))
                        {
                            var rule = (from x in Factory.Instance.i.rules.save where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.screenShot))
                        {
                            var rule = (from x in Factory.Instance.i.rules.screenShot where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }

                        if (r.GetType() == typeof(IntelliScraper.Db.plugin))
                        {
                            var rule = (from x in Factory.Instance.i.rules.plugin where x.id == (string)h.Tag select x).FirstOrDefault();
                            rule.id = h.Text;
                            changed = true;
                            break;
                        }


                    }
                }

                if (changed)
                {                   

                    Factory.Instance.Save();                    
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
            TextBox mi = e.Source as TextBox;

            mi.Background = new SolidColorBrush(Colors.LightSkyBlue);
            if (e.Key == Key.Enter)
            {

                mi.Background = new SolidColorBrush(Colors.White);
                this.evtArg.Source = mi;
               /* RoutedEventArgs e1 = new RoutedEventArgs();
                e1.RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler),mi.GetType());
                e1.Source =mi;*/
                MenuItem_Click_6(sender, this.evtArg);
            }
            
        }


        #endregion


        /// <summary>
        /// Post Process item Remove
        /// </summary>
        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            ContextMenu cm = mi.CommandParameter as ContextMenu;
            TreeViewItem tr = cm.PlacementTarget as TreeViewItem;
            string ruleId = (string)tr.Header;
            TreeViewItem parent = ItemsControl.ItemsControlFromItemContainer(tr) as TreeViewItem;
            string typeId = ((TextBlock)((StackPanel)parent.Header).Children[1]).Text;
            int index = typeId.LastIndexOf("(");
            if (index > 0)
                typeId = typeId.Substring(0, index).Trim();
            

            foreach (object o in Factory.Instance.getAllPostProcessObj())
            {
                Type t = o.GetType();
                string _id = (string)t.GetProperty("id").GetValue(o, null);

                //find id
                if (t.Name.Replace(" ", "").ToLower() == typeId.Replace(" ", "").Replace("/", "").ToLower())
                {
                    //Replace
                    if (t == typeof(IntelliScraper.Db.replace))
                    {
                        IntelliScraper.Db.replace r = (IntelliScraper.Db.replace)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.replace.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //append
                    if (t == typeof(IntelliScraper.Db.append))
                    {
                        IntelliScraper.Db.append r = (IntelliScraper.Db.append)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.append.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //htmlEncodeDecode
                    if (t == typeof(IntelliScraper.Db.htmlEncodeDecode))
                    {
                        IntelliScraper.Db.htmlEncodeDecode r = (IntelliScraper.Db.htmlEncodeDecode)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.htmlEncodeDecode.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //regularExpression
                    if (t == typeof(IntelliScraper.Db.regularExpression))
                    {
                        IntelliScraper.Db.regularExpression r = (IntelliScraper.Db.regularExpression)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.regularExpression.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //startEndWith
                    if (t == typeof(IntelliScraper.Db.startEndWith))
                    {
                        IntelliScraper.Db.startEndWith r = (IntelliScraper.Db.startEndWith)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.startEndWith.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //substring
                    if (t == typeof(IntelliScraper.Db.substring))
                    {
                        IntelliScraper.Db.substring r = (IntelliScraper.Db.substring)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.substring.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }

                    //trim
                    if (t == typeof(IntelliScraper.Db.trim))
                    {
                        IntelliScraper.Db.trim r = (IntelliScraper.Db.trim)o;
                        if (_id == ruleId)
                        {
                            Factory.Instance.i.postProcess.trim.Remove(r);
                            Factory.Instance.Save();
                            
                            loadRules();
                            loadPostProcessData();
                            break;
                        }
                    }
                }


              
            }

        }

        /// <summary>
        /// Post Process item Add
        /// </summary>
        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {

        }


        private void setTitle(string title, string img)
        {
            try
            {
                if (!string.IsNullOrEmpty(img))
                {
                    imgTitle.Visibility = System.Windows.Visibility.Visible;
                    imgTitle.Source = new BitmapImage(new Uri("pack://application:,,,/" + img));
                }
                else imgTitle.Visibility = System.Windows.Visibility.Hidden;
            }
            catch { }

            if (!string.IsNullOrEmpty(title))
            {
                txtTitle.Visibility = System.Windows.Visibility.Visible;
                txtTitle.Text = title;
                borderTitle.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                txtTitle.Visibility = System.Windows.Visibility.Hidden;
                borderTitle.Visibility = System.Windows.Visibility.Hidden;
            }
           
        }
    }
    
}
