using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.IO;

namespace IntelliScraper.Scrape.Action
{
    public class Save
    {
        Db.save save { get; set; }
        Db.intelliScraperProjectStoreInfo storeInfo { get; set; }
        int csvExistRowPosition { get; set; }
        string csvCurrentRowValue { get; set; }
        bool cleared = false;
        //bool isLast = false;
        int lastRow = 1;
        int existRowIndex = 0;
        public Save(Db.save save)
        {
            this.csvExistRowPosition = -1;
            this.save = save;
            this.storeInfo = (from x in Factory.Instance.i.Project.StoreInfo where x.Id == save.storeId select x).FirstOrDefault();
        }

        /// <summary>
        /// Run save from input data
        /// </summary>
        public List<KeyValuePair<string, object>> Run(List<List<KeyValuePair<string, object>>> data)
        {
            List<KeyValuePair<string, object>> cols = new List<KeyValuePair<string, object>>();
            int i = 0;
            foreach (List<KeyValuePair<string, object>> mvals in data)
            {
                cols = new List<KeyValuePair<string, object>>();
                List<KeyValuePair<string, object>> valsx = (List<KeyValuePair<string, object>>)mvals;
                foreach (Db.saveMap map in save.map)
                {
                    KeyValuePair<string, object> kval = (from x in valsx where x.Key == map.attributeId select x).FirstOrDefault();
                    cols.Add(new KeyValuePair<string, object>(map.mapColName, (string)kval.Value));
                }
                singleSaveWrapper(cols, storeInfo);

                i++;
            }
            return cols;
        }

        /// <summary>
        /// Run save from input data
        /// </summary>
        public List<KeyValuePair<string, object>> Run(List<KeyValuePair<string, object>> input)
        {
           
            List<KeyValuePair<string, object>> cols = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> valsx = (List<KeyValuePair<string, object>>)input;
            foreach (Db.saveMap map in save.map)
            {
                KeyValuePair<string, object> kval = (from x in valsx where x.Key == map.attributeId select x).FirstOrDefault();
                cols.Add(new KeyValuePair<string, object>(map.mapColName, (string)kval.Value));
            }
            singleSaveWrapper(cols, storeInfo);
            return cols;
        }
                   
        

        /// <summary>
        /// Run save by store Configuration
        /// </summary>
        private void singleSaveWrapper(List<KeyValuePair<string, object>> cols, Db.intelliScraperProjectStoreInfo storeInfo)
        {
            //csv
            if (storeInfo.type == Db.intelliScraperProjectStoreInfoType.csv)
            {
                if (!cleared)
                {
                    if (storeInfo.CsvExcelSetting.csvFileClear && System.IO.File.Exists(storeInfo.CsvExcelSetting.csvFileSaveTo))
                    {
                        System.IO.File.Delete(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        cleared = true;
                    }
                }
                csvSaveAction(cols, storeInfo);
            }

            //Database
            if (storeInfo.type == Db.intelliScraperProjectStoreInfoType.MySql || storeInfo.type == Db.intelliScraperProjectStoreInfoType.Oracle || storeInfo.type == Db.intelliScraperProjectStoreInfoType.PostgreSQL || storeInfo.type == Db.intelliScraperProjectStoreInfoType.sqlServer || storeInfo.type == Db.intelliScraperProjectStoreInfoType.sqlServerCE)
                databaseSaveAction(cols, storeInfo);

            //excel
            if (storeInfo.type == Db.intelliScraperProjectStoreInfoType.excel)
            {
                if (!cleared)
                {
                    //delete file
                    if (storeInfo.CsvExcelSetting.csvFileClear && System.IO.File.Exists(storeInfo.CsvExcelSetting.csvFileSaveTo))
                    {
                        System.IO.File.Delete(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        cleared = true;
                    }
                    //create file
                    if(!System.IO.File.Exists(storeInfo.CsvExcelSetting.csvFileSaveTo))
                    {
                        FileInfo newFile = new FileInfo(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        ExcelPackage _book = new ExcelPackage(newFile);
                         _book.Workbook.Worksheets.Add(storeInfo.CsvExcelSetting.workSheetName);
                         _book.Save();                       
                        cleared = true;
                    }
                }
                excelAddRow(cols, storeInfo);
               
            }

            //if (storeInfo.type == Db.intelliScraperProjectStoreInfoType.xml)
            //if (storeInfo.type == Db.intelliScraperProjectStoreInfoType.json) ;
        }

         /// <summary>
        /// Insert Result to database
        /// </summary>
        private void excelAddRow(List<KeyValuePair<string, object>> cols, Db.intelliScraperProjectStoreInfo storeInfo)
        {
            FileInfo newFile = new FileInfo(storeInfo.CsvExcelSetting.csvFileSaveTo);
            ExcelPackage book = new ExcelPackage(newFile);            
            List<string> headers = new List<string>(storeInfo.CsvExcelSetting.csvHeader.Split(storeInfo.CsvExcelSetting.csvSeparator.ToCharArray()));
            
            if (save.checkExistBeforeSave)
            {
                 //find colums to check
                bool exist = excelExist(book,storeInfo,headers,cols); 

                //Exist
                if (exist)
                {
                    if (save.ifExist == Db.saveIfExist.stopAll)
                        Environment.Exit(0);

                    //delete row
                    if (save.ifExist == Db.saveIfExist.delete)
                    {
                        book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].DeleteRow(existRowIndex, 1);
                        book.Save();
                    }

                    //update row
                    if (save.ifExist == Db.saveIfExist.update)
                    {

                        foreach (string h in headers)
                        {
                            int c=1;
                            foreach (KeyValuePair<string, object> col in cols)
                            {
                                if (h == col.Key)
                                {
                                    book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Cells[existRowIndex, c].Value = col.Value;
                                }                                 
                            }
                            c++;
                        }
                        book.Save();                        
                    }
                    if (save.ifExist == Db.saveIfExist.insert)
                    {
                        addExcelRow(book,storeInfo,headers,cols);
                    }
                }
                else   addExcelRow(book,storeInfo,headers,cols);  
            }
            else   addExcelRow(book,storeInfo,headers,cols);              
        }

        /// <summary>
        /// Add Row To excel
        /// </summary>
        private void addExcelRow(ExcelPackage book, Db.intelliScraperProjectStoreInfo storeInfo, List<string> headers, List<KeyValuePair<string, object>> cols)
        {
            if (book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName] != null)
            {

                if (book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Dimension != null)
                    lastRow = book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Dimension.End.Row + 1;

                int i = 1;
                foreach (string h in headers)
                {
                    foreach (KeyValuePair<string, object> o in cols)
                    {
                        if (h == o.Key)
                            book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Cells[lastRow, i].Value = o.Value;
                    }
                    i++;
                }
            }
            lastRow++;
            book.Save();  
        }

        /// <summary>
        /// Check if row exist in excel
        /// </summary>
        private bool excelExist(ExcelPackage book, Db.intelliScraperProjectStoreInfo storeInfo, List<string> headers, List<KeyValuePair<string, object>> cols)
        {
            bool exist = false;
           
            //File is not void
            if (book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Dimension != null)
            {
                //trasform input to row string
                string rowTocheck = string.Empty;
                foreach (string h in headers)
                {
                    foreach (KeyValuePair<string, object> col in cols)
                    {
                        if (h == col.Key)
                            rowTocheck += col.Value + storeInfo.CsvExcelSetting.csvSeparator;
                    }

                }

                //check exist by trasform columns to rows string and compare
                for (int i = 1; i < book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Dimension.End.Row + 1; i++)
                {
                    string currentRow = string.Empty;
                    for (int x = 1; x < book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Dimension.End.Column + 1; x++)
                    {
                        currentRow += book.Workbook.Worksheets[storeInfo.CsvExcelSetting.workSheetName].Cells[i, x].Value + storeInfo.CsvExcelSetting.csvSeparator;
                    }

                    if (currentRow == rowTocheck)
                    {
                        existRowIndex = i;
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

        /// <summary>
        /// Insert Result to database
        /// </summary>
        private void databaseSaveAction(List<KeyValuePair<string, object>> cols, Db.intelliScraperProjectStoreInfo storeInfo)
        {
            Database.DbManager db = new Database.DbManager(storeInfo.DatabaseSetting.connection, storeInfo.DatabaseSetting.providerName);
            List<string> attrsToCheck = new List<string>(save.checkExistAttributes.Split(save.checkExistAttributesSeparator.ToCharArray()));
               
            if (save.checkExistBeforeSave)
            {
                           
                if (db.exist(attrsToCheck, save.tableName, cols))
                {
                    if (save.ifExist == Db.saveIfExist.stopAll)
                        Environment.Exit(0);
                    if (save.ifExist == Db.saveIfExist.delete)
                    {
                        db.delete(attrsToCheck, save.tableName, cols);
                    }
                    if (save.ifExist == Db.saveIfExist.update)
                    {
                        List<string> updCols = new List<string>();
                        foreach (Db.saveMap m in save.map)
                        {
                            if (m != null) 
                                updCols.Add(m.mapColName);
                        }
                        db.update(attrsToCheck,updCols, save.tableName, cols);
                    }
                    if (save.ifExist == Db.saveIfExist.insert)
                         db.insert(attrsToCheck, save.tableName, cols);
                }
                else db.insert(attrsToCheck, save.tableName, cols);
            }
            else  db.insert(attrsToCheck, save.tableName, cols);
        }

        /// <summary>
        /// Save/Update To csv
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="storeInfo"></param>
        public void csvSaveAction(List<KeyValuePair<string, object>> cols, Db.intelliScraperProjectStoreInfo storeInfo)
        {
            if (save.checkExistBeforeSave)
            {
                //find colums to check
                bool exist = rowExistInCsv(cols, storeInfo);
                if (exist)
                {
                    if (save.ifExist == Db.saveIfExist.stopAll)
                        Environment.Exit(0);
                    if (save.ifExist == Db.saveIfExist.delete)
                    {
                        System.IO.StreamWriter rw = new System.IO.StreamWriter(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        IEnumerable<string> fileRows = System.IO.File.ReadLines(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        int i = 0;
                        foreach (string row in fileRows)
                        {
                            if (i != this.csvExistRowPosition)
                                rw.WriteLine(row);
                        }
                        rw.Close();
                    }
                    if (save.ifExist == Db.saveIfExist.update)
                    {
                        System.IO.StreamWriter rw = new System.IO.StreamWriter(storeInfo.CsvExcelSetting.csvFileSaveTo);
                        IEnumerable<string> fileRows = System.IO.File.ReadLines(storeInfo.CsvExcelSetting.csvFileSaveTo);

                        int i = 0;
                        foreach (string row in fileRows)
                        {
                            if (i == this.csvExistRowPosition)
                                rw.WriteLine(this.csvCurrentRowValue);
                            else rw.WriteLine(row);
                        }
                        rw.Close();
                    }
                    if (save.ifExist == Db.saveIfExist.insert)
                    {
                        List<string> rows = new List<string>();
                        rows.Add(this.csvCurrentRowValue);
                        System.IO.File.AppendAllLines(storeInfo.CsvExcelSetting.csvFileSaveTo, rows);
                    }
                }
                else
                {
                    List<string> rows = new List<string>();
                    rows.Add(this.csvCurrentRowValue);
                    System.IO.File.AppendAllLines(storeInfo.CsvExcelSetting.csvFileSaveTo, rows);
                }

            }
            else
            {
                List<string> headers = new List<string>(storeInfo.CsvExcelSetting.csvHeader.Split(storeInfo.CsvExcelSetting.csvSeparator.ToCharArray()));
                string row = string.Empty;
                foreach (string h in headers)
                {
                    KeyValuePair<string, object> r = (from x in cols where x.Key == h select x).SingleOrDefault();
                    row += r.Value + storeInfo.CsvExcelSetting.csvSeparator;
                }

                List<string> rows = new List<string>();
                rows.Add(row);
                System.IO.File.AppendAllLines(storeInfo.CsvExcelSetting.csvFileSaveTo, rows);
            }
            
           
        }

        /// <summary>
        /// Check if row exist in csv File
        /// </summary>
        private bool rowExistInCsv(List<KeyValuePair<string, object>> cols, Db.intelliScraperProjectStoreInfo storeInfo)
        {
            bool exist = false;
            List<string> headers = new List<string>(storeInfo.CsvExcelSetting.csvHeader.Split(storeInfo.CsvExcelSetting.csvSeparator.ToCharArray()));
               

            string row = string.Empty;
            foreach (string h in headers)
            {
                KeyValuePair<string, object> r = (from x in cols where x.Key == h select x).SingleOrDefault();
                row += r.Value + storeInfo.CsvExcelSetting.csvSeparator;
            }
            this.csvCurrentRowValue = row;

            if (System.IO.File.Exists(storeInfo.CsvExcelSetting.csvFileSaveTo))
            {
                IEnumerable<string> fileRows = System.IO.File.ReadLines(storeInfo.CsvExcelSetting.csvFileSaveTo);
                int i = 0;
                foreach (string frow in fileRows)
                {
                    string currentRow = frow;
                    if (!frow.EndsWith(storeInfo.CsvExcelSetting.csvSeparator))
                        currentRow += storeInfo.CsvExcelSetting.csvSeparator;

                    this.csvExistRowPosition = i;

                    if (currentRow == row)
                    {
                        exist = true;
                        break;
                    }
                    i++;
                }
            }
            return exist;
        }
        
       /*
        private string getMapValue(Db.saveMap map,object m)
        {
            string value = string.Empty;
            if (m != null)
            {               
                if (!map.inputIsXpathCollection)
                {
                    //Direct                           
                    if (m.GetType() == typeof(string))
                        value = (string)m;

                    //by attribute
                    if (m.GetType() == typeof(List<KeyValuePair<string, object>>))
                    {
                        List<KeyValuePair<string, object>> vals = (List<KeyValuePair<string, object>>)m;
                        KeyValuePair<string, object> kval = (from x in vals where x.Key == map.attributeId select x).FirstOrDefault();
                        value = (string)kval.Value;
                    }

                    //by attribute first
                    if (m.GetType() == typeof(List<List<KeyValuePair<string, object>>>))
                    {
                        List<List<KeyValuePair<string, object>>> vals = (List<List<KeyValuePair<string, object>>>)m;
                        foreach (List<KeyValuePair<string, object>> k in vals)
                        {
                            KeyValuePair<string, object> kval = (from x in k where x.Key == map.attributeId select x).FirstOrDefault();
                            value = (string)kval.Value;
                            break;
                        }
                    }
                }
                else
                {
                    //find by result position                                
                    List<List<KeyValuePair<string, object>>> mvals = (List<List<KeyValuePair<string, object>>>)m;
                    List<KeyValuePair<string, object>> valsx = (List<KeyValuePair<string, object>>)mvals[map.xpathCollectionPosition];
                    KeyValuePair<string, object> kval = (from x in valsx where x.Key == map.attributeId select x).FirstOrDefault();
                    value = (string)kval.Value;
                }
            }
            return value;
        }
        */
    }
}
