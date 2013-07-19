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
using IscraperBuilder.Controls.Rules.Rule;

namespace IscraperBuilder.Controls.Rules
{
    /// <summary>
    /// Logica di interazione per Rules.xaml
    /// </summary>
    public partial class ActionRules : Page
    {
        List<object> rules { get; set; }
        Action action { get; set; }
        IntelliScraper.Db.intelliScraperAction act { get; set; }
        bool isNew { get; set; }
        IRule ruleOpened { get; set; }
        public ActionRules()       
        {
            this.isNew = false;
            act = null;
            InitializeComponent(); 
            load();
        }

       
        private void load()
        {
            try
            {
                string[] types = Enum.GetNames(typeof(IntelliScraper.Db.intelliScraperActionType));
                if(cmbType.Items != null)
                    cmbType.Items.Clear();
                foreach (string t in types)
                    cmbType.Items.Add(t);

            }
            catch (Exception ex)
            {
                MainWindow.main.Status = "Error : " + ex.Message;
            }
        }

        public void setParentId(string id)
        {
            this.act = (from x in Factory.Instance.i.actions where x.id == id select x).FirstOrDefault();
        }

        private void loadItems()
        {           
            rules = new List<object>();
            if (Factory.Instance.i.rules.httpGet != null)
                rules.AddRange(Factory.Instance.i.rules.httpGet);

            if (Factory.Instance.i.rules.httpPost != null)
                rules.AddRange(Factory.Instance.i.rules.httpPost);

            if (Factory.Instance.i.rules.actionZip != null)
                rules.AddRange(Factory.Instance.i.rules.actionZip);

            if (Factory.Instance.i.rules.download != null)
                rules.AddRange(Factory.Instance.i.rules.download);

            if (Factory.Instance.i.rules.ftpPut != null)
                rules.AddRange(Factory.Instance.i.rules.ftpPut);

            if (Factory.Instance.i.rules.loop_link != null)
                rules.AddRange(Factory.Instance.i.rules.loop_link);

            if (Factory.Instance.i.rules.plugin != null)
                rules.AddRange(Factory.Instance.i.rules.plugin);

            if (Factory.Instance.i.rules.save != null)
                rules.AddRange(Factory.Instance.i.rules.save);

            if (Factory.Instance.i.rules.screenShot != null)
                rules.AddRange(Factory.Instance.i.rules.screenShot);

            if (Factory.Instance.i.rules.upload != null)
                rules.AddRange(Factory.Instance.i.rules.upload);

            if (Factory.Instance.i.rules.xpathCollection != null)
                rules.AddRange(Factory.Instance.i.rules.xpathCollection);

            if (Factory.Instance.i.rules.xpathSingle != null)
                rules.AddRange(Factory.Instance.i.rules.xpathSingle);

        }

        /// <summary>
        /// Add new Action
        /// </summary>
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.main.Status = "";
            string val = (string)cmbType.SelectedValue;
            IntelliScraper.Db.intelliScraperActionType t = (IntelliScraper.Db.intelliScraperActionType) Enum.Parse(typeof(IntelliScraper.Db.intelliScraperActionType), val);

            isNew = true;
            action = new Action(true, t, this, act);
            frame1.Navigate(action);
            
        }

      
       

        /// <summary>
        /// Load rule Frame by Rule Id
        /// </summary>
        /// <param name="ruleId"></param>
        public void LoadRuleFrame(string ruleId)
        {
            this.tabItem2.Header = ruleId;
            this.ruleOpened =  Factory.Instance.LoadRuleFrame(ruleId, frame2,false,null);
           
        }

       

       
    }
}
