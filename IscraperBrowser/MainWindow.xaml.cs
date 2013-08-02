using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Reflection;
using mshtml;

namespace IscraperBrowser
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        mshtml.IHTMLElement currentEl { get; set; }
        public MainWindow()
        {
            InitializeComponent();
           
           
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser1.Navigate(new Uri("http://stackoverflow.com/"));
          
        }

        private void webBrowser1_MouseUp(object sender, MouseButtonEventArgs e)
        {
           dynamic doc = webBrowser1.Document;
           dynamic htmlText = doc.documentElement.InnerHtml;
        }

        /// <summary>
        /// On click on webBrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            HideScriptErrors(webBrowser1,true);
            load(true,e);
           
           
        }

        private void load(bool cancel, NavigatingCancelEventArgs e)
        {
            mshtml.HTMLDocument c = (mshtml.HTMLDocument)webBrowser1.Document;
            if (c != null)
            {
                if (currentEl != null)
                    currentEl.style.backgroundColor = "#ffffff";

                richTextBox1.Document.Blocks.Clear();
                if(e != null)
                    e.Cancel = cancel;
                string html = c.activeElement.outerHTML;
                currentEl = c.activeElement;
                c.activeElement.style.backgroundColor = "#F0FF06";
                richTextBox1.AppendText(html);
            }
        }

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            mshtml.HTMLDocument doc = (mshtml.HTMLDocument)webBrowser1.Document;
            mshtml.HTMLDocumentEvents2_Event iEvent = (mshtml.HTMLDocumentEvents2_Event)doc;
            iEvent.onclick += new mshtml.HTMLDocumentEvents2_onclickEventHandler(iEvent_onclick);

           
           foreach (IHTMLElement element in doc.all)
           {
               element.style.border = "1px solid";
               element.style.borderColor = "#FF0000";
           }
        }

        bool iEvent_onclick(mshtml.IHTMLEventObj pEvtObj)
        {
            if (currentEl != null)
                currentEl.style.backgroundColor = "#ffffff";

            mshtml.HTMLDocument doc = (mshtml.HTMLDocument)webBrowser1.Document;
            mshtml.IHTMLWindow2 win = doc.parentWindow;
            richTextBox1.Document.Blocks.Clear();
            currentEl = win.@event.srcElement;
            richTextBox1.AppendText(win.@event.srcElement.outerHTML);

            foreach (IHTMLElement el in win.@event.srcElement.parentElement.all)
            {
                if(el.tagName == win.@event.srcElement.tagName)
                    el.style.backgroundColor = "#F0FF06";

                foreach (IHTMLElement e in el.all)
                {
                    if (e.tagName == win.@event.srcElement.tagName)
                        el.style.backgroundColor = "#F0FF06";
                }
            }

            win.@event.srcElement.style.backgroundColor = "#F0FF06";
            return false;
        }
       

        private void webBrowser1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            mshtml.HTMLDocument c = (mshtml.HTMLDocument)webBrowser1.Document;
            if (c != null)
            {
                string html = c.activeElement.innerHTML;
            }
            
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember(
                "Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide }
            );
        }

        private void webBrowser1_MouseEnter(object sender, MouseEventArgs e)
        {
            load(false,null);
        }

        
    }
}
