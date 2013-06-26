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

    public class Plugin : IScrapeAction
    {
        public string Name = "Plugin";
        Db.plugin d { get; set; }
        HttpWebRequest webRequest { get; set; }
        public Plugin(Db.plugin d)
        {
            this.d = d;
        }

        public string getName()
        {
            return "Plugin";
        }

        public object Run(object input)
        {
            IScrapeAction a =  Factory.Instance.pluginManager.getPluginByName(d.pluginName);
            if(a!=null)
                return a.Run(input);
            return null;
        }

      

    

     

      
    }



 
}
