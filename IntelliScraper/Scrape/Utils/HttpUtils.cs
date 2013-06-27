using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Globalization;

namespace IntelliScraper.Scrape
{
    public class HttpUtils
    {
        public string currentHtml { get; set; }
        bool asyncEnd { get; set; }

        /// <summary>
        /// Get html Wrapper
        /// </summary
        public static string getHtml(string url, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders, bool async)
        {
            if (async)
                return new HttpUtils().getHtmlAsync(url, customUserAgent,httpHeaders);
            else return getHtml(url, customUserAgent, httpHeaders);
        }

        /// <summary>
        /// Get html Async
        /// </summary>
        private string getHtmlAsync(string url, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders)
        {
            try
            {

                //set new proxy if use proxy
                CookieAwareWebClient client = Factory.Instance.getWebClientNext(customUserAgent,httpHeaders);
                client = setCredential(client, url);              

                //Set Wait time
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitEachRequestMilliseconds);

                //Stream s = 
                asyncEnd = false;
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
                client.OpenReadAsync(new Uri(url));
                while (asyncEnd == false)
                    System.Threading.Thread.Sleep(10);
                return this.currentHtml;

            }
            catch (Exception ex)
            {
                //Wait after error - prevent next request
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitAfterError);
                Factory.Instance.log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Wait Async Html Completed
        /// </summary>
        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            Stream s = (Stream)e.Result;
            //Stream s = Factory.Instance.client.OpenRead(url); ;
            StreamReader sr = new StreamReader(s);
            this.currentHtml = sr.ReadToEnd();
            s.Close();
            asyncEnd = true;
        }

        /// <summary>
        /// Get html
        /// </summary
        private static string getHtml(string url, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders)
        {
            try
            {

                //set new proxy if use proxy
                CookieAwareWebClient client = Factory.Instance.getWebClientNext(customUserAgent,httpHeaders);
                client = setCredential(client, url);

                //Set Wait time
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitEachRequestMilliseconds);


                Stream s = client.OpenRead(url); ;
                StreamReader sr = new StreamReader(s);
                string html = sr.ReadToEnd();
                s.Close();
                return html;
            }
            catch (Exception ex)
            {
                //Wait after error - prevent next request
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitAfterError);
                Factory.Instance.log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Download image
        /// </summary>
        public static bool downloadImage(string imgUrl, string saveTo, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders)
        {
            try
            {
                WebClient client = Factory.Instance.getWebClientNext(customUserAgent,httpHeaders);
                using (client)
                {
                    //download image
                    client.DownloadFile(imgUrl, saveTo);
                    return true;

                }
            }
            catch (Exception ex)
            {
                Factory.Instance.log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Post HTML
        /// </summary>
        public static string postHtml(string url, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders, List<KeyValuePair<string, string>> parameters)
        {
            //generate post data
            string postData = string.Empty;
            foreach (KeyValuePair<string, string> p in parameters)
            {
                postData += string.Format("{0}={1}&",p.Key,p.Value);
            }
            postData = postData.TrimEnd('&');
            postData = System.Web.HttpUtility.UrlEncode(postData);

            //set new proxy if use proxy
            CookieAwareWebClient client = Factory.Instance.getWebClientNext(customUserAgent,httpHeaders);
            client = setCredential(client, url);

          
            using (client)
            {
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.ContentType]))
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.Accept]))
                    client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.AcceptEncoding]))
                    client.Headers.Add(HttpRequestHeader.AcceptEncoding, "identity");
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.AcceptLanguage]))
                    client.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                return client.UploadString(url, "POST", postData);
                
            }
           
        }

        /// <summary>
        /// Post HTML
        /// </summary>
        public static string postHtml(string url, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders, string postData)
        {
           
            //set new proxy if use proxy

            CookieAwareWebClient client = Factory.Instance.getWebClientNext(customUserAgent, httpHeaders);
            client = setCredential(client, url);

         
            using (client)
            {
               
                if(string.IsNullOrEmpty(client.Headers[HttpRequestHeader.ContentType]))
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.Accept]))
                    client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.AcceptEncoding]))
                    client.Headers.Add(HttpRequestHeader.AcceptEncoding,"identity");
                if (string.IsNullOrEmpty(client.Headers[HttpRequestHeader.AcceptLanguage]))
                    client.Headers.Add(HttpRequestHeader.AcceptLanguage,"en-US,en;q=0.8");
                return client.UploadString(url, "POST", postData);

            }
           
        }

        /// <summary>
        /// Upload multiple files with post data
        /// </summary>
        public static string UploadFiles(string address, IEnumerable<Model.UploadFile> files, NameValueCollection values)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
         
            //set webRequest proxy and credential
            var request = WebRequest.Create(address);
            if (Factory.Instance.i.Project.ProxyInfo != null)
            {
                if (Factory.Instance.i.Project.ProxyInfo.useProxy)
                {
                    ProxyModel p = Factory.Instance.proxyManager.getNext();
                    WebProxy proxy = new WebProxy(p.Ip,Int32.Parse(p.Port));
                    if(!string.IsNullOrEmpty(p.Username) && !string.IsNullOrEmpty(p.Password))
                    {
                        if(!string.IsNullOrEmpty(p.Domain))
                            proxy.Credentials = new NetworkCredential( p.Username, p.Password,p.Domain);
                        else proxy.Credentials = new NetworkCredential( p.Username, p.Password);
                                 
                    }
                    request.Proxy = proxy;
                }
            }
            if (Factory.Instance.i.Project.ScrapingSetting.credential != null)
            {
                if (Factory.Instance.i.Project.ScrapingSetting.credential.useNetworkCredential)
                {
                    if(!string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.username) && !string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.password))
                    {
                        if(!string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.domain))
                            request.Credentials = new NetworkCredential(Factory.Instance.i.Project.ScrapingSetting.credential.username, Factory.Instance.i.Project.ScrapingSetting.credential.password, Factory.Instance.i.Project.ScrapingSetting.credential.domain);
                        else request.Credentials = new NetworkCredential(Factory.Instance.i.Project.ScrapingSetting.credential.username, Factory.Instance.i.Project.ScrapingSetting.credential.password);
                    }
                    
                }
            }
          

            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (var file in files)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                byte[] rawResponse = stream.ToArray();
                string r = System.Text.Encoding.ASCII.GetString(rawResponse);
                return r;           
            }
        }

        /// <summary>
        /// Upload file without post Data
        /// </summary>
        public static string uploadFileSimple(string url, string fileFullPath, string customUserAgent, Db.HttpHeadersInfoCollection httpHeaders,string method)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                //set new proxy if use proxy
                CookieAwareWebClient client = Factory.Instance.getWebClientNext(customUserAgent, httpHeaders);
                client = setCredential(client, url);

                //Set Wait time
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitEachRequestMilliseconds);

                           

                
                byte[] rawResponse = client.UploadFile(url, method, fileFullPath);
                string response = System.Text.Encoding.ASCII.GetString(rawResponse);
                return response;
            }
            catch (Exception ex)
            {
                //Wait after error - prevent next request
                System.Threading.Thread.Sleep(Factory.Instance.i.Project.ScrapingSetting.waitAfterError);
                Factory.Instance.log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Set CookieAwareWebClient Credential by configuration
        /// </summary>        
        private static CookieAwareWebClient setCredential(CookieAwareWebClient client,string url)
        {
            if (Factory.Instance.i.Project.ScrapingSetting.credential != null)
            {
                if (Factory.Instance.i.Project.ScrapingSetting.credential.useNetworkCredential)
                {
                    var credCache = new CredentialCache();
                    if (!string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.username) && !string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.password) && !string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.domain))
                    {
                        credCache.Add(new Uri(url), "Negotiate", new NetworkCredential(Factory.Instance.i.Project.ScrapingSetting.credential.username, Factory.Instance.i.Project.ScrapingSetting.credential.password, Factory.Instance.i.Project.ScrapingSetting.credential.domain));
                        client.Credentials = credCache;
                    }
                    if (!string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.username) &&
                        !string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.password) &&
                        string.IsNullOrEmpty(Factory.Instance.i.Project.ScrapingSetting.credential.domain))
                    {
                        credCache.Add(new Uri(url), "Negotiate", new NetworkCredential(Factory.Instance.i.Project.ScrapingSetting.credential.username, Factory.Instance.i.Project.ScrapingSetting.credential.password));
                        client.Credentials = credCache;
                    }
                }
            }
            return client;
        }

    }
}
