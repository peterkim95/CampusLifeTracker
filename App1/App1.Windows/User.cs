using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string profile_pic_url { get; set; }
        public double x_coord { get; set; }
        public double y_coord { get; set; }
        public bool is_at_location { get; set; }
        
        //public string location_name { get; set; }
        //public string location_type { get; set; }
        //public string location_description { get; set; }
        //public string checkin_time { get; set; }
        //public string personal_message { get; set; }
        //public bool ghost { get; set; }
    }
}
