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
    /// Logica di interazione per RuleLoopLink.xaml
    /// </summary>
    public partial class RuleLoopLink : Page, Rule.IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.loop_link rule { get; set; }
        public RuleLoopLink(string id)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.loop_link where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();

            load();
            txtThreadNum.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSECustomStart.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSECustomEnd.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSEEndAddInt.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);
            txtSEStartAddInt.PreviewTextInput += new TextCompositionEventHandler(txtThreadNum_PreviewTextInput);

        }

        /// <summary>
        /// Only Numbers
        /// </summary>       
        void txtThreadNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
            tabControl2.Visibility = System.Windows.Visibility.Hidden;
            cmbInputType.Items.Clear();           
            foreach (string v in Enum.GetNames(typeof(IntelliScraper.Db.loop_linkType)))
                cmbInputType.Items.Add(v);

            

            if (rule != null)
            {
                txtId.Text = rule.id;
                cmbInputType.SelectedValue = rule.type.ToString();
                cmbInputType_SelectionChanged(this, null);

                //Multithread
                if (rule.multiThreadOption != null)
                {
                    chkThreadEnabled.IsChecked = rule.multiThreadOption.enableMultithread;
                    chkThreadSetNumbers.IsChecked = rule.multiThreadOption.setThreadMaxNumbers;
                    txtThreadNum.Text = rule.multiThreadOption.ThreadNumbers.ToString();
                }

                //Start end Data
                if (rule.StartEndData != null)
                {
                    txtCustomUrl.Text = rule.StartEndData.url;
                    chkSEGetEndPage.IsChecked = rule.StartEndData.getEndPageNumberFromRule;
                    chkSEGetStartPage.IsChecked = rule.StartEndData.getStartPageNumberFromRule;
                    txtSECustomUrl.Text = rule.StartEndData.url;
                    txtSECustomStart.Text = rule.StartEndData.start.ToString();
                    txtSECustomEnd.Text = rule.StartEndData.end.ToString();
                    txtSEEndAttributeId.Text = rule.StartEndData.actionAttributeId_getEndPage;
                    txtSEStartAttributeId.Text = rule.StartEndData.actionAttributeId_getStartPage;
                    txtSEStartAddInt.Text = rule.StartEndData.appendStartVal.ToString();
                    txtSEEndAddInt.Text = rule.StartEndData.appendEndVal.ToString();
                    txtSEAppendBeforePageNum.Text = rule.StartEndData.appendBeforePageNumber;
                    txtSEAppendAfterPageNum.Text = rule.StartEndData.appendAfterPageNumber;
                }

                //Xpath
                cmbXpathType.Items.Clear();
                foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.loop_linkXpathType)))
                    cmbXpathType.Items.Add(s);

                txtXpathPre.Text = rule.preXpath;
                cmbXpathType.SelectedValue = rule.xpathType.ToString();
                cmbXpathType_SelectionChanged(this, null);

                cmbXpathRules.Items.Clear();
                if (rule.xpathSingle != null)
                {
                    foreach (var r in rule.xpathSingle)
                        cmbXpathRules.Items.Add(r.id);
                }
            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        
                


        /// <summary>
        /// Type Selection
        /// </summary>
        private void cmbInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInputType.SelectedValue != null)
            {
                IntelliScraper.Db.loop_linkType t = (IntelliScraper.Db.loop_linkType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkType), (string)cmbInputType.SelectedValue);

                tbCsv.Visibility = System.Windows.Visibility.Hidden;
                tbDb.Visibility = System.Windows.Visibility.Hidden;
                tbXls.Visibility = System.Windows.Visibility.Hidden;
                txtCustomUrl.IsEnabled = false;
                txtInputAttributeKey.IsEnabled = false;
                tabControl2.Visibility = System.Windows.Visibility.Visible;

                if (t == IntelliScraper.Db.loop_linkType.fromInputAttributes)
                {
                    txtInputAttributeKey.IsEnabled = true;
                    tabControl2.Visibility = System.Windows.Visibility.Hidden;
                }

                if (t == IntelliScraper.Db.loop_linkType.customUrl)
                {
                    txtCustomUrl.IsEnabled = true;
                    tabControl2.Visibility = System.Windows.Visibility.Hidden;
                }

                if (t == IntelliScraper.Db.loop_linkType.fromCsv)
                {
                    tabControl2.SelectedIndex = 0;
                    tbCsv.Visibility = System.Windows.Visibility.Visible;
                    cmbCsvStoreId.Items.Clear();
                    var dt = from x in Factory.Instance.i.Project.StoreInfo where x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.csv select x;
                    foreach (var s in dt)
                        cmbCsvStoreId.Items.Add(s.Id);
                    cmbCsvStoreId.SelectedValue = rule.csvInputDefinition.storeId;
                    cmbCsvStoreId_SelectionChanged(this, null);
                }

                if (t == IntelliScraper.Db.loop_linkType.fromExcel)
                {
                    cmbExcelStoreId.Items.Clear();
                    var dt = from x in Factory.Instance.i.Project.StoreInfo where x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.excel select x;
                    foreach (var s in dt)
                        cmbExcelStoreId.Items.Add(s.Id);

                    if (rule.excelInputDefinition != null)                    
                        cmbExcelStoreId.SelectedValue = rule.excelInputDefinition.storeId;

                    tabControl2.SelectedIndex = 1;
                    tbXls.Visibility = System.Windows.Visibility.Visible;
                    cmbExcelStoreId_SelectionChanged(this, null);
                }

                if (t == IntelliScraper.Db.loop_linkType.fromDatabase)
                {
                    cmbDbType.Items.Clear();
                    foreach(string s in Enum.GetNames(typeof(IntelliScraper.Db.loop_linkDatabaseInputDefinitionType)))
                        cmbDbType.Items.Add(s);

                   
                    
                     cmbDbStoreId.Items.Clear();
                    var dt = from x in Factory.Instance.i.Project.StoreInfo where 
                                 x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.MySql || 
                             x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.Oracle || 
                             x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.PostgreSQL || 
                             x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.sqlServer || 
                             x.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.sqlServerCE 
                             select x;
                    foreach (var s in dt)
                        cmbDbStoreId.Items.Add(s.Id);

                    if (rule.databaseInputDefinition != null)
                    {
                        cmbDbType.SelectedValue = rule.databaseInputDefinition.type.ToString();
                        cmbDbStoreId.SelectedValue = rule.databaseInputDefinition.storeId;
                        txtDbColName.Text = rule.databaseInputDefinition.columnName;
                        txtDbQuery.Text = rule.databaseInputDefinition.query;
                        txtDbTableName.Text = rule.databaseInputDefinition.tableName;
                    }

                   
                    

                    tabControl2.SelectedIndex = 2;
                    tbDb.Visibility = System.Windows.Visibility.Visible;
                    cmbDbType_SelectionChanged(this, null);
                }
            }
        }

        /// <summary>
        /// Csv Store id Selection
        /// </summary>
        private void cmbCsvStoreId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCsvStoreId.SelectedValue != null)
            {
               // IntelliScraper.Db.loop_linkType t = (IntelliScraper.Db.loop_linkType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkType), (string)cmbInputType.SelectedValue);

                var store = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == (string)cmbCsvStoreId.SelectedValue select x).FirstOrDefault();
                if (store != null)
                {
                   
                        cmbCsvStoreColName.Items.Clear();
                        if (store.CsvExcelSetting.csvHeader != null)
                        {
                            foreach (string s in store.CsvExcelSetting.csvHeader.Split(store.CsvExcelSetting.csvSeparator.ToCharArray()))
                                cmbCsvStoreColName.Items.Add(s);

                            cmbCsvStoreColName.SelectedValue = rule.csvInputDefinition.columnName;
                        }
                    
      
                }
            }
        }


        /// <summary>
        /// Excel Store id Selection
        /// </summary>
        private void cmbExcelStoreId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbExcelStoreId.SelectedValue != null)
            {
                IntelliScraper.Db.loop_linkType t = (IntelliScraper.Db.loop_linkType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkType), (string)cmbInputType.SelectedValue);

                var store = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == (string)cmbExcelStoreId.SelectedValue select x).FirstOrDefault();
                cmbExcelStoreCol.Items.Clear();
                if (store.CsvExcelSetting.csvHeader != null)
                {
                    foreach (string s in store.CsvExcelSetting.csvHeader.Split(store.CsvExcelSetting.csvSeparator.ToCharArray()))
                        cmbExcelStoreCol.Items.Add(s);

                    txtExcelSheetName.Text = rule.excelInputDefinition.sheetName;
                    txtExcelSheetName.IsEnabled = true;

                  
                            string[] cols = store.CsvExcelSetting.csvHeader.Split(store.CsvExcelSetting.csvSeparator.ToCharArray());
                            cmbExcelStoreCol.SelectedValue = cols[rule.excelInputDefinition.columnIndex];
                  

                    txtExcelSheetName.Text = rule.excelInputDefinition.sheetName;
                }
            }
        }

        /// <summary>
        /// Db Type Change
        /// </summary>
        private void cmbDbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDbType.SelectedValue != null)
            {
                

                IntelliScraper.Db.loop_linkDatabaseInputDefinitionType t = (IntelliScraper.Db.loop_linkDatabaseInputDefinitionType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkDatabaseInputDefinitionType), (string)cmbDbType.SelectedValue);
                if (t == IntelliScraper.Db.loop_linkDatabaseInputDefinitionType.query)
                {
                    txtDbColName.IsEnabled = false;
                    txtDbTableName.IsEnabled = false;
                    txtDbQuery.IsEnabled = true;
                }

                if (t == IntelliScraper.Db.loop_linkDatabaseInputDefinitionType.readAllTableColumn)
                {
                    txtDbColName.IsEnabled = true;
                    txtDbTableName.IsEnabled = true;
                    txtDbQuery.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            if (rule.StartEndData == null)
                rule.StartEndData = new IntelliScraper.Db.loop_linkStartEndData();
            if (rule.csvInputDefinition == null)
                rule.csvInputDefinition = new IntelliScraper.Db.loop_linkCsvInputDefinition();
            if (rule.databaseInputDefinition == null)
                rule.databaseInputDefinition = new IntelliScraper.Db.loop_linkDatabaseInputDefinition();
            if (rule.excelInputDefinition == null)
                rule.excelInputDefinition = new IntelliScraper.Db.loop_linkExcelInputDefinition();
            if (rule.multiThreadOption == null)
                rule.multiThreadOption = new IntelliScraper.Db.loop_linkMultiThreadOption();
            if (rule.xpathSingle == null)
                rule.xpathSingle = new IntelliScraper.Db.xpathSingleCollection();

            if (cmbInputType.SelectedValue != null)
                rule.type = (IntelliScraper.Db.loop_linkType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkType), (string)cmbInputType.SelectedValue);

            rule.inputAttributeKey = txtInputAttributeKey.Text;
            rule.inputAttributeKey = txtInputAttributeKey.Text;

            rule.csvInputDefinition.storeId = (string)cmbCsvStoreId.Text;
            rule.csvInputDefinition.columnName = (string)cmbCsvStoreColName.Text;

            rule.excelInputDefinition.columnIndex = cmbExcelStoreCol.SelectedIndex;
            rule.excelInputDefinition.sheetName = txtExcelSheetName.Text;
            rule.excelInputDefinition.storeId = (string)cmbExcelStoreId.SelectedValue;

            rule.databaseInputDefinition.storeId = (string)cmbDbStoreId.Text;
            rule.databaseInputDefinition.query = txtDbQuery.Text;
            rule.databaseInputDefinition.tableName = txtDbTableName.Text;
            rule.databaseInputDefinition.columnName = txtDbColName.Text;
            if(cmbDbType.SelectedValue != null)
                rule.databaseInputDefinition.type = (IntelliScraper.Db.loop_linkDatabaseInputDefinitionType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkDatabaseInputDefinitionType),(string)cmbDbType.SelectedValue);

            rule.multiThreadOption.enableMultithread = (bool)chkThreadEnabled.IsChecked;
            rule.multiThreadOption.setThreadMaxNumbers = (bool)chkThreadSetNumbers.IsChecked;
            rule.multiThreadOption.ThreadNumbers = Int32.Parse(txtThreadNum.Text);

            rule.StartEndData.actionAttributeId_getEndPage = txtSEEndAttributeId.Text;
            rule.StartEndData.actionAttributeId_getStartPage = txtSEStartAttributeId.Text;
            rule.StartEndData.appendAfterPageNumber = txtSEAppendAfterPageNum.Text;
            rule.StartEndData.appendBeforePageNumber = txtSEAppendBeforePageNum.Text;
            rule.StartEndData.appendEndVal = Int32.Parse(txtSEEndAddInt.Text);
            rule.StartEndData.appendStartVal = Int32.Parse(txtSEStartAddInt.Text);
            rule.StartEndData.end = Int32.Parse(txtSECustomEnd.Text);
            rule.StartEndData.getEndPageNumberFromRule = (bool) chkSEGetEndPage.IsChecked;
            rule.StartEndData.getStartPageNumberFromRule =(bool) chkSEGetStartPage.IsChecked;
            rule.StartEndData.start = Int32.Parse(txtSECustomStart.Text);
            rule.StartEndData.url = txtSECustomUrl.Text;

            if(cmbXpathType.SelectedValue != null)
                rule.xpathType = (IntelliScraper.Db.loop_linkXpathType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkXpathType), (string)cmbXpathType.SelectedValue);

            rule.preXpath = txtXpathPre.Text;

            Factory.Instance.Save();
            load();
        }

        /// <summary>
        /// Check get end page
        /// </summary>
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {

            txtSECustomEnd.IsEnabled = true;
            txtSEEndAddInt.IsEnabled = true;
            txtSEEndAttributeId.IsEnabled = true;

            if (!(bool)chkSEGetStartPage.IsChecked)
            {
                txtSECustomStart.IsEnabled = false;
                txtSEStartAddInt.IsEnabled = false;
                txtSEStartAttributeId.IsEnabled = false;
            }
        }

        /// <summary>
        /// Check get start page
        /// </summary>
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (!(bool)chkSEGetEndPage.IsChecked)
            {
                txtSECustomEnd.IsEnabled = false;
                txtSEEndAddInt.IsEnabled = false;
                txtSEEndAttributeId.IsEnabled = false;
            }

            txtSECustomStart.IsEnabled = true;
            txtSEStartAddInt.IsEnabled = true;
            txtSEStartAttributeId.IsEnabled = true;
        }
                

        /// <summary>
        /// Xpath Type changed
        /// </summary>
        private void cmbXpathType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbXpathType.SelectedValue != null)
            {
                IntelliScraper.Db.loop_linkXpathType t = (IntelliScraper.Db.loop_linkXpathType)Enum.Parse(typeof(IntelliScraper.Db.loop_linkXpathType), (string)cmbXpathType.SelectedValue);
                if (t == IntelliScraper.Db.loop_linkXpathType.single)
                {
                    txtXpathPre.IsEnabled = false;
                }
                if (t == IntelliScraper.Db.loop_linkXpathType.collection)
                {
                    txtXpathPre.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Edit rule
        /// </summary>
        private void cmbXpathRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbXpathRules.SelectedValue != null)
            {
                IntelliScraper.Db.xpathCollection coll = new IntelliScraper.Db.xpathCollection();
                coll.id = "loopLink";
                coll.xpath = new string[0];
                coll.xpathSingle = new IntelliScraper.Db.xpathSingleCollection();
                foreach (var r in rule.xpathSingle)
                    coll.xpathSingle.Add(r);


                xpathSingle1.load((string)cmbXpathRules.SelectedValue, true, coll);
            }
        }

        /// <summary>
        /// Add new xpath rule
        /// </summary>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtXpathAddNew.Text))
            {
                if (rule.xpathSingle == null)
                    rule.xpathSingle = new IntelliScraper.Db.xpathSingleCollection();

                IntelliScraper.Db.xpathSingle s = new IntelliScraper.Db.xpathSingle();
                s.id = txtXpathAddNew.Text;
                s.attributes = new IntelliScraper.Db.xpathSingleAttributesCollection();
                s.postProcessTriggerGroup = new IntelliScraper.Db.xpathSinglePostProcessTriggerGroupCollection();
                rule.xpathSingle.Add(s);
                Factory.Instance.Save();

                load();

                cmbXpathRules.SelectedValue = s.id;

                IntelliScraper.Db.xpathCollection coll = new IntelliScraper.Db.xpathCollection();
                coll.id = "loopLink";
                coll.xpath = new string[0];
                coll.xpathSingle = new IntelliScraper.Db.xpathSingleCollection();
                coll.xpathSingle.Add(s);
                xpathSingle1.load(txtXpathAddNew.Text, true, coll);
            }
        }

        /// <summary>
        /// Delete xpath rule
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (cmbXpathRules.SelectedValue != null)
            {
                rule.xpathSingle.RemoveAt(cmbXpathRules.SelectedIndex);
                Factory.Instance.Save();
                load();
                xpathSingle1.ruleName.Text = "{Null}";
            }
        }
    }
}
