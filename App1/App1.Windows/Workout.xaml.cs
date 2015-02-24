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
    public sealed partial class Workout : Page
    {
        public Workout()
        {
            this.InitializeComponent();

            /*for(int n = 0; n <= 24; n++)
                hours.Items.Add(n);*/

             for(int n = 00; n <= 60; n += 10)
                minutes.Items.Add(n);

            ampm1.Items.Add("AM");
            ampm1.Items.Add("PM");


        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            for (int n = 0; n <= 24; n++)
                hours.Items.Add(n);
        }

        private void hours_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            if ((int)hours.SelectedItem == 1)
                Frame.Navigate(typeof(SignUpPage));

        }

    }
}
