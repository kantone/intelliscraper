using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using IntelliScraper.Scrape;
using System.Xml.Linq;
using IntelliScraper;
using System.Xml.XPath;
using HtmlAgilityPack;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
/*
namespace IntelliScraper
{
    /// <summary>
    /// Scraper - run full logic
    /// </summary>
    public class Scraper
    {
        private bool getHtmlAsync = false;
        private DateTime start;
        private DateTime end;

        /// <summary>
        /// Run intelliscraping wrapper
        /// </summary>
        public void RunScraping(IntelliScraper.Data.intelliScraper i)
        {
            start = DateTime.Now;
            if (i.settings.usePageLoop)
                runScrapingByPageLoop(i);
            else runScrapingDefault(i,i.Action,false,string.Empty);
            end = DateTime.Now;

         
            TimeSpan span = end - start;
            Factory.Instance.iInfo(string.Format("Elapsed time : {0} days, {1} hours, {2} minutes, {3} seconds, {4} milliseconds",span.TotalDays,span.TotalHours,span.TotalMinutes,span.TotalSeconds,span.TotalMilliseconds));
        }

        /// <summary>
        /// Loop pages and scraping
        /// </summary>
        private void runScrapingByPageLoop(IntelliScraper.Data.intelliScraper i)
        {
            Factory.Instance.log.Info("Starting scraping by pages loop...");

            //set start end page loop
            int start = 0;
            int end = 0;

            //custom start end
            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromStart)
            {               
                start =  i.settings.pageLoopInfo.start;
                end = i.settings.pageLoopInfo.end;
                Factory.Instance.iInfo("fromStart");
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} - Custom", start, end));
            }

            //find end by rule
            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromZero_to_byRule)
            {
                Data.intelliScraperAction refRule = findRuleById(i.Action, i.settings.pageLoopInfo.ruleId_getEndPage);
                string _end = (string)xpathRunSingle(refRule.Rule.xpath.xpathRule[0], i.Action, refRule.Rule.xpath.xpathRule, true, false, null,false,string.Empty);
                end = Int32.Parse(_end);

                start =  i.settings.pageLoopInfo.start;
                end = i.settings.pageLoopInfo.end;
                Factory.Instance.iInfo(string.Format("fromZero_to_byRule : Checked end page by rule {0}", refRule.Rule.id));
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} - Start from zero & end page fetched", 0, end));
                
            }

            if (i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromCustom_to_byRule)
            {
                start = i.settings.pageLoopInfo.start;
                Data.intelliScraperAction refRule = findRuleById(i.Action, i.settings.pageLoopInfo.ruleId_getEndPage);
                string _end = (string)xpathRunSingle(refRule.Rule.xpath.xpathRule[0], i.Action, refRule.Rule.xpath.xpathRule, true, false, null, false, string.Empty);
                end = Int32.Parse(_end);
                Factory.Instance.iInfo(string.Format("fromCustom_to_byRule : Checked end page by rule {0}", refRule.Rule.id));
                Factory.Instance.log.Info(string.Format("Set page start={0} & end={1} -  Start from Custom & end page fetched", 0, end));
            }

            //Run
            if (i.settings.pageLoopInfo.MultiThreadOption.enableMultithread)
            {
                this.getHtmlAsync = true;

                DateTime dt1 = DateTime.Now;
                //Now i have start and end page
                Factory.Instance.log.Info(string.Format("Looping  {0} pages Multhithreaded", (end - start).ToString()));
                ParallelOptions options = new ParallelOptions();
                if (i.settings.pageLoopInfo.MultiThreadOption.setThreadMaxNumbers)
                    options.MaxDegreeOfParallelism = i.settings.pageLoopInfo.MultiThreadOption.ThreadNumbers;

                
                System.Threading.Tasks.Parallel.For(start, end,options,  ii =>
                {
                    runActionsPageLoop(i, ii);
                });

               
                object sync = new Object();              
                var waitHandle = new ManualResetEvent(false);
                int counter = 0;
                System.Threading.Tasks.Parallel.For(start, end,options,  ii =>
                {
                   lock (sync)
                   {
                       runActionsPageLoop(i, ii);
                       if (Interlocked.Increment(ref counter) == end - 1)
                       {
                           waitHandle.Set();
                       }
                       waitHandle.WaitOne();
                   }                   

                });
               

               
            }
            else
            {
                DateTime dt1 = DateTime.Now;
                //Now i have start and end page
                Factory.Instance.log.Info(string.Format("Looping {0} pages", (end - start).ToString()));

                for (int ii = start; ii < end; ii++)
                {
                    runActionsPageLoop(i, ii);
                }
                DateTime dt2 = DateTime.Now;
                TimeSpan span = dt2 - dt1;
                int ms = (int)span.TotalMilliseconds;
            }
           
        }

        /// <summary>
        /// Performa Action for specific Page
        /// </summary>
        private void runActionsPageLoop(IntelliScraper.Data.intelliScraper i,int ii)
        {
            string pageNumber = Factory.Instance.i.settings.pageLoopInfo.appendBeforePageNumber + ii + Factory.Instance.i.settings.pageLoopInfo.appendAfterPageNumber;
            string url = string.Format(Factory.Instance.i.settings.pageLoopInfo.urlFormat, pageNumber);
            Factory.Instance.log.Info(string.Format("Get Html from {0}", url));
            string html = Scrape.ScraperAction.getHtml(url, i.settings.pageLoopInfo.customHttpHeader,this.getHtmlAsync);
            i.settings.pageLoopInfo.fetchHtml = html;
            Data.intelliScraperActionCollection actions = new Data.intelliScraperActionCollection();
            foreach (string ruleId in Factory.Instance.i.settings.pageLoopInfo.ruleId_executePage)
            {
                Data.intelliScraperAction refRule = findRuleById(i.Action, ruleId);
                Factory.Instance.iInfo(string.Format("{0} - Determine rule to execute - id = {1}", ruleId, refRule.Rule.id));
                if (refRule != null)
                    actions.Add(refRule);
            }
            Factory.Instance.iInfo(string.Format("Execute {0} actions rule for page {1}", actions.Count.ToString(), url));
            runScrapingDefault(i, actions, true, html);
        }

        /// <summary>
        /// Scrape data logic Wrapper
        /// </summary>
        private void runScrapingDefault(IntelliScraper.Data.intelliScraper i,Data.intelliScraperActionCollection actions,bool customHtml,string customHtmlData)
        {          
            foreach (Data.intelliScraperAction r in actions)
            {
                runScrapingSingle(i, r, customHtml, customHtmlData);     
            }
        }

        /// <summary>
        /// Scrape data logic
        /// </summary>
        private void runScrapingSingle(IntelliScraper.Data.intelliScraper i,Data.intelliScraperAction r, bool customHtml, string customHtmlData)
        {
            //Case HTML
            ruleGetHtml(i, r, customHtml, customHtmlData);          

            //Case XPATH
            ruleXPATH(i, r, customHtml, customHtmlData);

            //Case DOWNLOAD
            ruleDOWNLOAD(i, r, customHtml, customHtmlData);

            //Case Save
            ruleSAVE(i, r, customHtml, customHtmlData);
          
        }

         /// <summary>
        /// ruleSAVE
        /// </summary>
        private void ruleDOWNLOAD(IntelliScraper.Data.intelliScraper i, Data.intelliScraperAction r, bool customHtml, string customHtmlData)
        {
            if (r.Rule.download)
            {
                if (!System.IO.Directory.Exists(r.Rule.DownloadAction.DirSaveToFileInfo.startFolder))
                {
                    Factory.Instance.iInfo(string.Format("Create start download directory : {0}",r.Rule.DownloadAction.DirSaveToFileInfo.startFolder));
                    System.IO.Directory.CreateDirectory(r.Rule.DownloadAction.DirSaveToFileInfo.startFolder);
                }

                //Create right folder
                string folder = r.Rule.DownloadAction.DirSaveToFileInfo.startFolder;
                if(r.Rule.DownloadAction.DirSaveToFileInfo.generateSubFolder)
                {
                   if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.custom)
                   {
                       folder= folder + "\\" + r.Rule.DownloadAction.DirSaveToFileInfo.SubFolderCustomName;
                       if (!System.IO.Directory.Exists(folder))
                       {
                            System.IO.Directory.CreateDirectory(folder);
                            Factory.Instance.iInfo(string.Format("Download - Created folder : {0}",folder));
                       }
                   }
                   else
                   {
                       DateTime d = DateTime.Now;
                       if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.dd)
                           folder = folder + "\\" + d.Day.ToString();

                        if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.mm)
                           folder = folder + "\\" + d.Month.ToString();

                        if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.mm_dd)
                           folder = folder + "\\" + d.Month.ToString() + "_" + d.Day.ToString();

                        if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.yy)
                           folder = folder + "\\" + d.Year.ToString();

                        if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.yy_mm)
                           folder = folder + "\\" +  d.Year.ToString() + "_" + d.Month.ToString();

                       if( r.Rule.DownloadAction.DirSaveToFileInfo.subfolderNameType == Data.RuleEntityDownloadActionDirSaveToFileInfoSubfolderNameType.yy_mm_dd)
                           folder = folder + "\\" +  d.Year.ToString() + "_" + d.Month.ToString() + "_" + d.Day.ToString();
                                                                  
                       if (!System.IO.Directory.Exists(folder))
                       {
                           System.IO.Directory.CreateDirectory(folder);
                           Factory.Instance.iInfo(string.Format("Download - Created folder : {0}",folder));
                       }
                   }
                }

                //get image name
                string[] ids = r.Rule.DownloadAction.inputRuleIdxPathId.Split('.');
                //Data.intelliScraperAction refRule = findRuleValueByIdAndXpathId(i.Action, ids[0], ids[1]);
                //List<KeyValuePair<string,object>> dt = (List<KeyValuePair<string,object>>)refRule.Rule.fetchedData;
                string imgUrl = (string)findRuleValueByIdAndXpathId(i.Action, ids[0], ids[1]); ;
                int index =imgUrl.LastIndexOf("/");
                string imgNameWithExt = string.Empty;
                string ext = string.Empty;
                if(index > 0)
                    imgNameWithExt = imgUrl.Substring(index+1);                
                index = imgNameWithExt.LastIndexOf(".");
                 if(index > 0)
                    ext = imgNameWithExt.Substring(index+1);
                string imgName = imgNameWithExt.Replace(ext,"");                
                string imgNameFullPath = folder + "\\" + imgNameWithExt;
                string timeStamp = Utils.GetTimestamp(DateTime.Now);
                if (r.Rule.DownloadAction.autoRenameImage)
                {
                    imgNameFullPath = folder + "\\" + timeStamp + "." + ext;
                }


                //Dowload image                
                bool downloaded = ScraperAction.downloadImage(imgUrl,imgNameFullPath);
                Factory.Instance.log.Info(string.Format("Downloaded file {0} to {1}", imgUrl, imgNameFullPath));
                if(downloaded){

                    bool deleteOriginal = false;
                    //chage image size
                    if (r.Rule.DownloadAction.ImgConvertAction.changeSize)
                    {
                       System.Drawing.Image img =  System.Drawing.Image.FromFile(imgNameFullPath);
                       if (img != null)
                       {
                           deleteOriginal = true;
                           System.Drawing.Image img2 = IntelliScraper.Utils.ResizeImage(img, new System.Drawing.Size(r.Rule.DownloadAction.ImgConvertAction.toSizew, r.Rule.DownloadAction.ImgConvertAction.toSizeH), true);
                           img.Dispose();
                           img2.Save(imgNameFullPath);
                           Factory.Instance.log.Info(string.Format("Image {0} resized to {1}x{2}", imgNameFullPath, r.Rule.DownloadAction.ImgConvertAction.toSizew, r.Rule.DownloadAction.ImgConvertAction.toSizeH));
                       }
                       else
                       {
                           Factory.Instance.log.Error(string.Format("Error resizing image {0}",imgNameFullPath));
                       }
                    }

                    //Convert to format
                    string newImgName = imgNameFullPath;
                    if (r.Rule.DownloadAction.ImgConvertAction.convertToFormat)
                    {
                        deleteOriginal = true;
                        System.Drawing.Image img = System.Drawing.Image.FromFile(imgNameFullPath);
                        if (r.Rule.DownloadAction.ImgConvertAction.convertImageType == Data.ConvertActionEntityConvertImageType.bmp)
                        {                          
                            string replaceImage = imgNameFullPath.Replace(ext,"bmp");                           
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                        }
                        if (r.Rule.DownloadAction.ImgConvertAction.convertImageType == Data.ConvertActionEntityConvertImageType.ico)
                        {                          
                            string replaceImage = imgNameFullPath.Replace(ext,"ico");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                        }
                        if (r.Rule.DownloadAction.ImgConvertAction.convertImageType == Data.ConvertActionEntityConvertImageType.jpg)
                        {                          
                            string replaceImage = imgNameFullPath.Replace(ext,"jpg");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                        }
                        if (r.Rule.DownloadAction.ImgConvertAction.convertImageType == Data.ConvertActionEntityConvertImageType.png)
                        {                          
                            string replaceImage = imgNameFullPath.Replace(ext,"png");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                       }
                       if (r.Rule.DownloadAction.ImgConvertAction.convertImageType == Data.ConvertActionEntityConvertImageType.tiff)
                        {                          
                            string replaceImage = imgNameFullPath.Replace(ext,"tiff");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                        }
                         Factory.Instance.iInfo(string.Format("Image {0} converted to {1}", imgNameFullPath, r.Rule.DownloadAction.ImgConvertAction.convertImageType.ToString()));
                         img.Dispose();             
                    }

                    //Create Thumbs
                    if (r.Rule.DownloadAction.ImgConvertAction.createThumbs)
                    {
                        //create thumb folder
                        string thumbFolder = folder + "\\thumbs";
                        if (r.Rule.DownloadAction.ImgConvertAction.createThumbFolder)
                        {
                            if (!System.IO.Directory.Exists(thumbFolder))
                            {
                                System.IO.Directory.CreateDirectory(thumbFolder);
                                Factory.Instance.iInfo(string.Format("Download - Created folder : {0}", thumbFolder));
                            }
                        }

                        //generate thumbs
                        System.Drawing.Image img = System.Drawing.Image.FromFile(newImgName);
                        foreach (Data.ConvertActionEntityThumbs thumb in r.Rule.DownloadAction.ImgConvertAction.Thumbs)
                        {
                            string thumbName = thumbFolder + "\\" + thumb.name + "." + ext;
                             if (r.Rule.DownloadAction.autoRenameImage)
                                 thumbName = thumbFolder + "\\" + thumb.name + "_" + timeStamp + "." + ext;
                            System.Drawing.Image t = IntelliScraper.Utils.ResizeImage(img, new System.Drawing.Size(thumb.toSizeW, thumb.toSizeH), true);
                            t.Save(thumbName);
                            Factory.Instance.iInfo(string.Format("Generate thumb {0} - {1}x{2}", thumbName, thumb.toSizeW, thumb.toSizeH));
                        }
                        img.Dispose();
                    }

                    if (deleteOriginal)
                    {
                       
                        System.IO.File.Delete(imgNameFullPath);
                    }
                    
                }
                else
                {
                    Factory.Instance.log.Error(string.Format("Error downloading image {0} for rule {1}",imgUrl,r.Rule.id));
                }
            }
        }

        /// <summary>
        /// ruleSAVE
        /// </summary>
        private void ruleSAVE(IntelliScraper.Data.intelliScraper i, Data.intelliScraperAction r, bool customHtml, string customHtmlData)
        {
            //Now Save Fetched Data
            if (r.Save != null)
            {
                if (r.Save.enableSave == true)
                {
                    //Save to csv File
                    if (Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.csv)
                    {
                        Factory.Instance.iInfo(string.Format("Start saving data into csv file {0}", Factory.Instance.i.storeInfo.csvFileSaveTo));
                        string[] cols = Factory.Instance.i.storeInfo.csvHeader.Split(',');
                        Factory.Instance.iInfo(string.Format("Csv have {0} columns", cols.Length.ToString()));

                        //get fetched data
                        List<KeyValuePair<string, object>> dt = (List<KeyValuePair<string, object>>)r.Rule.fetchedData;
                        List<string[]> rows = ScraperAction.mapCsvToRows(r.Save.mapping, r.Rule.xpath.xpathRule, cols, r.Rule.id, dt);

                        Factory.Instance.iInfo(string.Format("Saving {0} rows", rows.Count.ToString()));
                        foreach (string[] rowData in rows)
                        {
                            Factory.Instance.appendCsvLine(rowData);
                        }
                    }


                    //Save to Database
                    if (Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.MySql || Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.Oracle || Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.PostgreSQL || Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.sqlServer || Factory.Instance.i.storeInfo.type == Data.intelliScraperStoreInfoType.sqlServerCE)
                    {
                        Factory.Instance.iInfo(string.Format("Saving to database"));
                        DatabaseSaveAction(r);
                    }
                }
            }
        }

        /// <summary>
        /// ruleXPATH
        /// </summary>
        private void ruleXPATH(IntelliScraper.Data.intelliScraper i, Data.intelliScraperAction r, bool customHtml, string customHtmlData)
        {
            //Get xpath 
            if (r.Rule.type == Data.RuleEntityType.xpath)
            {
                List<KeyValuePair<string, object>> xpathData = new List<KeyValuePair<string, object>>();

                //add single
                if (r.Rule.xpath.type == Data.RuleEntityXpathType.single)
                {
                    Factory.Instance.iInfo(string.Format("Rule Action : get xpath single for rule id {0}", r.Rule.xpath.xpathRule[0].id));
                    object o = xpathRunSingle(r.Rule.xpath.xpathRule[0], i.Action, r.Rule.xpath.xpathRule, true, false, null, customHtml, customHtmlData);
                    string id = string.Format("{0}.{1}", r.Rule.id, r.Rule.xpath.xpathRule[0].id);
                    KeyValuePair<string, object> ks = new KeyValuePair<string, object>(id, o);
                    xpathData.Add(ks);
                    r.Rule.fetchedData = xpathData;
                    r.Rule.xpath.xpathRule[0].fetchedData = o;
                }

                //add multiple saved as KeyValuePair where id = RuleId.XpathActionId
                if (r.Rule.xpath.type == Data.RuleEntityXpathType.collection)
                {
                    Factory.Instance.iInfo(string.Format("Rule Action : get xpath collection - rule id {0}", r.Rule.id));
                    Factory.Instance.iInfo(string.Format("Total xpath actions : {0}", r.Rule.xpath.xpathRule.Count.ToString()));
                    foreach (Data.xpathEntity a in r.Rule.xpath.xpathRule)
                    {
                        //get previus Data
                        object previusObject = new object();
                        if (a.FindType == Data.xpathEntityFindType.inputData)
                        {
                            Factory.Instance.iInfo(string.Format("Find previus object in rule : id {0}", a.inputRuleId));
                            Data.intelliScraperAction refRule = findRuleById(i.Action, a.inputRuleId);
                            previusObject = refRule.Rule.fetchedData;
                        }

                        //Case simple Node
                        if (a.inputCollectionType == Data.xpathEntityInputCollectionType.singleNode)
                        {
                            Factory.Instance.iInfo(string.Format("Return single Node : id {0}", a.inputRuleId));
                            HtmlNode node = (HtmlNode)previusObject;
                            object o = xpathRunSingle(a, i.Action, r.Rule.xpath.xpathRule, false, true, node, customHtml, customHtmlData);
                            string id = string.Format("{0}.{1}", r.Rule.id, a.id);
                            KeyValuePair<string, object> k = new KeyValuePair<string, object>(id, o);
                            xpathData.Add(k);
                        }

                        //Case node Collection
                        if (a.inputCollectionType == Data.xpathEntityInputCollectionType.nodeCollection)
                        {
                            List<KeyValuePair<string, object>> ks = (List<KeyValuePair<string, object>>)previusObject;
                            HtmlNodeCollection nodes = (HtmlNodeCollection)ks[0].Value;
                            Factory.Instance.iInfo(string.Format("Loop trought {0} node collection : id {1}", nodes.Count.ToString(), a.inputRuleId));
                            foreach (HtmlNode node in nodes)
                            {
                                object o = xpathRunSingle(a, i.Action, r.Rule.xpath.xpathRule, false, true, node, customHtml, customHtmlData);
                                string id = string.Format("{0}.{1}", r.Rule.id, a.id);
                                KeyValuePair<string, object> k = new KeyValuePair<string, object>(id, o);
                                xpathData.Add(k);
                            }
                        }



                    }

                    //Ordering
                    //Example 2 xpath rules
                    //xpathData was in format : 1,2,3,4
                    //I need : 1,3 and 2,4
                    Factory.Instance.iInfo(string.Format("Reorder fetched data for saving purpose"));
                    List<KeyValuePair<string, object>> xpathDataOrder = new List<KeyValuePair<string, object>>();
                    int tot = r.Rule.xpath.xpathRule.Count;
                    int t2 = (xpathData.Count / tot);
                    for (int ii = 0; ii < t2; ii++)
                    {
                        for (int s = 0; s < tot; s++)
                        {
                            int index = (t2 * s) + ii;
                            string id = xpathData[index].Key;
                            object o = xpathData[index].Value;
                            KeyValuePair<string, object> k2 = new KeyValuePair<string, object>(id, o);
                            xpathDataOrder.Add(k2);
                        }

                    }

                    r.Rule.fetchedData = xpathDataOrder;
                }
            }
        }

        /// <summary>
        /// ruleGetHtml
        /// </summary>
        private void ruleGetHtml(IntelliScraper.Data.intelliScraper i, Data.intelliScraperAction r, bool customHtml, string customHtmlData)
        {
            //Get html
            if (r.Rule.type == Data.RuleEntityType.httpGet)
            {
                Factory.Instance.iInfo(string.Format("Rule Action : get html from {0}", r.Rule.url));
                string html = Scrape.ScraperAction.getHtml(r.Rule.url, r.Rule.customHttpHeader, this.getHtmlAsync);
                r.Rule.fetchedData = html;
            }

            //Get html from file
            if (r.Rule.type == Data.RuleEntityType.loadHtmlFromfile)
            {
                Factory.Instance.iInfo(string.Format("Rule Action : get html from file {0}", r.Rule.htmlFileName));
                if (System.IO.File.Exists(r.Rule.htmlFileName))
                {
                    string html = System.IO.File.ReadAllText(r.Rule.htmlFileName);
                    r.Rule.fetchedData = html;
                }
                else throw new FileNotFoundException(r.Rule.htmlFileName);
            }

        }

        /// <summary>
        /// Save data to database
        /// </summary>
        private void DatabaseSaveAction(Data.intelliScraperAction r)
        {
            try
            {
                //get fetched data
                List<KeyValuePair<string, object>> dt = (List<KeyValuePair<string, object>>)r.Rule.fetchedData;

                //Db Manager
                Database.DbManager m = new Database.DbManager(Factory.Instance.i.storeInfo.connection, Factory.Instance.i.storeInfo.providerName);

                //Mapping Rows
                string[] cols = new string[r.Save.mapping.Count];
                for (int x = 0; x < r.Save.mapping.Count; x++)
                    cols[x] = r.Save.mapping[x].mapColName;
                List<List<KeyValuePair<string, object>>> mappedRows = ScraperAction.mapTable(r.Save.mapping, r.Rule.xpath.xpathRule, cols, r.Rule.id, dt);

                //Load Attributes
                List<string> attributes = new List<string>(r.Save.checkExistAttributes.Split(','));
                List<string> insertAttrs = new List<string>();
                foreach (Data.SaveEntityMap map in r.Save.mapping)
                    insertAttrs.Add(map.mapColName);

                //Foreach Data
                foreach (List<KeyValuePair<string, object>> mpdr in mappedRows)
                {
                    //Check Exist
                    if (r.Save.checkExist)
                    {
                        if (m.exist(attributes, r.Save.tableName, mpdr))
                        {
                            Factory.Instance.iInfo(string.Format("Record exist {0}", String.Join(",", attributes)));

                            //exit
                            if (r.Save.ifExist == Data.SaveEntityIfExist.stopAll)
                            {
                                Factory.Instance.iInfo(string.Format("STOP ALL - rule {0}", r.Rule.id));
                                Environment.Exit(0);
                            }

                            if (r.Save.ifExist == Data.SaveEntityIfExist.update)
                            {
                                Factory.Instance.iInfo(string.Format("Update record {0}", String.Join(",", insertAttrs)));
                                m.update(attributes, insertAttrs, r.Save.tableName, mpdr);
                            }

                            if (r.Save.ifExist == Data.SaveEntityIfExist.delete)
                            {
                                Factory.Instance.iInfo(string.Format("Delete Record {0}", String.Join(",", attributes)));
                                m.delete(attributes, r.Save.tableName, mpdr);
                            }
                        }
                        else
                        {
                            Factory.Instance.iInfo(string.Format("Insert record  {0}", String.Join(",", insertAttrs)));
                            m.insert(insertAttrs, r.Save.tableName, mpdr);
                        }
                    }
                    else
                    {
                        Factory.Instance.iInfo(string.Format("Insert record  {0}", String.Join(",", insertAttrs)));
                        m.insert(insertAttrs, r.Save.tableName, mpdr);
                    }
                }
            }
            catch (Exception ex)
            {
                Factory.Instance.log.Error(ex);
                if (r.Save.onError == Data.SaveEntityOnError.stopAll)
                    Environment.Exit(8);
            }

        }

        /// <summary>
        /// Perform xpath action by definitions
        /// </summary>
        private object xpathRunSingle(Data.xpathEntity r, Data.intelliScraperActionCollection rules, Data.xpathEntityCollection xpathRules, bool loadhtml, bool useCustomNode, HtmlNode customNode,bool customHtml,string customHtmlData)
        {

            object res = new object();
            string html = string.Empty;
            HtmlDocument doc = new HtmlDocument();

            //Load html page
            if (loadhtml && !customHtml)
            {
              
                //Load html By page Url Searcher
                if (Data.xpathEntityFindType.pageLoop != null)
                {
                    if (r.FindType == Data.xpathEntityFindType.pageLoop)
                    {
                        if (Factory.Instance.i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromCustom_to_byRule ||
                            Factory.Instance.i.settings.pageLoopInfo.pageLoopType == Data.intelliScraperSettingsPageLoopInfoPageLoopType.fromZero_to_byRule)
                        {
                            Factory.Instance.iInfo(string.Format("Loading html by PageLoop - page {0}", Factory.Instance.i.settings.pageLoopInfo.urlSearcher));
                            html = Scrape.ScraperAction.getHtml(Factory.Instance.i.settings.pageLoopInfo.urlSearcher, Factory.Instance.i.settings.pageLoopInfo.customHttpHeader, this.getHtmlAsync);
                        }
                        else
                        {
                            Factory.Instance.iInfo(string.Format("Loading html by PageLoop - page {0}", Factory.Instance.i.settings.pageLoopInfo.fetchHtml));
                            html = Factory.Instance.i.settings.pageLoopInfo.fetchHtml;
                        }
                    }
                }

                //Get from Rule
                if (r.FindType == Data.xpathEntityFindType.inputData && !string.IsNullOrEmpty(r.inputRuleId))
                {
                    Data.intelliScraperAction refRule = findRuleById(rules, r.inputRuleId);
                    Factory.Instance.iInfo(string.Format("Loading html by rule reference fetched html - rule id {0}", r.inputRuleId)); 
                    if (refRule != null)
                        html = (string)refRule.Rule.fetchedData;
                }

                //get Html Web Directly
                if (r.FindType == Data.xpathEntityFindType.getUrl)
                {
                    Factory.Instance.iInfo(string.Format("Load rule html : {0}", r.url));
                    html = Scrape.ScraperAction.getHtml(r.url, null,this.getHtmlAsync);
                }

                doc.LoadHtml(html);

            }
            if (customHtml)
            {
                Factory.Instance.iInfo(string.Format("Load custom - html previusly fetched"));
                doc.LoadHtml(customHtmlData);
            }
            
            //Make xPath                    

            
            HtmlNode node = null;
            if (useCustomNode)
            {
                node = customNode.SelectSingleNode(r.xpath);
            }
            else node = doc.DocumentNode.SelectSingleNode(r.xpath);

            if (r.getType == Data.xpathEntityGetType.text)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Text"));
                string txt = node.InnerText;
                res = multipletxtOperations(txt, r.postProcessData);
                Factory.Instance.log.Info(string.Format("Found value - {0}",res));
            }
            if (r.getType == Data.xpathEntityGetType.html)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Html"));
                string txt = node.InnerHtml;
                res = multipletxtOperations(txt, r.postProcessData);
                Factory.Instance.log.Info(string.Format("Found value - {0}",res));
            }

            if (r.getType == Data.xpathEntityGetType.attribute)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Attribute Value {0}",r.attributeName));
                if (node.Attributes[r.attributeName] != null)
                {
                    string txt = node.Attributes[r.attributeName].Value;
                    res = multipletxtOperations(txt, r.postProcessData);
                    Factory.Instance.log.Info(string.Format("Found value - {0}",res));
                }
            }


            if (r.getType == Data.xpathEntityGetType.singleNode)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Node"));
                res = doc.DocumentNode.SelectSingleNode(r.xpath);
            }

            if (r.getType == Data.xpathEntityGetType.nodeCollection)
            {
                Factory.Instance.iInfo(string.Format("XPATH get Node Collection"));
                res = doc.DocumentNode.SelectNodes(r.xpath);
            }


            return res;
        }

        /// <summary>
        /// Make text operation like substring,trim,replace and more
        /// </summary>
        private string multipletxtOperations(string txt, IntelliScraper.Data.ResultActionEntityCollection resultActions)
        {
            foreach (IntelliScraper.Data.ResultActionEntity a in resultActions)
                txt = txtOperations(txt, a);
            return txt;
        }

        /// <summary>
        /// Make text operation like substring,trim,replace and more
        /// </summary>
        private string txtOperations(string txt, IntelliScraper.Data.ResultActionEntity resultAction)
        {
            txt = txt.Replace("\n", "");

            //trim
            if (resultAction.trim)
                txt = txt.Trim();

            //Encode
            if (resultAction.HtmlDecode)
                txt = System.Web.HttpUtility.HtmlDecode(txt);

            //Substring
            if (resultAction.makeSubstring)
            {
                Factory.Instance.log.Info("Make Substring of fetched data");

                if(resultAction.substringType == Data.ResultActionEntitySubstringType.simple && resultAction.substringFrom >0)
                {
                    Factory.Instance.iInfo(string.Format("Substring from {0}", resultAction.substringFrom));
                    txt = txt.Substring(resultAction.substringFrom);
                }
                if (resultAction.substringType == Data.ResultActionEntitySubstringType.fromTo && resultAction.substringFrom > 0 &&  resultAction.substringTo >0)
                {
                    Factory.Instance.iInfo(string.Format("Substring from {0} to {1}", resultAction.substringFrom, resultAction.substringTo));
                    txt = txt.Substring(resultAction.substringFrom, resultAction.substringTo);                    
                }

                if (resultAction.substringType == Data.ResultActionEntitySubstringType.search && !string.IsNullOrEmpty(resultAction.substringSearch))
                {
                    Factory.Instance.iInfo(string.Format("Substring from - by string search - {0}", resultAction.substringSearch));                   
                    int index = txt.IndexOf(resultAction.substringSearch);
                    if (index > 0)
                    {
                        txt = txt.Substring(index+1);
                    }
                }

                if (resultAction.substringType == Data.ResultActionEntitySubstringType.searchTo && !string.IsNullOrEmpty(resultAction.substringSearch))
                {
                    Factory.Instance.iInfo(string.Format("Substring from/to - by string search - {0}", resultAction.substringSearch)); 
                    int index = txt.IndexOf(resultAction.substringSearch);
                    if (index > 0)
                    {
                        txt = txt.Substring(0,index);
                    }
                }

            }

            //Replace text
            if (resultAction.replace)
            {
                Factory.Instance.iInfo(string.Format("Replace text {0} with {1}", resultAction.findText, resultAction.replaceText));                   
                if (!string.IsNullOrEmpty(resultAction.findText) && !string.IsNullOrEmpty(resultAction.replaceText))
                {
                    txt = txt.Replace(resultAction.findText, resultAction.replaceText);
                }
            }

            //if Start With
            if (resultAction.startWithEnabled && !string.IsNullOrEmpty(resultAction.startWithVal)  && txt.StartsWith(resultAction.startWithVal))
            {
                Factory.Instance.iInfo(string.Format("If start with append before e/o after"));                                   
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.before)                 
                    txt = resultAction.startWithAddVal + txt;
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.after)
                    txt = txt + resultAction.startWithAddVal;
                if (resultAction.startWithAttachType.attachType == Data.attachTypeEntityAttachType.both)
                    txt = resultAction.startWithAddVal + txt + resultAction.startWithAddVal;
            }

            //if End With
            if (resultAction.endWithEnabled && !string.IsNullOrEmpty(resultAction.endWithVal) && txt.StartsWith(resultAction.endWithVal))
            {
                Factory.Instance.iInfo(string.Format("If end with append before e/o after"));   
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.before)
                    txt = resultAction.endWithAddVal + txt;
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.after)
                    txt = txt + resultAction.endWithAddVal;
                if (resultAction.endWithAttachType.attachType == Data.attachTypeEntityAttachType.both)
                    txt = resultAction.endWithAddVal + txt + resultAction.endWithAddVal;
            }

            if (resultAction.useRegularExpression){
                 Match match = Regex.Match(txt, resultAction.regularExpression);
                 txt = match.Value;
            }

            //trim
            if (resultAction.trim)
            {
                Factory.Instance.log.Info("Trim value");
                txt = txt.Trim();
            }

            return txt;
        }

        /// <summary>
        /// Find previusly fetched data by id
        /// </summary>
        private Data.intelliScraperAction findRuleById(Data.intelliScraperActionCollection rules, string inputId)
        {
            var values = from x in rules where x.Rule.id == inputId select x;
            foreach (var val in values)
                return val;
            return null;
        }

        /// <summary>
        /// Find previusly fetched data by id
        /// </summary>
        private object findRuleValueByIdAndXpathId(Data.intelliScraperActionCollection rules, string ruleId,string xpathId)
        {
            var values = from x in rules where x.Rule.id == ruleId select x;
            foreach (var val in values)
            {
                Data.intelliScraperAction e = (Data.intelliScraperAction)val;
                var val2 = from y in e.Rule.xpath.xpathRule where y.id == xpathId select y;
                foreach (var v in val2)
                    return v.fetchedData;
            }
            return null;
        }
        
    }


    
}*/
