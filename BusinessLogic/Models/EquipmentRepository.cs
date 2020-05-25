using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class EquipmentRepository : IRepository<Equipment>
    {
        private AntiCafeDb db;

        public EquipmentRepository(AntiCafeDb context)
        {
            db = context;
        }
        public void Create(Equipment item)
        {
            if(item != null)
            {
                db.CafeEquipment.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var equipment = db.CafeEquipment.Where(e => e.Id == id).FirstOrDefault();
            if (equipment != null)
            {
                db.CafeEquipment.Remove(equipment);
                db.SaveChanges();
            }
        }

        public List<Equipment> GetAll()
        {
            return db.CafeEquipment.ToList();
        }

        public Equipment GetT(int id)
        {
            return db.CafeEquipment.Where(e => e.Id == id).FirstOrDefault();
        }

        public void Update(Equipment item)
        {
            var oldEquipment = db.CafeEquipment.Where(e => e.Id == item.Id).FirstOrDefault();
            if(oldEquipment != null)
            {
                oldEquipment.Name = item.Name;
                oldEquipment.Description = item.Description;
                oldEquipment.RoomsThatHaveIt = item.RoomsThatHaveIt;
                db.SaveChanges();
            }
        }
    }
}
