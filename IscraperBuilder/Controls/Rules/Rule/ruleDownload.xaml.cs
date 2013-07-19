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

namespace IscraperBuilder.Controls.Rules.Rule
{
    /// <summary>
    /// Logica di interazione per ruleDownload.xaml
    /// </summary>
    public partial class ruleDownload : Page,Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.download rule { get; set; }
        public ruleDownload(string id)
        {
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.download where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            txtMTMaxThread.PreviewTextInput += new TextCompositionEventHandler(txtonlyNubmers_PreviewTextInput);
            load();
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


        private void load()
        {
            txtID.Text = this.id;

            if (this.rule != null)
            {
                chkAutoRename.IsChecked = (bool)rule.autoRename;
                txtAttributeKey.Text = rule.inputAttributeKey;
                chkSetFileExt.IsChecked = (bool)rule.setFileExtension;
                txtFileExt.Text = rule.fileExtension;
                txtCustomUserAgent.Text = rule.customUserAgent;
                httpHeaderInfo1.load(rule.customHttpHeadersInfo);

                if (rule.DirSaveToFileInfo != null)
                {
                    txtStartFolder.Text = rule.DirSaveToFileInfo.startFolder;
                    chkGenerateDir.IsChecked = (bool)rule.DirSaveToFileInfo.generateSubFolder;
                    txtSubFolderCustomName.Text = rule.DirSaveToFileInfo.SubFolderCustomName;

                    cmbFolderGenType.Items.Clear();
                    foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType)))
                        cmbFolderGenType.Items.Add(s);

                    cmbFolderGenType.SelectedValue = rule.DirSaveToFileInfo.subfolderNameType.ToString();

                    chkGenerateDir_Checked(this, null);
                }

                if (rule.MultiThreadOption != null)
                {
                    chkMTEnable.IsChecked = rule.MultiThreadOption.enableMultithread;
                    chkMTSetMaxThread.IsChecked = rule.MultiThreadOption.setThreadMaxNumbers;
                    txtMTMaxThread.Text = rule.MultiThreadOption.ThreadNumbers.ToString();
                }

                if (rule.ImgConvertAction != null)
                {
                    imageConvert1.load(rule.ImgConvertAction);
                }
            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        private void cmbFolderGenType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFolderGenType.SelectedValue != null)
            {
                IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType t = (IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType)Enum.Parse(typeof(IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType),(string)cmbFolderGenType.SelectedValue);
                if (t != IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType.custom)
                    txtSubFolderCustomName.IsEnabled = false;
                else txtSubFolderCustomName.IsEnabled = true;
            }
        }

        private void chkGenerateDir_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)chkGenerateDir.IsChecked)
            {
                txtSubFolderCustomName.IsEnabled = true;
                if (cmbFolderGenType.SelectedValue != null)
                {
                    IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType t = (IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType)Enum.Parse(typeof(IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType),(string)cmbFolderGenType.SelectedValue);
                    if (t != IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType.custom)
                        txtSubFolderCustomName.IsEnabled = false;
                    else txtSubFolderCustomName.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            rule.autoRename = (bool)chkAutoRename.IsChecked;
            rule.inputAttributeKey = txtAttributeKey.Text;
            rule.setFileExtension = (bool)chkSetFileExt.IsChecked;
            rule.fileExtension = txtFileExt.Text;
            rule.customUserAgent = txtCustomUserAgent.Text;
            rule.customHttpHeadersInfo = httpHeaderInfo1.getHeaders();

            if (rule.DirSaveToFileInfo == null)
                rule.DirSaveToFileInfo = new IntelliScraper.Db.downloadDirSaveToFileInfo();

            rule.DirSaveToFileInfo.startFolder = txtStartFolder.Text;
            rule.DirSaveToFileInfo.generateSubFolder = (bool)chkGenerateDir.IsChecked;
            rule.DirSaveToFileInfo.SubFolderCustomName = txtSubFolderCustomName.Text;



            rule.DirSaveToFileInfo.subfolderNameType = (IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType)Enum.Parse(typeof(IntelliScraper.Db.downloadDirSaveToFileInfoSubfolderNameType), (string)cmbFolderGenType.SelectedValue);




            if (rule.MultiThreadOption == null)
                rule.MultiThreadOption = new IntelliScraper.Db.downloadMultiThreadOption();

            rule.MultiThreadOption.enableMultithread = (bool)chkMTEnable.IsChecked;
            rule.MultiThreadOption.setThreadMaxNumbers = (bool)chkMTSetMaxThread.IsChecked;
            rule.MultiThreadOption.ThreadNumbers = Int32.Parse(txtMTMaxThread.Text);

            this.imageConvert1.save();

            Factory.Instance.Save();
            load();
        }

        /// <summary>
        /// On select folder
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result ==  System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                txtStartFolder.Text = dialog.SelectedPath;
            }
        }
    }
}
