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

namespace IscraperBuilder.Controls.Project
{
    /// <summary>
    /// Logica di interazione per PrjInfo.xaml
    /// </summary>
    public partial class PrjInfo : Page
    {
        public PrjInfo()
        {
            MainWindow.main.Status = "";
            InitializeComponent();
            loadSetting();
        }
        private void loadSetting()
        {
            this.txtPrjName.Text = Factory.Instance.i.Project.ProjectInfo.projectName;
            this.txtInitialMessage.Text = Factory.Instance.i.Project.ProjectInfo.initialMessage;
            this.showInitialMessage.IsChecked = Factory.Instance.i.Project.ProjectInfo.showInitialMessage;
            this.verboseLog.IsChecked = Factory.Instance.i.Project.ProjectInfo.logIscraperInfo;
        }

        private void setConfig()
        {
            Factory.Instance.i.Project.ProjectInfo.projectName = this.txtPrjName.Text;
            Factory.Instance.i.Project.ProjectInfo.initialMessage = this.txtInitialMessage.Text;
            Factory.Instance.i.Project.ProjectInfo.showInitialMessage = (bool) this.showInitialMessage.IsChecked;
            Factory.Instance.i.Project.ProjectInfo.logIscraperInfo = (bool)this.verboseLog.IsChecked;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            setConfig();
            System.IO.File.Delete(Factory.Instance.openedFileProject);
            System.IO.FileStream s = System.IO.File.Create(Factory.Instance.openedFileProject);
            s.Close();
            IntelliScraper.Xml.Serialization.DeSerialize(Factory.Instance.i, Factory.Instance.openedFileProject);
            loadSetting();
            MainWindow.main.Status = "Saved!";
        }
    }
}
