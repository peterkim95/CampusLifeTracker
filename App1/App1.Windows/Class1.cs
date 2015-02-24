using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public static class Class1
    {
        public static int locationType,hour,min,ampm;
        public static String email,fname,lname,pw,locationname,pm;
        public static double latitudex, longitudey;
        public static void setUserLocation(double x, double y)
        {
            latitudex = x;
            longitudey = y;
        }
        public static double getLatitudeX()
        {
            return latitudex;
        }
        public static double getLongitudeY()
        {
            return longitudey;
        }
        public static void setlt(int x)
        {
            locationType = x;
        }
        public static int getlt()
        {
            return locationType;
        }
        public static void setampm(int x)
        {
            ampm = x;
        }
        public static int getampm()
        {
            return ampm;
        }
        public static void setmin(int x)
        {
            min = x;
        }
        public static int getmin()
        {
            return min;
        }
        public static void sethour(int x)
        {
            hour = x;
        }
        public static int gethour()
        {
            return hour;
        }

        public static void setem(String x)
        {
            email = x;
        }
        public static String getem()
        {
            return email;
        }
        public static void setfn(String x)
        {
            fname = x;
        }
        public static String getfn()
        {
            return fname;
        }
        public static void setlname(String x)
        {
            lname = x;
        }
        public static String getlname()
        {
            return lname;
        }
        public static void setpw(String x)
        {
            pw = x;
        }
        public static String getpw()
        {
            return pw;
        }
        public static void setlocationname(String x)
        {
            locationname = x;
        }
        public static String getlocationname()
        {
            return locationname;
        }
        public static void setpm(String x)
        {
            pm = x;
        }
        public static String getpm()
        {
            return pm;
        }
    }
}
