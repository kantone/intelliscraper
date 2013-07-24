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
    /// Logica di interazione per HtmlEncodeDecode.xaml
    /// </summary>
    public partial class HtmlEncodeDecode : Page
    {
        IntelliScraper.Db.htmlEncodeDecode rule { get; set; }
        public HtmlEncodeDecode(IntelliScraper.Db.htmlEncodeDecode rule)
        {
            InitializeComponent();
            this.rule = rule;
            load();
        }

        void load()
        {
            if (rule != null)
            {
                txtID.Text = rule.id;
                chkDecode.IsChecked = rule.HtmlDecode;
                chkEncode.IsChecked = rule.HtmlEncode;
            }
        }

        Utils.PopUp p = new Utils.PopUp();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            p.hide();
            if (validate())
            {
                rule.HtmlDecode = (bool)chkDecode.IsChecked;
                rule.HtmlEncode = (bool)chkEncode.IsChecked;
                Factory.Instance.Save();
                load();
            }
            else p.showCannotSave(button1);
        }

        private bool validate()
        {
            if (!(bool)chkDecode.IsChecked || !(bool)chkEncode.IsChecked)
            {
                Utils.PopUp.showPopUpError("Encode or decode must be set or delete this rule", chkEncode);
                return false;
            }

          

            return true;
        }
    }
}
