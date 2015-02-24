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
    public sealed partial class SignUpPage : Page
    {
        public string fname, lname, email, pw;

        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            fname = FirstName.Text;
            Class1.setfn(fname);
        }

        private void LastName_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            lname = LastName.Text;
            Class1.setlname(lname);
        }

        private void Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            email = Email.Text;
            Class1.setem(email);
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            //Confirm Email
        }

        private void Password_TextChanged_5(object sender, TextChangedEventArgs e)
        {
            //Password
            pw = Password.Text;
            Class1.setpw(pw);
        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            //Confirm Password
        }


        private void signUp_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LogInPage));
            Maps.register_new_user(Class1.getem(),Class1.getpw(),Class1.getfn(),Class1.getlname());
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        
      
    }
}
