using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class Equipment
    {
        public Equipment()
        {
            RoomsThatHaveIt = new List<Room>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Room> RoomsThatHaveIt { get; set; }
    }
}
