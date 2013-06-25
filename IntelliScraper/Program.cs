using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using HtmlAgilityPack;

namespace IntelliScraper
{
  
    public class Program
    {
        static void Main(string[] args)
        {
            
           
            //Factory.Instance.pluginManager.runPlugin("test1");
            string fname = @"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\yh.xml";
            Factory.Instance.Run(fname);

          /*   List<KeyValuePair<string, object>> data = new List<KeyValuePair<string, object>>();
            data.Add(new KeyValuePair<string, object>("Repository", "repo1"));
            data.Add(new KeyValuePair<string, object>("Likes", "100"));
            data.Add(new KeyValuePair<string, object>("Fork", "100"));
            data.Add(new KeyValuePair<string, object>("description", "testme"));

            Database.DbManager db = new Database.DbManager("Server=localhost;Database=test;Uid=admin;Pwd=;", "MySql.Data.MySqlClient");

            List<string> checkAttr= new List<string>();
            checkAttr.Add("Repository");

            List<string> attr = new List<string>();
            attr.Add("Repository");
            attr.Add("Likes");
            attr.Add("Fork");
            attr.Add("description");
            bool exist = db.exist(checkAttr, "gitbesttoday", data);
            if (!exist)
                db.insert(attr, "gitbesttoday", data);
           /* ICollection<KeyValuePair<String, Object>> coll =(ICollection<KeyValuePair<String, Object>>) db.FirstOrDefault<object>("select * from data1 where name=@0", "nome");
            
            
            foreach (KeyValuePair<string, object> c in coll)
            {
                string p = "";
            }*/

          /* object xx = db.FirstOrDefault<object>("select * from data1 where name=@0", "nome");
           KeyValuePair<object, object> datax = db.FirstOrDefault<KeyValuePair<object, object>>("select * from data1 where name=@0", "nome");
          /*  foreach (object o in (ICollection)datax)
            {
                string tmp = "";
            }*/
        }

        private void httpPostTest()
        {
            var credCache = new CredentialCache();
            credCache.Add(new Uri("http://srvqlik01/QlikPortal/login.aspx"), "Negotiate", new NetworkCredential("sp2can", "yh4393xa", "intranet"));

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = credCache;

            string url = "http://srvqlik01/QlikPortal/login.aspx";




            Stream s = client.OpenRead(url); ;
            StreamReader sr = new StreamReader(s);
            string html = sr.ReadToEnd();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode n = doc.DocumentNode.SelectSingleNode("//*[@id=\"__VIEWSTATE\"]");
            string viewstateVal = n.Attributes["value"].Value;
            n = doc.DocumentNode.SelectSingleNode("//*[@id=\"__EVENTVALIDATION\"]");
            string eventValidation = n.Attributes["value"].Value;

            viewstateVal = System.Web.HttpUtility.HtmlDecode(viewstateVal);
            eventValidation = System.Web.HttpUtility.HtmlDecode(eventValidation);
            viewstateVal = viewstateVal.Replace("/", "%2F");
            eventValidation = eventValidation.Replace("/", "%2F");
            viewstateVal = viewstateVal.Replace("=", "%3D");
            eventValidation = eventValidation.Replace("=", "%3D");
            viewstateVal = viewstateVal.Replace("+", "%2B");
            eventValidation = eventValidation.Replace("+", "%2B");


            string _prms = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl00%24ContentPlaceHolder1%24txtUtente=sp2can&ctl00%24ContentPlaceHolder1%24txtPassword=yh4393xa&ctl00%24ContentPlaceHolder1%24txtDominio=intranet&ctl00%24ContentPlaceHolder1%24btnAccedi=Accedi", viewstateVal, eventValidation);
            client.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            client.Headers.Add(HttpRequestHeader.AcceptLanguage, "it-IT,it;q=0.8,en-US;q=0.5,en;q=0.3");
            client.Headers.Add(HttpRequestHeader.Host, "srvqlik01");
            client.Headers.Add(HttpRequestHeader.Referer, "http://srvqlik01/QlikPortal/login.aspx");
            client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0");
            client.Headers.Add("Content-Type: application/x-www-form-urlencoded");

            string HtmlResult = client.UploadString(url, "POST", _prms);
           

        }
        
    }
}
