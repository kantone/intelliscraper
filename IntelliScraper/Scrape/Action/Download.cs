using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IntelliScraper.Scrape.Action
{
    public class Download 
    {       
        
        Db.download d { get; set; }
        public Download(Db.download d)
        {
            this.d = d;
        }

        /// <summary>
        /// Execute Download Action
        /// </summary>
        /// <param name="files">files to download</param>
        /// <returns>lis of files downloaded</returns>
        public List<string> Run(List<string> files)
        {
            List<string> downloadedFiles = new List<string>();
            bool runNonMultitrhead = true;
            if (d.MultiThreadOption != null)
            {
                if (d.MultiThreadOption.enableMultithread)
                {
                    runNonMultitrhead = false;
                    ParallelOptions options = new ParallelOptions();
                    if (d.MultiThreadOption.setThreadMaxNumbers)
                        options.MaxDegreeOfParallelism = d.MultiThreadOption.ThreadNumbers;

                    //Run links multithread
                    System.Threading.Tasks.Parallel.For(0, files.Count, options, ii =>
                    {
                        downloadedFiles.Add(downloadFile(files[ii]));
                    });
                }
                
            }
            if (runNonMultitrhead)
            {
                foreach (string file in files)
                    downloadedFiles.Add(downloadFile(file));                
            }
          

            return downloadedFiles;
        }


        /// <summary>
        /// Create Folders - download file - convert image e create thumbs
        /// </summary>
        /// <param name="fname"></param>
        private string downloadFile(string fname)
        {
            if (!System.IO.Directory.Exists(d.DirSaveToFileInfo.startFolder))
            {
                Factory.Instance.iInfo(string.Format("Create start download directory : {0}", d.DirSaveToFileInfo.startFolder));
                System.IO.Directory.CreateDirectory(d.DirSaveToFileInfo.startFolder);
            }

            //Create right folder
            string folder = d.DirSaveToFileInfo.startFolder;
            if (d.DirSaveToFileInfo.generateSubFolder)
            {
                if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.custom)
                {
                    folder = folder + "\\" + d.DirSaveToFileInfo.SubFolderCustomName;
                    if (!System.IO.Directory.Exists(folder))
                    {
                        System.IO.Directory.CreateDirectory(folder);
                        Factory.Instance.iInfo(string.Format("Download - Created folder : {0}", folder));
                    }
                }
                else
                {
                    DateTime dd = DateTime.Now;
                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.dd)
                        folder = folder + "\\" + dd.Day.ToString();

                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.mm)
                        folder = folder + "\\" + dd.Month.ToString();

                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.mm_dd)
                        folder = folder + "\\" + dd.Month.ToString() + "_" + dd.Day.ToString();

                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.yy)
                        folder = folder + "\\" + dd.Year.ToString();

                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.yy_mm)
                        folder = folder + "\\" + dd.Year.ToString() + "_" + dd.Month.ToString();

                    if (d.DirSaveToFileInfo.subfolderNameType == Db.downloadDirSaveToFileInfoSubfolderNameType.yy_mm_dd)
                        folder = folder + "\\" + dd.Year.ToString() + "_" + dd.Month.ToString() + "_" + dd.Day.ToString();

                    if (!System.IO.Directory.Exists(folder))
                    {
                        System.IO.Directory.CreateDirectory(folder);
                        Factory.Instance.iInfo(string.Format("Download - Created folder : {0}", folder));
                    }
                }
            }

            //get file name
            string fileUrl = fname;
            int index = fileUrl.LastIndexOf("/");
            string fileNameWithExt = string.Empty;
            string ext = string.Empty;

            if (d.setFileExtension)
            {
                if (index > 0)
                    fileNameWithExt = fileUrl.Substring(index + 1);
                else fileNameWithExt = Regex.Replace(fileNameWithExt, @"[^\w\s]", string.Empty); 
                ext = d.fileExtension;
                if (!ext.StartsWith("."))
                    ext = "." + ext;
                fileNameWithExt = Regex.Replace(fileNameWithExt, @"[^\w\s]", string.Empty); 
                fileNameWithExt = fileNameWithExt + ext;

            }
            else
            {
                if (index > 0)
                    fileNameWithExt = fileUrl.Substring(index + 1);
                else fileNameWithExt = Regex.Replace(fileNameWithExt, @"[^\w\s]", string.Empty); 
                index = fileNameWithExt.LastIndexOf(".");
                if (index > 0)
                    ext = fileNameWithExt.Substring(index + 1);
            }
           

            string fileName = fileNameWithExt.Replace(ext, "");
            string fileNameFullPath = folder + "\\" + fileNameWithExt;
            string timeStamp = IntelliScraper.Utils.GetTimestamp(DateTime.Now);
            if (d.autoRename)
            {
                fileNameFullPath = folder + "\\" + timeStamp + "." + ext;
            }

            bool downloaded = HttpUtils.downloadImage(fname, fileNameFullPath, d.customUserAgent, d.customHttpHeadersInfo);

             if (downloaded)
                {

                    bool deleteOriginal = false;
                    //chage image size
                    if (d.ImgConvertAction.changeSize)
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(fileNameFullPath);
                        if (img != null)
                        {
                            deleteOriginal = true;
                            System.Drawing.Image img2 = IntelliScraper.Utils.ResizeImage(img, new System.Drawing.Size(d.ImgConvertAction.toSizew, d.ImgConvertAction.toSizeH), true);
                            img.Dispose();
                            img2.Save(fileNameFullPath);
                            Factory.Instance.log.Info(string.Format("Image {0} resized to {1}x{2}", fileNameFullPath, d.ImgConvertAction.toSizew,d.ImgConvertAction.toSizeH));
                        }
                        else
                        {
                            Factory.Instance.log.Error(string.Format("Error resizing image {0}", fileNameFullPath));
                        }
                    }

                    //Convert to format
                    string newImgName = fileNameFullPath;
                    string extNew = ext;
                    if (d.ImgConvertAction.convertToFormat)
                    {
                        deleteOriginal = true;
                        System.Drawing.Image img = System.Drawing.Image.FromFile(fileNameFullPath);
                        string _ext = ext;
                        if(_ext.Contains("."))
                            _ext = ext.Replace(".","");
                        if (d.ImgConvertAction.convertType == Db.imageConvertConvertType.bmp)
                        {
                            string replaceImage = fileNameFullPath.Replace(_ext, "bmp");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                            extNew = "bmp";
                        }
                        if (d.ImgConvertAction.convertType == Db.imageConvertConvertType.ico)
                        {
                            string replaceImage = fileNameFullPath.Replace(_ext, "ico");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                            extNew = "ico";
                        }
                        if (d.ImgConvertAction.convertType == Db.imageConvertConvertType.jpg)
                        {
                            string replaceImage = fileNameFullPath.Replace(_ext, "jpg");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                            extNew = "jpg";
                        }
                        if (d.ImgConvertAction.convertType == Db.imageConvertConvertType.png)
                        {
                            string replaceImage = fileNameFullPath.Replace(_ext, "png");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                            extNew = "png";
                        }
                        if (d.ImgConvertAction.convertType == Db.imageConvertConvertType.tiff)
                        {
                            string replaceImage = fileNameFullPath.Replace(_ext, "tiff");
                            img.Save(replaceImage, ImageFormat.Jpeg);
                            newImgName = replaceImage;
                            extNew = "tiff";
                        }
                        Factory.Instance.iInfo(string.Format("Image {0} converted to {1}", fileNameFullPath, d.ImgConvertAction.convertType.ToString()));
                        img.Dispose();
                    }
                    //Create Thumbs
                    if (d.ImgConvertAction.createThumbs)
                    {
                        //create thumb folder
                        string thumbFolder = folder + "\\thumbs";
                        if (d.ImgConvertAction.createThumbFolder)
                        {
                            if (!System.IO.Directory.Exists(thumbFolder))
                            {
                                System.IO.Directory.CreateDirectory(thumbFolder);
                                Factory.Instance.iInfo(string.Format("Download - Created folder : {0}", thumbFolder));
                            }
                        }

                        //generate thumbs
                        System.Drawing.Image img = System.Drawing.Image.FromFile(newImgName);
                        foreach (Db.imageConvertThumbs thumb in d.ImgConvertAction.Thumbs)
                        {
                            string thumbName = thumbFolder + "\\" + thumb.name + "." + extNew.Replace(".", ""); ;
                            if (d.autoRename)
                                thumbName = thumbFolder + "\\" + thumb.name + "_" + timeStamp + "." + ext;
                            System.Drawing.Image t = IntelliScraper.Utils.ResizeImage(img, new System.Drawing.Size(thumb.toSizeW, thumb.toSizeH), true);
                            t.Save(thumbName);
                            Factory.Instance.iInfo(string.Format("Generate thumb {0} - {1}x{2}", thumbName, thumb.toSizeW, thumb.toSizeH));
                        }
                        img.Dispose();
                    }

                    if (deleteOriginal)
                    {

                        System.IO.File.Delete(fileNameFullPath);
                    }

                }
             else
             {
                 Factory.Instance.log.Error(string.Format("Error downloading image {0} for rule {1}", fname, d.id));
             }
             return fileNameFullPath;

        }
             

        /// <summary>
        /// Get files from (List<KeyValuePair<string, object>> 
        /// </summary>
        private List<string> getFiles(List<KeyValuePair<string, object>> val)
        {
            List<string> files = new List<string>();
            if (val != null)
            {
                foreach (KeyValuePair<string, object> v in val)
                {
                    
                        if (v.Key == d.inputAttributeKey)
                            files.Add((string)v.Value);
                    
                }
            }
            return files;
        }

        /// <summary>
        /// Get files from List<List<KeyValuePair<string, object>>>
        /// </summary>
        private List<string> getFiles( List<List<KeyValuePair<string, object>>> vals)
        {
            List<string> files = new List<string>();
            if (vals != null)
            {
                foreach (List<KeyValuePair<string, object>> val in vals)
                {
                    if (val != null)
                    {
                        foreach (KeyValuePair<string, object> v in val)
                        {
                                if (v.Key == d.inputAttributeKey)
                                    files.Add((string)v.Value);
                            
                        }
                    }
                }
            }
            return files;
        }
    }
}
