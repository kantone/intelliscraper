using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public class UserAgentManager
    {
       
        public int currentRow { get; set; }
        public List<string> UserAgents { get; set; }
        public UserAgentManager(List<string> UserAgents)
        {
            currentRow = 0;
            this.UserAgents = UserAgents;
        }

        public string getNext()
        {
            if (this.UserAgents != null)
            {
                if (this.UserAgents.Count > 0)
                {
                    if (currentRow < this.UserAgents.Count)
                    {
                        string res = UserAgents[currentRow];
                        currentRow++;
                        return res;
                    }
                    currentRow = 1;

                    return UserAgents[0];
                }
            }
            return null;
        }

        public static List<string> GetUserAgentList(string fileFullPath)
        {
            List<string> p = new List<string>();
            System.IO.StreamReader rd = new System.IO.StreamReader(fileFullPath);
            while (rd.EndOfStream != true)
            {
                string row = rd.ReadLine();
                p.Add(row);
            }

            return p;
        }
    }

}
