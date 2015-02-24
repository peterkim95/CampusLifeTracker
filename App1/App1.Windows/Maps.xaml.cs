 using App1.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using System.ComponentModel;
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
using Windows.Devices.Sensors;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Bing.Maps;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237
namespace App1
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Maps : Page
    {
        /* DATABASE IMPLEMENTATION STARTS HERE */
        public static int user_id = -1;

        static SQLiteAsyncConnection connection = new SQLiteAsyncConnection("User.db");
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await create_db();
            //string sql = "SELECT * FROM Users WHERE id = " + user_id + " LIMIT 1";
            //List<User> users = await connection.QueryAsync<User>(sql);
            //update_pushpins(users[0]);
            navigationHelper.OnNavigatedTo(e);
        }

        public async Task create_db()
        {
            await connection.CreateTableAsync<User>();
            await connection.CreateTableAsync<UserPin>();
            var firstPin = myMap.Children.First();
        }

        public async void add_userpin(TappedRoutedEventArgs e, Pushpin p, int locationType, String LocationName, String message, int hour, int minute, int ampm)
        {
            var up = new UserPin()
            {
                user_id = user_id,
                x_coord = 0,
                y_coord = 0,
                ampm = ampm,
                checkin_hour = hour,
                checkin_minute = minute,
                location_name = LocationName,
                personal_message = message,
                location_type = locationType, // 0 - Study, 1 - Eat, 2 - Paste
                pp = p.TabIndex.ToString()
            };
            string sql = "SELECT * FROM Users WHERE id = " + user_id + " LIMIT 1";
            List<User> users = await connection.QueryAsync<User>(sql);
            drop_pushpin(users[0], e, p);
            await connection.InsertAsync(up);
        }
        public async void add_userpin(Pushpin p, int locationType, String LocationName, String message, int hour, int minute, int ampm)
        {
            var up = new UserPin()
            {
                user_id = user_id,
                x_coord = 0,
                y_coord = 0,
                ampm = ampm,
                ghost = true,
                checkin_hour = hour,
                checkin_minute = minute,
                location_name = LocationName,
                personal_message = message,
                location_type = locationType, // 0 - Study, 1 - Eat, 2 - Paste
                pp = p.TabIndex.ToString()
            };
            string sql = "SELECT * FROM Users WHERE id = " + user_id + " LIMIT 1";
            List<User> users = await connection.QueryAsync<User>(sql);
            drop_pushpin(users[0],p);
            await connection.InsertAsync(up);
        }

        public static async void log_in(string em, string pw)
        {
            string sql = "SELECT * FROM Users WHERE email = '" + em + "'";
            List<User> users = await connection.QueryAsync<User>(sql);
            if (users.Count != 0)
            {
                if (users[0].password == pw)
                {
                    user_id = users[0].id;


                }

            }
            else
            {
                // No user with that email exists.
                throw new ArgumentNullException("FUCKKK");
            }
            //User person = users[0];
            // connection.Table<User>().Where(m => m.email == em);
        }

        public async static void register_new_user(string em, string pw, string fname, string lname)
        {
            var User = new User()
            {
                email = em,
                lastname = lname,
                firstname = fname,
                password = pw
            };
            await connection.InsertAsync(User);
        }


        /*
        public async void set_personal_message(string message)
        {
            string sql = "UPDATE UserPin SET personal_message = " + message + " WHERE user_id = "+user_id;
            await connection.QueryAsync<User>(sql);
        }
        */
        public async void check_in()
        {
            string sql = "UPDATE Users SET is_at_location = TRUE WHERE id = " + user_id;
            await connection.QueryAsync<User>(sql);

        }

        public async void update_pushpins(User u)
        {
            string sql = "SELECT * FROM Users";
            List<User> users = await connection.QueryAsync<User>(sql);
            for (int i = 0; i < users.Count; i++)
            {
                string sql2 = "SELECT * FROM UserPins WHERE user_id = " + users[i].id + " LIMIT 1";
                List<UserPin> userpins = await connection.QueryAsync<UserPin>(sql2);
                for (int j = 0; j < userpins.Count; j++)
                {
                    //drop_pushpin(u,e,p);
                    if (self_radius(u, userpins[j]))
                    {
                        if (userpins[j].ghost == false) {
                            remove_pin(userpins[j]);
                            return;
                        }
                        drop_pushpin(u, new Pushpin());
                        userpins[j].ghost = false;
                    }
                }
            }
        }

        public bool self_radius(User u, UserPin up)
        {
            if (u.x_coord >= (up.x_coord - 0.0001) && u.x_coord <= (up.x_coord + 0.0001))
            {
                if (u.y_coord >= (up.y_coord - 0.0001) && u.y_coord <= (up.y_coord + 0.0001))
                {
                    u.is_at_location = true;
                    return u.is_at_location;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public async void remove_pin(UserPin up)
        {
            up.view = false;
            string searchText = up.pp;

            Pushpin toRemove = null;
            foreach (UIElement elem in myMap.Children)
            {
                Pushpin pushPin = (Pushpin)elem;
                if (pushPin.TabIndex.ToString() == up.pp)
                {
                    toRemove = pushPin;
                }
            }

            if (toRemove != null)
            {
                myMap.Children.Remove(toRemove);
            }

            string sql = "DELETE FROM UserPins WHERE id = " + up.id;
            await connection.QueryAsync<UserPin>(sql);
        }

        /*
        public async void update_yourself()
        {
            string sql = "SELECT * FROM Users WHERE id = " + user_id;
            List<User> users = await connection.QueryAsync<User>(sql);
            string sql2 = "SELECT * FROM UserPins WHERE user_id = "  + user_id;
            List<User> pins = await connection.QueryAsync<User>(sql2);
            if (users.Count != 0) 
            {
                for(int i = 0)
                if(user_is_at_location(users[0]))
                {
                    drop_pushpin(users[0]);
                }
                
            }

        }

                }
            }
        }
        */
        /* DATABASE IMPLEMENTATION ENDS HERE */

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public Maps()
        {
            this.InitializeComponent();
            InitializeMap();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        void InitializeMap()
        {
            var bounds = Window.Current.Bounds;
            double height = bounds.Height;
            double width = bounds.Width;
            myMap.Center = new Location(33.975023, -118.426128);
            myMap.ZoomLevel = 17;
            myMap.MapType = MapType.Road;
            myMap.Width = width * 0.7;
            myMap.Height = height * 0.9;
        }
        // private async void pushpinTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        // MessageDialog dialog = new MessageDialog("Hello from Seattle.");
        //await dialog.ShowAsync();
        //}

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private async void myMap_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Point position = e.GetPosition(myMap);
            Point position = e.GetPosition(myMap);
            //drop_pushpin(u,position);
            Pushpin pp = new Pushpin();
            string sql = "SELECT * FROM Users WHERE id = " + user_id + " LIMIT 1";
            List<User> users = await connection.QueryAsync<User>(sql);
            string sql1 = "SELECT * FROM Users";
            List<User> users1 = await connection.QueryAsync<User>(sql1);
            Location location;
            bool flg=false;
            if (myMap.TryPixelToLocation(position, out location))
            {
                for (int i = 0; i < users1.Count;i++ )
                 {
                    string sql2 = "SELECT * FROM UserPins WHERE id = " + users1[i].id;
                    List<UserPin> userpins = await connection.QueryAsync<UserPin>(sql2);
                    for(int j=0;j<userpins.Count;j++)
                    {
                        double range = (double)(20-myMap.ZoomLevel) * 0.001;
                        if(location.Latitude-range<=userpins[j].x_coord&&location.Latitude+range>=userpins[j].x_coord)
                        {
                            if (location.Longitude - range <= userpins[j].y_coord && location.Longitude + range >= userpins[j].y_coord)
                            {
                                flg=setUpBubble(userpins[j],users1[i]);         
                            }
                        }
                    }
                }
            }
            if(!flg)
            {
                update_pushpins(users[0]);
                //Location l;
                //if (myMap.TryPixelToLocation(position, out l)) { }

                add_userpin(e, pp, Class1.getlt(), Class1.getlocationname(), Class1.getpm(), Class1.gethour(), Class1.getmin(), Class1.getampm());
            }
            
        }
        bool setUpBubble(UserPin up, User u)
        {
            Popup.IsOpen = true;
            Last.Text = Last.Text+u.lastname;
            First.Text = First.Text + u.firstname;
            Loc.Text = Loc.Text + up.location_name;
            Tim.Text = Tim.Text + up.personal_message;
            return true;
        }
        private async void drop_pushpin(User u, Pushpin p)
        {
            string sql = "SELECT * FROM UserPins WHERE user_id = " + u.id + " LIMIT 1";
            List<UserPin> userpins = await connection.QueryAsync<UserPin>(sql);
            for (int i = 0; i < userpins.Count; i++)
            {
                if (!userpins[i].view)
                {
                    userpins[i].x_coord = Class1.getLatitudeX();
                    userpins[i].y_coord = Class1.getLongitudeY();
                    p.Text = u.firstname;
                    userpins[i].ghost = false;

                    if (userpins[i].ghost && Class1.getlt() == 0)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Blue);
                    }
                    else if (userpins[i].ghost && Class1.getlt() == 1)
                    {

                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                    }
                    else if (userpins[i].ghost && Class1.getlt() == 2)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 0)
                    {

                        p.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 1)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 2)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    Location L = new Location(Class1.getLatitudeX(), Class1.getLongitudeY());
                    MapLayer.SetPosition(p, L);
                    myMap.Children.Add(p);
                    myMap.SetView(L);
                    userpins[i].view = true;
                }
            }
        }
        private async void drop_pushpin(User u, TappedRoutedEventArgs e, Pushpin p)
        {
            string sql = "SELECT * FROM UserPins WHERE user_id = " + u.id + " LIMIT 1";
            List<UserPin> userpins = await connection.QueryAsync<UserPin>(sql);
            Bing.Maps.Location location;
            Point position = e.GetPosition(myMap);
            if (myMap.TryPixelToLocation(position, out location))
            {
            }
            for (int i = 0; i < userpins.Count; i++)
            {
                if (!userpins[i].view)
                {
                    userpins[i].x_coord = location.Latitude;
                    userpins[i].y_coord = location.Longitude;
                    p.Text = u.firstname;
                    userpins[i].ghost = true;
                    if (userpins[i].ghost && Class1.getlt() == 0)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Blue);
                    }
                    else if (userpins[i].ghost && Class1.getlt() == 1)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);

                    }
                    else if (userpins[i].ghost && Class1.getlt() == 2)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.White);

                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 0)
                    {

                        p.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 1)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    else if (!userpins[i].ghost && Class1.getlt() == 2)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                        /*if(userpins[i].location_type==0)
                        {
                            p.Background = new SolidColorBrush(Windows.UI.Colors.White);
                            p.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                        }
                    else if (userpins[i].location_type == 1)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    }
                    else if (userpins[i].location_type == 2)
                    {
                        p.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                        p.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    }
                    */
                    MapLayer.SetPosition(p, location);
                    myMap.Children.Add(p);
                    myMap.SetView(location);
                    userpins[i].view = true;
                }
            }
        }


        public static string pm, ln;
        private void PersonalMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            pm = "";
            Class1.setpm(pm);
        }
        private void LocationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ln = LocationName.Text;
            Class1.setlocationname(ln);
        }

        public static int type = -1;

        private void Workout_Click(object sender, RoutedEventArgs e)
        {
            type = 2;
            Class1.setlt(type);
        }

        private void Eat_Click(object sender, RoutedEventArgs e)
        {
            type = 1;
            Class1.setlt(type);
        }

        private void Study_Click(object sender, RoutedEventArgs e)
        {
            type = 0;
            Class1.setlt(type);
        }

        public static int hour = -1;

        private void _1_Click(object sender, RoutedEventArgs e)
        {
            hour = 1;
            Class1.sethour(hour);
        }

        private void _2_Click(object sender, RoutedEventArgs e)
        {
            hour = 2;
            Class1.sethour(hour);
        }

        private void _3_Click(object sender, RoutedEventArgs e)
        {
            hour = 3;
            Class1.sethour(hour);
        }

        private void _4_Click(object sender, RoutedEventArgs e)
        {
            hour = 4;
            Class1.sethour(hour);
        }

        private void _5_Click(object sender, RoutedEventArgs e)
        {
            hour = 5;
            Class1.sethour(hour);
        }

        private void _6_Click(object sender, RoutedEventArgs e)
        {
            hour = 6;
            Class1.sethour(hour);
        }


        private void _7_Click(object sender, RoutedEventArgs e)
        {
            hour = 7;
            Class1.sethour(hour);
        }

        private void _8_Click(object sender, RoutedEventArgs e)
        {
            hour = 8;
            Class1.sethour(hour);
        }

        private void _9_Click(object sender, RoutedEventArgs e)
        {
            hour = 9;
            Class1.sethour(hour);
        }

        private void _10_Click(object sender, RoutedEventArgs e)
        {
            hour = 10;
            Class1.sethour(hour);
        }

        private void _11_Click(object sender, RoutedEventArgs e)
        {
            hour = 11;
            Class1.sethour(hour);
        }

        private void _12_Click(object sender, RoutedEventArgs e)
        {
            hour = 12;
            Class1.sethour(hour);
        }

        public static int minute = -1;

        private void _00_Click(object sender, RoutedEventArgs e)
        {
            minute = 00;
            Class1.setmin(minute);
        }

        private void _10MIN_Click(object sender, RoutedEventArgs e)
        {
            minute = 10;
            Class1.setmin(minute);
        }



        private void _20_Click(object sender, RoutedEventArgs e)
        {
            minute = 20;
            Class1.setmin(minute);
        }

        private void _30_Click(object sender, RoutedEventArgs e)
        {
            minute = 30;
            Class1.setmin(minute);
        }

        private void _40_Click(object sender, RoutedEventArgs e)
        {
            minute = 40;
            Class1.setmin(minute);
        }

        private void _50_Click(object sender, RoutedEventArgs e)
        {
            minute = 50;
            Class1.setmin(minute);
        }

        private void _60_Click(object sender, RoutedEventArgs e)
        {
            minute = 60;
            Class1.setmin(minute);
        }

        public static int ampm1 = -1;

        private void AM_Click(object sender, RoutedEventArgs e)
        {
            ampm1 = 1;
            Class1.setampm(ampm1);
        }

        private void PM_Click(object sender, RoutedEventArgs e)
        {
            ampm1 = 2;
            Class1.setampm(ampm1);
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Maps));

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    
        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
                geolocation.Text = "GPS:" + geoposition.Coordinate.Latitude.ToString("0.00") + ", " + geoposition.Coordinate.Longitude.ToString("0.00");
                Class1.setUserLocation(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                Location L = new Location(Class1.getLatitudeX(), Class1.getLongitudeY());
                myMap.SetView(L);
                Pushpin pp = new Pushpin();
                add_userpin(pp, Class1.getlt(), Class1.getlocationname(), Class1.getpm(), Class1.gethour(), Class1.getmin(), Class1.getampm());
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest.
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }
        }

       

    }
}
