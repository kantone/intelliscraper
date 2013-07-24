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
    /// Logica di interazione per Trim.xaml
    /// </summary>
    public partial class Trim : Page
    {
        IntelliScraper.Db.trim rule { get; set; }
        public Trim(IntelliScraper.Db.trim rule)
        {
            this.rule = rule;
            InitializeComponent();
            load();
        }

        void load()
        {
            cmbType.Items.Clear();
            foreach(string s in Enum.GetNames(typeof(IntelliScraper.Db.trimType)))
                cmbType.Items.Add(s);

            
            if(rule != null)
            {
                cmbType.SelectedValue = rule.type.ToString();
                txtId.Text = rule.id;
                txtTrimValue.Text = rule.trimValue;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTrimValue.Text))
            {
                var _rule = (from x in Factory.Instance.i.postProcess.trim where x.id == rule.id select x).FirstOrDefault();
                if (_rule != null)
                {

                    if (cmbType.SelectedValue != null)
                        _rule.type = (IntelliScraper.Db.trimType)Enum.Parse(typeof(IntelliScraper.Db.trimType), (string)cmbType.SelectedValue);

                    _rule.trimValue = txtTrimValue.Text;


                    Factory.Instance.Save();
                    load();
                }
            }
            else Utils.PopUp.showPopUpError("mandatory", txtTrimValue);
        }
    }
}
