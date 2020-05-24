using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class Order
    {
        public Order()
        {
            OrderedRooms = new List<Room>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime StartDate { get; set; }

        public int Hours { get; set; }

        public virtual ICollection<Room> OrderedRooms { get; set; }
    }
}
