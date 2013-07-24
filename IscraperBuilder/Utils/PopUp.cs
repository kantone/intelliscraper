using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;

namespace IscraperBuilder.Utils
{
    public class PopUp
    {
        public Popup popup { get; set; }
        public PopUp()
        {

        }

        public void hide()
        {
            if (popup != null)
                popup.IsOpen = false;
        }

        public void showCannotSave(object control)
        {
            show("Error (cannot save)", control);
        }

        public void show(string message, object control)
        {
            if(popup == null)
                popup = new Popup();
            //popup.Name = "somepopup";
            popup.PlacementTarget = (System.Windows.UIElement)control;
            popup.Placement = PlacementMode.Bottom;
            TextBlock txtblock = new TextBlock();
            txtblock.Background = Brushes.White;
            txtblock.Foreground = Brushes.Red;
            txtblock.Text = "* " + message;
            popup.Child = txtblock;
            popup.StaysOpen = false;
            popup.Height = 20;
            popup.Width = 200;
            popup.IsOpen = true;
        }

        public static Popup showPopUpError(string message, object control)
        {
            Popup popup = new Popup();
            //popup.Name = "somepopup";
            popup.PlacementTarget = (System.Windows.UIElement)control;
            popup.Placement = PlacementMode.Bottom;
            TextBlock txtblock = new TextBlock();
            txtblock.Background = Brushes.White;
            txtblock.Foreground = Brushes.Red;
            txtblock.Text = "* " + message;
            popup.Child = txtblock;            
            popup.StaysOpen = false;
            popup.Height = 20;
            popup.Width = 200;
            popup.IsOpen = true;
            return popup;
        }
    }
}
