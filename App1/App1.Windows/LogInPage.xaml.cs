using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {
        public LogInPage()
        {
            this.InitializeComponent();
        }
        public string username, password;
        private void UsernameChanged(object sender, TextChangedEventArgs e)
        {
            username = Username.Text; // how to grab user input
            Class1.setem(username);
        }

        private void PasswordTextBox(object sender, TextChangedEventArgs e)
        {
            password = Password.Text;
            Class1.setpw(password);
        }
   

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Maps.log_in(Class1.getem(), Class1.getpw());
            
            Frame.Navigate(typeof(Maps));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }





    
    }
}
