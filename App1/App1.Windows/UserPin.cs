using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Text;
using System.Threading.Tasks;
using Bing.Maps;

namespace App1
{
    [Table("UserPins")]
    public class UserPin
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int user_id { get; set; }
        public double x_coord { get; set; }
        public double y_coord { get; set; }
        public int ampm { get; set; }
        public string location_name { get; set; }
        public int location_type { get; set; }
        public int checkin_hour { get; set; }
        public int checkin_minute { get; set; }
        public string personal_message { get; set; }
        public bool ghost { get; set; }
        public bool view { get; set; }
        public string pp { get; set; }
    }
}
