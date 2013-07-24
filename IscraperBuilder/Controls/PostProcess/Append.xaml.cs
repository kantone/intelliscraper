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
    /// Logica di interazione per Append.xaml
    /// </summary>
    public partial class Append : Page
    {
        IntelliScraper.Db.append rule { get; set; }
        public Append(IntelliScraper.Db.append rule)
        {
            InitializeComponent();
            this.rule = rule;
            load();
        }

        void load()
        {
            if (this.rule != null)
            {
                txtID.Text = rule.id;
                txtVal.Text = rule.appendValue;
              

                cmbType.Items.Clear();
                foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.appendType)))
                    cmbType.Items.Add(s);
                cmbType.SelectedValue = rule.type.ToString();
               

            }
        }

        Utils.PopUp p = new Utils.PopUp();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            p.hide();
            if (validate())
            {
                rule.appendValue = txtVal.Text;
                if (cmbType.SelectedValue != null)
                    rule.type = (IntelliScraper.Db.appendType)Enum.Parse(typeof(IntelliScraper.Db.appendType), (string)cmbType.SelectedValue);

                Factory.Instance.Save();
                load();
            }
            p.showCannotSave(button1);

        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(txtVal.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtVal);
                return false;
            }

            if (cmbType.SelectedIndex < 0)
            {
                Utils.PopUp.showPopUpError("mandatory", cmbType);
                return false;
            }

            return true;
        }
    }
}
