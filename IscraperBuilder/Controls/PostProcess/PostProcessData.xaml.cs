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
    /// Logica di interazione per PostProcessData.xaml
    /// </summary>
    public partial class PostProcessData : Page
    {
        public PostProcessData()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            cmbType.Items.Clear();
            foreach (string s in Factory.Instance.getAllPostProcessObjTypesName())
                cmbType.Items.Add(s);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(Factory.Instance.i.postProcess == null)
                Factory.Instance.i.postProcess = new IntelliScraper.Db.intelliScraperPostProcess();

            if (cmbType.SelectedValue != null && !string.IsNullOrEmpty(txtId.Text))
            {
                string type = (string)cmbType.SelectedValue;
                if (type.ToLower() == typeof(IntelliScraper.Db.replace).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.replace == null)
                        Factory.Instance.i.postProcess.replace = new IntelliScraper.Db.replaceCollection();
                    IntelliScraper.Db.replace r = new IntelliScraper.Db.replace();
                    r.id= txtId.Text;
                    Factory.Instance.i.postProcess.replace.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.Replace(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.regularExpression).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.regularExpression == null)
                        Factory.Instance.i.postProcess.regularExpression = new IntelliScraper.Db.regularExpressionCollection();
                    IntelliScraper.Db.regularExpression r = new IntelliScraper.Db.regularExpression();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.regularExpression.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.Regex(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.substring).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.substring == null)
                        Factory.Instance.i.postProcess.substring = new IntelliScraper.Db.substringCollection();
                    IntelliScraper.Db.substring r = new IntelliScraper.Db.substring();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.substring.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.SubString(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.trim).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.trim == null)
                        Factory.Instance.i.postProcess.trim = new IntelliScraper.Db.trimCollection();
                    IntelliScraper.Db.trim r = new IntelliScraper.Db.trim();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.trim.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.Trim(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.startEndWith).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.startEndWith == null)
                        Factory.Instance.i.postProcess.startEndWith = new IntelliScraper.Db.startEndWithCollection();
                    IntelliScraper.Db.startEndWith r = new IntelliScraper.Db.startEndWith();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.startEndWith.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.StartEndWith(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.append).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.append == null)
                        Factory.Instance.i.postProcess.append = new IntelliScraper.Db.appendCollection();
                    IntelliScraper.Db.append r = new IntelliScraper.Db.append();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.append.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.Append(r));
                }

                if (type.ToLower() == typeof(IntelliScraper.Db.htmlEncodeDecode).Name.ToLower())
                {
                    if (Factory.Instance.i.postProcess.htmlEncodeDecode == null)
                        Factory.Instance.i.postProcess.htmlEncodeDecode = new IntelliScraper.Db.htmlEncodeDecodeCollection();
                    IntelliScraper.Db.htmlEncodeDecode r = new IntelliScraper.Db.htmlEncodeDecode();
                    r.id = txtId.Text;
                    Factory.Instance.i.postProcess.htmlEncodeDecode.Add(r);
                    Factory.Instance.Save();
                    mainLoad();
                    frame1.Navigate(new Controls.PostProcess.HtmlEncodeDecode(r));
                }
            }
            else MessageBox.Show("Missing values", "Error");
        }

        private void mainLoad()
        {
            //MainWindow.main.
            //MainWindow.main.loadRules();
            MainWindow.main.loadPostProcessData();
        }

    }
}
