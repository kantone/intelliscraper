﻿using System;
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
    /// Logica di interazione per ruleSave.xaml
    /// </summary>
    public partial class ruleSave : Page,IRule
    {
        public string id { get; set; }
        public bool isNew { get; set; }
        IntelliScraper.Db.save rule { get; set; }
        public ruleSave(string id)
        {
            if (string.IsNullOrEmpty(id))
                isNew = true;
            else
            {
                isNew = false;
                this.id = id;
                rule = (from x in Factory.Instance.i.rules.save where x.id == id select x).FirstOrDefault();
            }
            InitializeComponent();


            load();
        }

        private void load()
        {
            cmbStoreId.Items.Clear();
            foreach (var r in Factory.Instance.i.Project.StoreInfo)
                cmbStoreId.Items.Add(r.Id);

            cmbExistAction.Items.Clear();
            foreach (string s in Enum.GetNames(typeof(IntelliScraper.Db.saveIfExist)))
                cmbExistAction.Items.Add(s);

            cmbMapActionId.Items.Clear();
            foreach (var s in Factory.Instance.i.actions)
                cmbMapActionId.Items.Add(s.id);

            if (rule != null)
            {
                txtTableName.Text = rule.tableName;
                chkSaveAllFromInput.IsChecked = rule.saveAllFromInput;

                chkCheckExist.IsChecked = rule.checkExistBeforeSave;
                txtCheckExistAttrs.Text = rule.checkExistAttributes;
                txtCheckExistSeparator.Text = rule.checkExistAttributesSeparator;

                cmbExistAction.SelectedValue = rule.ifExist.ToString();
                cmbStoreId.SelectedValue = rule.storeId;
                cmbStoreId_SelectionChanged(this, null);

                if (rule.map != null)
                {
                    dataGrid1.Items.Clear();
                    foreach(var m in rule.map)
                        dataGrid1.Items.Add(m);
                }

                var store = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == rule.storeId select x).FirstOrDefault();
                if (store != null)
                {
                    if ((store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.csv || store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.excel) && store.CsvExcelSetting != null)
                    {
                        foreach (string s in store.CsvExcelSetting.csvHeader.Split(store.CsvExcelSetting.csvSeparator.ToCharArray()))
                            cmbMapColName.Items.Add(s);
                    }     
                }

                cmbAttributes.Items.Clear();
                List<string> attrs = new List<string>();
                if (Factory.Instance.i.rules.xpathSingle != null)
                {
                    foreach (var o in Factory.Instance.i.rules.xpathSingle)
                    {
                        foreach (var attr in o.attributes)
                            attrs.Add(attr.id);
                    }
                }
                if (Factory.Instance.i.rules.xpathCollection != null)
                {
                    foreach (var c in Factory.Instance.i.rules.xpathCollection)
                    {
                        foreach (var o in c.xpathSingle)
                        {
                            foreach (var attr in o.attributes)
                                attrs.Add(attr.id);
                        }
                    }
                }

                attrs.Remove(string.Empty);
                attrs = attrs.Distinct().ToList();
                foreach(string attr in attrs)
                    cmbAttributes.Items.Add(attr);
            }
        }


        public string save()
        {
            throw new NotImplementedException();
        }

        private void cmbStoreId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStoreId.SelectedValue != null)
            {
                string id = (string)cmbStoreId.SelectedValue;
                var store = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == id select x).FirstOrDefault();
                if (store != null)
                {
                    if (store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.MySql ||
                        store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.Oracle ||
                        store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.PostgreSQL ||
                        store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.sqlServer ||
                        store.type == IntelliScraper.Db.intelliScraperProjectStoreInfoType.sqlServerCE)
                    {
                        txtTableName.IsEnabled = true;
                    }
                    else txtTableName.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Map action id select load map column Name
        /// </summary>
        private void cmbMapActionId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void cmbMapColName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMapColName.SelectedValue != null)
                txtMapColName.Text = (string)cmbMapColName.SelectedValue;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMapActionId.SelectedValue != null && !string.IsNullOrEmpty(txtMapColName.Text) && !string.IsNullOrEmpty(txtAttributeId.Text)
                && !string.IsNullOrEmpty(txtXpathCollPosition.Text))
            {
                IntelliScraper.Db.saveMap m = new IntelliScraper.Db.saveMap();
                m.actionId = (string)cmbMapActionId.SelectedValue;
                m.attributeId = txtAttributeId.Text;
                m.inputIsXpathCollection = (bool)chkInputIsCollection.IsChecked;
                m.mapColName = txtMapColName.Text;
                m.xpathCollectionPosition = Int32.Parse(txtXpathCollPosition.Text);

                if (rule.map == null)
                    rule.map = new IntelliScraper.Db.saveMapCollection();
                rule.map.Add(m);
                Factory.Instance.Save();
                load();
            }
            else MessageBox.Show("Missing some attributes!", "Error");
        }

        private void button2a_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedIndex >= 0)
            {
                rule.map.RemoveAt(dataGrid1.SelectedIndex);
                Factory.Instance.Save();
                load();
            }
        }

        private void cmbAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAttributes.SelectedValue != null)
                txtAttributeId.Text = (string)cmbAttributes.SelectedValue;
        }
    }
}