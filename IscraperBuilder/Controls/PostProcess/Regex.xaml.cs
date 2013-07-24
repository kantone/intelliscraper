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

namespace IscraperBuilder.Controls.PostProcess
{
    /// <summary>
    /// Logica di interazione per Regex.xaml
    /// </summary>
    public partial class Regex : Page
    {
        IntelliScraper.Db.regularExpression rule { get; set; }
        public Regex(IntelliScraper.Db.regularExpression rule)
        {
            this.rule = rule;
            InitializeComponent();
            load();
        }

        private void load()
        {
            if (this.rule != null)
            {
                if (rule.regularExpressionValue == null)
                    rule.regularExpressionValue = new string[0];

                txtId.Text = rule.id;
                listBox1.Items.Clear();
                foreach (string s in rule.regularExpressionValue)
                    listBox1.Items.Add(s);
               
            }
        }

        //Save
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            rule.regularExpressionValue = new string[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
                rule.regularExpressionValue[i] = (string)listBox1.Items[i];

            Factory.Instance.Save();
            load();

        }

        /// <summary>
        /// Add
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegex.Text))
            {
                listBox1.Items.Add(txtRegex.Text);
            }
            else MessageBox.Show("Missing regex value","Error");
        }

        /// <summary>
        /// Remove
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }
}
