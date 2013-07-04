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
        internal string Status
        {
            get { return currentMessage.Content.ToString(); }
            set { currentMessage.Content = value; /*Dispatcher.Invoke(new Action(() => { currentMessage.Content = value; }));*/ }
        }
        public MainWindow()
        {         
            InitializeComponent();
            main = this;
           
        }
       

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
                loadActionTree(); 
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


        #endregion

        #region Project Setting
        /// <summary>
        /// 
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
                    case "ActionTree":
                    case "Tree":
                        {
                            this.frame1.Source = new Uri("Controls\\Rules\\Tree.xaml", UriKind.Relative);
                            break;
                        }
                   
                }
            }
            else MessageBox.Show("No project opened/created!!!");
            
        }

        private void sRules_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel_MouseLeftButtonDown(sender, e);
        }

        #endregion
        
        #region TreeView
        /// <summary>
        /// Load action TreeView
        /// </summary>
        private void loadActionTree()
        {
            if (Factory.Instance.i.actions != null)
            {
                List<object> rules = Factory.Instance.getAllObj();

              
                List<TreeviewH> actions = new List<TreeviewH>();

                var tmp = from x in Factory.Instance.i.actions where  
                              (x.input != null && string.IsNullOrEmpty(x.input.actionId)) 
                          
                          select x;
                int c = tmp.Count();
                foreach (var a in tmp)
                {
                    TreeViewActionResult r = getActiontreeView(a);                   
                    TreeViewAction.Items.Add(r.treeView);
                }               
                               
            }
           
        }

        /// <summary>
        /// Get single parent node with Childs (recursive)
        /// </summary>
        private TreeViewActionResult getActiontreeView(IntelliScraper.Db.intelliScraperAction a)
        {
            TreeviewH singleAction = new TreeviewH(a.id);
            singleAction.FontWeight = FontWeights.Bold;

            if (string.IsNullOrEmpty(a.ruleId))
                a.ruleId = "none";

            StackPanel rule = TreeviewH.getTreeWithImage(string.Format("Rule '{0}'", a.ruleId), TreeviewH.TreeType.rule, "051.png", a.id);
            singleAction.Items.Add(rule);


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
            if (e.Source.GetType() == typeof(TreeviewH))
            {
                TreeViewItem i = e.Source as TreeViewItem;
                _actionId = (string)i.Header;
            }
            if (e.Source.GetType() == typeof(TextBlock))
            {
                TextBlock p = e.Source as TextBlock;
                _actionId = (string)p.Tag;
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

                Controls.Rules.Action a = new Controls.Rules.Action(false, act.type,null,act);
                MainWindow.main.frame1.Navigate(a);
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
