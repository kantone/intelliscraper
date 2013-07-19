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
    

    public class TextBoxStreamWriter : TextWriter
    {
        RichTextBox _output = null;

        public TextBoxStreamWriter(RichTextBox output)
        {
            
            _output = output;
        }
       

        public override void Write(char value)
        {
            //base.AppendText(value.ToString());
            _output.Dispatcher.Invoke(DispatcherPriority.Normal,
    (ThreadStart)delegate { _output.AppendText(value.ToString()); });
            //_output.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    /// <summary>
    /// Logica di interazione per Run.xaml
    /// </summary>
    public partial class Run : Page
    {
        System.Threading.Thread t { get; set; }       
        StringWriter consoleOut { get; set; }
        public Run()
        {
            InitializeComponent();
        }



      

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBoxStreamWriter tt = new TextBoxStreamWriter(richTextBox1);
            Console.SetOut(tt);
            int i = 0;
            
            
            t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(run));
            t.Start();
            
           

            
        }

        private void run(object o){

           // IntelliScraper.Factory.Instance.Run(Factory.Instance.openedFileProject);
            int i = 0;
            while (i < 1000)
            {
                Console.WriteLine(i);
                i++;
                System.Threading.Thread.Sleep(10);
            }
        }

        
    }
}
