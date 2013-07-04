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
using System.Windows.Shapes;

namespace IscraperBuilder.Controls
{
    /// <summary>
    /// Logica di interazione per PrjNew.xaml
    /// </summary>
    public partial class PrjNew : Window
    {
        public string projectName { get; set; }
        public PrjNew()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.projectName = textBox1.Text;
            this.Close();
        }
    }
}
