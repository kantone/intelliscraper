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

namespace IscraperBuilder.Controls.Common
{
    /// <summary>
    /// Logica di interazione per ImageConvert.xaml
    /// </summary>
    public partial class ImageConvert : UserControl
    {
        public IntelliScraper.Db.imageConvert rule { get; set; }
        public ImageConvert()
        {
            InitializeComponent();
            txtThumbH.PreviewTextInput += new TextCompositionEventHandler(txtonlyNubmers_PreviewTextInput);
            txtThumbW.PreviewTextInput += new TextCompositionEventHandler(txtonlyNubmers_PreviewTextInput);
            txtWidht.PreviewTextInput += new TextCompositionEventHandler(txtonlyNubmers_PreviewTextInput);
            txtHeight.PreviewTextInput += new TextCompositionEventHandler(txtonlyNubmers_PreviewTextInput);
        }

        /// <summary>
        /// Only Numbers
        /// </summary>       
        void txtonlyNubmers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        public void load(IntelliScraper.Db.imageConvert rule)
        {
            this.rule = rule;

             cmbConvertType.Items.Clear();
            foreach(string s in Enum.GetNames(typeof(IntelliScraper.Db.imageConvertConvertType)))
                cmbConvertType.Items.Add(s);


            if (this.rule != null)
            {
                txtId.Text = this.rule.id;
                chkConvertFormat.IsChecked = this.rule.convertToFormat;
                chkResize.IsChecked = this.rule.changeSize;
                txtWidht.Text = this.rule.toSizew.ToString();
                txtHeight.Text = this.rule.toSizeH.ToString();
                chkCreateThumbs.IsChecked = this.rule.createThumbs;
                chkCreateThumbFolder.IsChecked = this.rule.createThumbFolder;
                if(rule.Thumbs != null)
                    listViewThumbs.ItemsSource = rule.Thumbs;

                cmbConvertType.SelectedValue = rule.convertType.ToString();
            }
                 
        }

        /// <summary>
        /// Add Thumb
        /// </summary>
        private void btnAddUAgent_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtThumbName.Text) && !string.IsNullOrEmpty(txtThumbH.Text) && !string.IsNullOrEmpty(txtThumbW.Text))
            {
                if (rule.Thumbs == null)
                    rule.Thumbs = new IntelliScraper.Db.imageConvertThumbsCollection();

                IntelliScraper.Db.imageConvertThumbs t = new IntelliScraper.Db.imageConvertThumbs();
                t.name = txtThumbName.Text;
                t.toSizeH = Int32.Parse(txtThumbH.Text);
                t.toSizeW =  Int32.Parse(txtThumbW.Text);

                rule.Thumbs.Add(t);
                Factory.Instance.Save();
                load(this.rule);
                CollectionViewSource.GetDefaultView(rule.Thumbs).Refresh();
            }
            else MessageBox.Show("Missing arguments (name or height or width)!", "Error");
        }

        /// <summary>
        /// Remove Thumb
        /// </summary>
        private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (listViewThumbs.SelectedIndex >= 0)
            {
                rule.Thumbs.RemoveAt(listViewThumbs.SelectedIndex);
                Factory.Instance.Save();
                load(this.rule);
                CollectionViewSource.GetDefaultView(rule.Thumbs).Refresh();
            }
        }

        public void save()
        {
            
            if(cmbConvertType.SelectedValue != null)
                rule.convertType = (IntelliScraper.Db.imageConvertConvertType)Enum.Parse(typeof(IntelliScraper.Db.imageConvertConvertType), (string)cmbConvertType.SelectedValue);

           this.rule.convertToFormat = (bool)chkConvertFormat.IsChecked;
           this.rule.changeSize= (bool)chkResize.IsChecked;
           this.rule.toSizew= Int32.Parse( txtWidht.Text);
           this.rule.toSizeH=  Int32.Parse(txtHeight.Text);
           this.rule.createThumbs = (bool)chkCreateThumbs.IsChecked;
           this.rule.createThumbFolder = (bool)chkCreateThumbFolder.IsChecked;
           
            cmbConvertType.SelectedValue = rule.convertType.ToString();

        }
    }
}
