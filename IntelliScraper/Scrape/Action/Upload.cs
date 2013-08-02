using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Packaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;
using System.Collections.Specialized;



namespace IntelliScraper.Scrape.Action
{

    public class Upload 
    {
      
        Db.upload d { get; set; }
        public Upload(Db.upload d)
        {
            this.d = d;
        }
        
        public string Run(string file)
        {
            if (d != null)
            {           
               if (d.inputType == Db.uploadInputType.custom)
                    file = d.customFileToUpload;

                string method = "POST";
                if(d.method == Db.uploadMethod.GET)
                    method = "GET";
                if(d.method == Db.uploadMethod.PUT)
                    method = "PUT";

                if (d.usePostData)
                {

                     Model.UploadFile f = new Model.UploadFile();
                     FileInfo finfo = new FileInfo(file);
                     f.Filename = file;
                     f.Stream = File.Open(file, FileMode.Open);
                     f.ContentType = MimeTypeUtils.GetMimeType(finfo.Extension);
                     f.Name = finfo.Name;
                    
                     List< Model.UploadFile> files = new List<Model.UploadFile>();
                     files.Add(f);

                    //add post params
                     NameValueCollection values = new NameValueCollection();
                     foreach (Db.uploadPostData p in d.postData)
                         values.Add(p.key, p.value);

                     string res = HttpUtils.UploadFiles(d.uploadUrl,files, values);
                     f.Stream.Close();
                     return res;
                    
                }
                else return HttpUtils.uploadFileSimple(d.uploadUrl, file, d.customUserAgent, d.customHttpHeadersInfo, method);
            }
            return string.Empty; ;
        }

      
    }



 
}
