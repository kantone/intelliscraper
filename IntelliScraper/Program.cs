using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using HtmlAgilityPack;
using OfficeOpenXml;

namespace IntelliScraper
{
  
    public class Program
    {
        static void Main(string[] args)
        {
           /* FileInfo newFile = new FileInfo("c:\\test2.xlsx");
            ExcelPackage _book = new ExcelPackage(newFile);
           /* _book.Workbook.Worksheets.Add("W1");
            _book.Save();*/

           /* ExcelWorksheet sheet =  _book.Workbook.Worksheets["W1"];
            sheet.Cells[1, 1].Value = "TEST";
            //_book.Workbook.Worksheets.Add("W1",sheet);
            _book.Save();*/

            

            //string fname = @"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\yh.xml";
            string fnameCrypted = @"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\yh_crypted.xml";
            
        /*    string crypted = IntelliScraper.Crypto.EncryptStringAES(System.IO.File.ReadAllText(fname),"yh4393xr");
            System.IO.File.WriteAllText(@"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\yh_crypted.xml",crypted);

            string uncrypt = IntelliScraper.Crypto.DecryptStringAES(System.IO.File.ReadAllText(@"F:\sp2can\P\_me\prjects-publics\IntelliScraper\intelliscraper\IntelliScraper\testData\yh_crypted.xml"), "yh4393xr");
            */
            Factory.Instance.Run(fnameCrypted, "yh4393xr");

          
        }

      
    }
}
