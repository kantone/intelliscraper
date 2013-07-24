using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;

namespace IntelliScraper
{
    public class CookieAwareWebClient : WebClient
    {
        HttpWebRequest request { get; set; }
        bool keepAlive = false;
        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
            
        }
        public CookieAwareWebClient(CookieContainer c)
        {
            this.CookieContainer = c;
        }
        public CookieContainer CookieContainer { get; set; }


       
        protected override WebRequest GetWebRequest(Uri address)
        {
            
           
           this.request = (HttpWebRequest)base.GetWebRequest(address);
            this.request.KeepAlive = keepAlive;
            this.request.Timeout = 180000;
           //set Connecion header keep Alive
           if (keepAlive)           
               this.request.Headers.GetType().InvokeMember("ChangeInternal",BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,Type.DefaultBinder, this.request.Headers, new object[] { "Connection", "keep-alive" });

           if (!string.IsNullOrEmpty(this.request.Host))
               this.request.Headers.GetType().InvokeMember("ChangeInternal", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, Type.DefaultBinder, this.request.Headers, new object[] { "Host",this.request.Host });

            

            var castRequest = request as HttpWebRequest;
            if (castRequest != null)
            {
                castRequest.CookieContainer = this.CookieContainer;     
            }

            return request;
        }

        public void setKeepAliveOn()
        {
            this.keepAlive = true;
        }
        public void setKeepAliveOff()
        {
            this.keepAlive = false;
        }
    }
}
