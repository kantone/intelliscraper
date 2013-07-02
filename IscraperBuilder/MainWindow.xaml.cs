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

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
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
                            //this.frame1.Source = new Uri("Controls\\test.xaml", UriKind.Relative);
                        }
                        
                        break;
                    }
            }
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
                        case "Browser":
                        {
                            this.frame1.Source = new Uri("Controls\\Project\\PrjBrowser.xaml", UriKind.Relative);
                            break;
                        }
                    case "sPrjStore":
                        {
                            break;
                        }
                    case "sPrjFtp":
                        {
                            break;
                        }
                   
                }
            }
            else MessageBox.Show("No project opened/created!!!");
            
        }
        #endregion

        


    }
}
