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
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using System.Threading;


namespace IscraperBuilder.Controls.Execute
{
    

   
    /// <summary>
    /// Logica di interazione per Run.xaml
    /// </summary>
    public partial class Run : Page
    {
        System.Threading.Thread t { get; set; }
        ManualResetEvent _event = new ManualResetEvent(true); 
        StringWriter consoleOut { get; set; }
       
        public Run()
        {
           
            InitializeComponent();

            cmbrunFrom.Items.Add("all");
           
        }



      

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            btnRun.IsEnabled = false;
            btnStop.IsEnabled = true;
            TextBoxStreamWriter tt = new TextBoxStreamWriter(richTextBox1);
            Console.SetOut(tt);
           

            
            richTextBox1.Document.Blocks.Clear();
            t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(run));
            t.Start(cmbrunFrom);
                       
        }

        private void run(object o){

           /* ComboBox c = (ComboBox)o;
            string val = string.Empty;
            c.Dispatcher.Invoke((System.Windows.Forms.MethodInvoker)delegate()
            {
                val = (string)c.SelectedValue;
            });
            if (val == null)
            {
                IntelliScraper.Factory.Instance.Run(Factory.Instance.openedFileProject);
            }
            else
            {
                if (val == "all")
                    IntelliScraper.Factory.Instance.Run(Factory.Instance.openedFileProject);
                else
                {
                    IntelliScraper.Db.intelliScraperActionCollection actions = new IntelliScraper.Db.intelliScraperActionCollection();
                    foreach(var a in Factory.Instance.i.actions){
                        if (a.id == val)
                        {
                            actions.Add(a);
                            var acts = (from x in Factory.Instance.i.actions where x.input != null && x.input.actionId == a.id select x);
                            foreach (var ac in acts)
                                actions.Add(ac);
                        }
                    }
                    Factory.Instance.i.actions = actions;
                    IntelliScraper.Factory.Instance.Run(Factory.Instance.i);
                }
            }
            
            while (true)
            {
                if (IntelliScraper.Factory.Instance.isRunning)
                {
                    _event.WaitOne();
                   // Console.WriteLine(i);
                   // i++;
                }
                else break;
            }
            btnRun.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnRun.IsEnabled = true; });
            btnStop.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnStop.IsEnabled = false; });*/
        }

        /// <summary>
        /// Pause
        /// </summary>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            btnPause.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnPause.IsEnabled = false; });
            btnStart.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnStart.IsEnabled = true; });
            btnStop.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnStop.IsEnabled = false; });
            _event.Reset();
            
        }

        /// <summary>
        /// Restart
        /// </summary>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            btnPause.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnPause.IsEnabled = true; });
            btnStart.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnStart.IsEnabled = false; });
            btnStop.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { btnStop.IsEnabled = true; });
           
            _event.Set();
        }

        /// <summary>
        /// Strop
        /// </summary>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {                    
            btnRun.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        
    }

    public class TextBoxStreamWriter : TextWriter
    {
        RichTextBox _output = null;

        public TextBoxStreamWriter(RichTextBox output)
        {

            _output = output;
        }


        public override void Write(char value)
        {
            if (value != '\n')
            {
                //base.AppendText(value.ToString());
                _output.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { _output.AppendText(value.ToString()); });
                _output.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { _output.ScrollToEnd(); });
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

}
