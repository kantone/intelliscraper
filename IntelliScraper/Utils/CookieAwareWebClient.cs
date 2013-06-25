using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace IntelliScraper
{
    public class CookieAwareWebClient : WebClient
    {
     
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
            WebRequest request = base.GetWebRequest(address);

            var castRequest = request as HttpWebRequest;
            if (castRequest != null)
            {
                castRequest.CookieContainer = this.CookieContainer;     
            }

            return request;
        }
    }
}
