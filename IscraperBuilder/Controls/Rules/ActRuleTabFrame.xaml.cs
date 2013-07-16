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

namespace IscraperBuilder.Controls.Rules
{
    /// <summary>
    /// Logica di interazione per ActRuleTabFrame.xaml
    /// </summary>
    public partial class ActRuleTabFrame : Page
    {
        public ActRuleTabFrame()
        {
            InitializeComponent();
        }

        public void setFrame1(object o)
        {
            this.frame1.Navigate(o);
        }

        public void setFrame2(object o)
        {
            this.frame2.Navigate(o);
        }

        public void setFrames(object o, object o2)
        {
            this.frame1.Navigate(o);
            this.frame2.Navigate(o2);
        }
    }
}
