using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IntelliScraper
{
    //http://msdn.microsoft.com/it-it/library/ms229715.aspx
    public class FtpManager
    {
        FtpWebRequest request { get; set; }
        string user { get; set; }
        string pass { get; set; }
        bool authenticate { get; set; }
        string ftpServerString { get; set; }
        string domain { get; set; }
        public FtpManager(string ftpServerString,bool authenticate,string user,string pass,string domain)
        {
            this.authenticate = authenticate;
            this.user = user;
            this.pass = pass;
            this.ftpServerString = ftpServerString;
            this.domain = domain;
        }

        public string upload(string file)
        {
            if (System.IO.File.Exists(file))
            {
                System.IO.FileInfo finfo = new FileInfo(file);
                if (!ftpServerString.EndsWith("/"))
                    ftpServerString += "/";
                ftpServerString += finfo.Name;
                request = (FtpWebRequest)WebRequest.Create(ftpServerString);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                if (authenticate)
                {
                    if(string.IsNullOrEmpty(this.domain))
                        request.Credentials = new NetworkCredential(user, pass);
                    else request.Credentials = new NetworkCredential(user, pass, domain);
                }

                // Copy the contents of the file to the request stream.
                StreamReader sourceStream = new StreamReader(file);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Factory.Instance.iInfo(string.Format("Upload File Complete, status {0}", response.StatusDescription));

                return response.ExitMessage;
            }
            else
            {
                string msg = string.Format("Ftp file {0} not exist", file);
                Factory.Instance.log.Error(msg);
                return msg;
            }
        
        }

        public bool testConnection()
        {

            request = (FtpWebRequest)WebRequest.Create(ftpServerString);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            if (authenticate)
            {
                if (string.IsNullOrEmpty(this.domain))
                    request.Credentials = new NetworkCredential(user, pass);
                else request.Credentials = new NetworkCredential(user, pass, domain);
            }


            try
            {
                WebResponse r = request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }

        }
        
    }
}
