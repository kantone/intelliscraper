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

    public class ScreenShot : IScrapeAction
    {
      
        Db.screenShot d { get; set; }
        HttpWebRequest webRequest { get; set; }
        public ScreenShot(Db.screenShot d)
        {
            this.d = d;
        }

        public string getName()
        {
            return "ScreenShot";
        }

        public object Run(object input)
        {         
            string url = string.Empty;
            if(d.inputType == Db.screenShotInputType.custom)
                url = d.customUrl;
            if(d.inputType == Db.screenShotInputType.fromInput)
                url = InputUtils.getValueFromInputByType(input, d.inputAttributeKey);

            if (!string.IsNullOrEmpty(url))
            {
                Thread t = new Thread(new ParameterizedThreadStart(run));
                t.SetApartmentState(ApartmentState.STA);
                t.Start(url);
                
            }
            return url;
        }

        public void run(object url)
        {
           Bitmap bitmap = GenerateScreenshot((string)url, d.width, d.height);
           bitmap.Save(d.saveTo);
        }

        public Bitmap GenerateScreenshot(string url, int width, int height) { 
            // Load the webpage into a WebBrowser control 
            WebBrowser wb = new WebBrowser(); 
            wb.ScrollBarsEnabled = false; 
            wb.ScriptErrorsSuppressed = true; wb.Navigate(url);  
            while (wb.ReadyState != WebBrowserReadyState.Complete) 
            { 
                System.Windows.Forms.Application.DoEvents();                
            }
            // Set the size of the 
            wb.Width = width; wb.Height = height;   
            if (width == -1) { 
                // Take Screenshot of the web pages full width 
                wb.Width = wb.Document.Body.ScrollRectangle.Width; 
            }   
            if (height == -1) 
            { 
                    // Take Screenshot of the web pages full height 
                    wb.Height = wb.Document.Body.ScrollRectangle.Height; 
            }   
            // Get a Bitmap representation of the webpage as it's rendered in the WebBrowser control 
            Bitmap bitmap = new Bitmap(wb.Width, wb.Height); wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height)); wb.Dispose();  
            return bitmap; 
        }

    

     

      
    }



 
}
