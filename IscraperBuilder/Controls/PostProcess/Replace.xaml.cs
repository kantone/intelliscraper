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
    /// Logica di interazione per Replace.xaml
    /// </summary>
    public partial class Replace : Page
    {
        IntelliScraper.Db.replace rule { get; set; }
        public Replace(IntelliScraper.Db.replace rule)
        {
            this.rule = rule;
            InitializeComponent();
            load();
        }

        private void load()
        {
            if (this.rule != null)
            {
                txtId.Text = rule.id;
                txtFind.Text = rule.findText;
                txtReplace.Text = rule.replaceText;
            }
        }

        Utils.PopUp p = new Utils.PopUp();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
          

            p.hide();
            if (validate())
            {
                rule.findText = txtFind.Text;
                rule.replaceText = txtReplace.Text;
                Factory.Instance.Save();
                load();
            }
            else p.showCannotSave(button1);
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(txtFind.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtFind);
                return false;
            }


            return true;
        }
    }
}
