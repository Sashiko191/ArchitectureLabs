using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class Activity
    {
        public Activity()
        {
            PosibleRooms = new List<Room>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public bool IsSpecialActivity { get; set; }

        public virtual ICollection<Room> PosibleRooms { get; set; }
    }
}
