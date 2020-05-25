using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class RoomsRepository : IRepository<Room>
    {
        private AntiCafeDb db;
        public RoomsRepository(AntiCafeDb context)
        {
            db = context;
        }
        public void Create(Room item)
        {
            if(item != null)
            {
                db.Rooms.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var room = db.Rooms.Where(r => r.Id == id).FirstOrDefault();
            if(room != null)
            {
                db.Rooms.Remove(room);
                db.SaveChanges();
            }
        }

        public List<Room> GetAll()
        {
            return db.Rooms.ToList();
        }

        public Room GetT(int id)
        {
            return db.Rooms.Where(r => r.Id == id).FirstOrDefault();
        }

        public void Update(Room item)
        {
            var oldItem = db.Rooms.Where(r => r.Id == item.Id).FirstOrDefault();
            if(oldItem != null)
            {
                oldItem.Name = item.Name;
                oldItem.PosibleActivities = item.PosibleActivities;
                oldItem.RoomEquipment = item.RoomEquipment;
                oldItem.RoomOrders = item.RoomOrders;
                db.SaveChanges();
            }
        }
    }
}
