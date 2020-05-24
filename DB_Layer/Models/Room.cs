using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class Room
    {
        public Room()
        {
            RoomOrders = new List<Order>();
            PosibleActivities = new List<Activity>();
            RoomEquipment = new List<Equipment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> RoomOrders { get; set; }
        public virtual ICollection<Activity> PosibleActivities { get; set; }
        public virtual ICollection<Equipment> RoomEquipment { get; set; }
    }
}
