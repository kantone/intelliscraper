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
    /// Logica di interazione per StartEndWith.xaml
    /// </summary>
    public partial class StartEndWith : Page
    {
        IntelliScraper.Db.startEndWith rule { get; set; }
        public StartEndWith(IntelliScraper.Db.startEndWith rule)
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
                txtAppendText.Text = rule.startEndWithAddVal;
                txtIfStartEndWith.Text = rule.starEndtWithVal;
                chkEnabled.IsChecked = rule.startEndWithEnabled;
                chkEnabled_Checked_1(this, null);

                cmbType.Items.Clear();
                foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.startEndWithType)))
                    cmbType.Items.Add(s);
                cmbType.SelectedValue = rule.type.ToString();
                cmbType_SelectionChanged(this, null);



                cmbAttachType.Items.Clear();
                foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.startEndWithAttachType)))
                    cmbAttachType.Items.Add(s);
                cmbAttachType.SelectedValue = rule.attachType.ToString();
                cmbAttachType_SelectionChanged(this, null);
            }
        }

       
        
        
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbAttachType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Enabled
        /// </summary>
        private void chkEnabled_Checked_1(object sender, RoutedEventArgs e)
        {
            bool enabled = false;
            if ((bool)chkEnabled.IsChecked)
                enabled = true;


            txtID.IsEnabled = enabled;
            txtAppendText.IsEnabled = enabled;
            txtIfStartEndWith.IsEnabled = enabled;
            cmbAttachType.IsEnabled = enabled;
            cmbType.IsEnabled = enabled;
        }

        Utils.PopUp p = new Utils.PopUp();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            p.hide();
            if (validate())
            {
                rule.id = txtID.Text;
                rule.startEndWithAddVal = txtAppendText.Text;
                rule.starEndtWithVal = txtIfStartEndWith.Text;
                rule.startEndWithEnabled = (bool)chkEnabled.IsChecked;

                if (cmbAttachType.SelectedValue != null)
                    rule.attachType = (IntelliScraper.Db.startEndWithAttachType)Enum.Parse(typeof(IntelliScraper.Db.startEndWithAttachType), (string)cmbAttachType.SelectedValue);

                if (cmbType.SelectedValue != null)
                    rule.type = (IntelliScraper.Db.startEndWithType)Enum.Parse(typeof(IntelliScraper.Db.startEndWithType), (string)cmbType.SelectedValue);

                Factory.Instance.Save();
                load();
            }
            else p.showCannotSave(button1);
           
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(txtAppendText.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtAppendText);
                return false;
            }

            if(string.IsNullOrEmpty(txtIfStartEndWith.Text))
            {
                Utils.PopUp.showPopUpError("mandatory", txtIfStartEndWith);
                return false;
            }

            return true;
        }
        
    }
}
