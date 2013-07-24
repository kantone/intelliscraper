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
    /// Logica di interazione per HttpHeaderInfo.xaml
    /// </summary>
    public partial class HttpHeaderInfo : UserControl
    {
        IntelliScraper.Db.HttpHeadersInfoCollection headers;
        public HttpHeaderInfo()
        {
            InitializeComponent();
        }

        public IntelliScraper.Db.HttpHeadersInfoCollection getHeaders()
        {
            return headers;
        }

        public void load(IntelliScraper.Db.HttpHeadersInfoCollection headers)
        {
            if (headers != null)
            {
                this.headers = headers;
                if (Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo != null)
                {
                    //var sequence = Factory.Instance.i.Project.ScrapingSetting.GlobalHttpHeadersInfo;
                    listViewHttpHeaders.ItemsSource = this.headers;
                }
            }
            else this.headers = new IntelliScraper.Db.HttpHeadersInfoCollection();
        }

        /// <summary>
        /// Add Http Header
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHname.Text) && !string.IsNullOrEmpty(txtHValue.Text))
            {
                IntelliScraper.Db.HttpHeadersInfo i = new IntelliScraper.Db.HttpHeadersInfo();
                i.name = txtHname.Text;
                i.value = txtHValue.Text;
                headers.Add(i);
                load(this.headers);
                CollectionViewSource.GetDefaultView(headers).Refresh();
            }
            else MessageBox.Show("Name & Value Mandatory!", "Error");
        }

        /// <summary>
        /// Delete Header
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {

            if (listViewHttpHeaders.SelectedIndex >= 0)
            {
                headers.RemoveAt(listViewHttpHeaders.SelectedIndex);
                load(this.headers);
                CollectionViewSource.GetDefaultView(headers).Refresh();
            }
        }
    }
}
