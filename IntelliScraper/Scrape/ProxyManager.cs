using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public class ProxyModel
    {
        public string Ip{get;set;}
        public string Port { get; set; }
        public string Username{get;set;}
        public string Password {get;set;}
        public ProxyModel(){}
        public ProxyModel(string Ip, string Port, string Username, string Password)
        {
            this.Ip = Ip;
            this.Username = Username;
            this.Password = Password;
            this.Port = Port;
        }
    }


    public class ProxyManager
    {
        public int currentProxyRow { get; set; }
        public List<ProxyModel> ProxyList { get; set; }
        public ProxyManager(List<ProxyModel> ProxyList)
        {
            currentProxyRow = 0;
            this.ProxyList = ProxyList;
        }

        public ProxyModel getNext()
        {
            
            if (this.ProxyList != null)
            {
                if(this.ProxyList.Count > 0)
                {
                    if (currentProxyRow < this.ProxyList.Count)
                    {
                        ProxyModel res = ProxyList[currentProxyRow];
                        currentProxyRow++;
                        return res;
                    }
                    currentProxyRow = 1;

                    return ProxyList[0];
                }
            }
            return null;
        }

        public static List<ProxyModel> GetProxyList(string fileFullPath)
        {
            List<ProxyModel> p = new List<ProxyModel>();
            System.IO.StreamReader rd = new System.IO.StreamReader(fileFullPath);
            while (rd.EndOfStream != true)
            {
                string row = rd.ReadLine();
                if (!string.IsNullOrEmpty(row))
                {
                    List<string> vals = new List<string>(row.Split(';'));
                    p.Add(new ProxyModel(vals[0], vals[1], vals[2], vals[3]));
                }
            }

            return p;
        }
    }
}
