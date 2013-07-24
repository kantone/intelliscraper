using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace IscraperBuilder.Controls.Rules
{
    public class TreeviewH : TreeViewItem
    {
        public enum TreeType
        {
            action,
            actionInput,
            rule,
            postProcessData,
            none,
        }

        public string currentHeader { get; set; }
        public TreeviewH(string id,string header)
        {
            this.currentHeader = header;
            this.Header = header;
            this.Tag = id;
            //this.IsExpanded = true;
            this.FontWeight = FontWeights.Normal;
            this.MouseUp += new System.Windows.Input.MouseButtonEventHandler(MainWindow.actionClick);
            this.ContextMenu = MainWindow.main.Resources["ActionContextMenu"] as ContextMenu;
        }

        public TreeviewH(string header, TreeType type)
        {
            this.currentHeader = header;
            this.Header = header;
            //this.IsExpanded = true;
            this.Tag = type;
            this.FontWeight = FontWeights.Normal;
            this.MouseUp += new System.Windows.Input.MouseButtonEventHandler(MainWindow.actionClick);
            this.ContextMenu = MainWindow.main.Resources["ActionContextMenu"] as ContextMenu;
        }

        public static StackPanel getTreeWithImage(string header, TreeType type,string img,string id)
        {
            Image i = new Image();
            var uriSource = new Uri(@"/IscraperBuilder;component/img/" + img, UriKind.Relative);
            i.Source = new BitmapImage(uriSource);
            i.Height = 15;
            i.Width = 15;
            i.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            i.SnapsToDevicePixels = true;
            i.Opacity = 0.5;
            i.Tag = id;

            StackPanel panel = new StackPanel();
            //panel.MinWidth = 200;
            panel.Tag = type;
            panel.Orientation = Orientation.Horizontal;
            panel.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            panel.VerticalAlignment = VerticalAlignment.Top;
            panel.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MainWindow.actionClick);
            panel.Tag = id;

            TextBlock t = new TextBlock();
            t.Text = header;
            t.Tag = type;
            t.Margin = new System.Windows.Thickness(0,5,0,0);
            t.FontWeight = FontWeights.Normal;
            t.Height = 15;
            t.Foreground = new SolidColorBrush(Colors.Black);
            t.Tag = id;

            panel.Children.Add(i);
            panel.Children.Add(t);

            return panel;
          
        }

       
    }
}
