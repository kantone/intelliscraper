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
using System.Windows.Controls.Primitives;

namespace IscraperBuilder.Controls.Project
{
    /// <summary>
    /// Logica di interazione per PrjInfo.xaml
    /// </summary>
    public partial class PrjInfo : Page
    {
        Utils.PopUp p = new Utils.PopUp();
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
            p.hide();
            if (validate())
            {
                setConfig();
                System.IO.File.Delete(Factory.Instance.openedFileProject);
                System.IO.FileStream s = System.IO.File.Create(Factory.Instance.openedFileProject);
                s.Close();
                IntelliScraper.Xml.Serialization.DeSerialize(Factory.Instance.i, Factory.Instance.openedFileProject);
                loadSetting();
                MainWindow.main.Status = "Saved!";
            }
            else p.show("Cannot save (errors)", button1);
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(txtPrjName.Text))
            {
                Utils.PopUp.showPopUpError("Mandatory!", txtPrjName);      
                return false;
            }

            if ((bool)showInitialMessage.IsChecked)
            {
                if (string.IsNullOrEmpty(txtInitialMessage.Text))
                {
                    Utils.PopUp.showPopUpError("Show intial message is mandatory", txtInitialMessage);                  
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// delete project
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format("Delete project {0}?", Factory.Instance.i.Project.ProjectInfo.projectName), "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                path += "\\projects\\";
                string prjFile = path + MainWindow.main.cmbPrjts.SelectedValue;
                int index = MainWindow.main.cmbPrjts.SelectedIndex;
                if (System.IO.File.Exists(prjFile))                             
                    System.IO.File.Delete(prjFile);

                MainWindow.main.cmbPrjts.SelectedIndex = 0;
                MainWindow.main.frame1.Content = null;
                MainWindow.main.frame1.NavigationService.RemoveBackEntry();
                MessageBox.Show(string.Format("Project {0} deleted!", Factory.Instance.i.Project.ProjectInfo.projectName));
                Factory.Instance.i = null;
                Factory.Instance.openedFileProject = null;
                MainWindow.main.cmbPrjts.Items.RemoveAt(index);
            }
        }
    }
}
