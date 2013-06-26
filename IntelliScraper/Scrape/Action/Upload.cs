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



namespace IntelliScraper.Scrape.Action
{

    public class Upload : IScrapeAction
    {
      
        Db.upload d { get; set; }
        public Upload(Db.upload d)
        {
            this.d = d;
        }

        public string getName()
        {
            return "Upload";
        }

        public object Run(object input)
        {
            if (d != null)
            {
                string file = string.Empty;
                if(d.inputType == Db.uploadInputType.fromInput)
                    file = InputUtils.getValueFromInputByType(input, d.inputAttributeKey);
                if (d.inputType == Db.uploadInputType.custom)
                    file = d.customFileToUpload;

                string method = "POST";
                if(d.method == Db.uploadMethod.GET)
                    method = "GET";
                if(d.method == Db.uploadMethod.PUT)
                    method = "PUT";

                 return  HttpUtils.uploadFile(d.uploadUrl, file, d.customUserAgent, d.customHttpHeadersInfo, method);
            }
            return string.Empty; ;
        }

        

    

     

      
    }



 
}
