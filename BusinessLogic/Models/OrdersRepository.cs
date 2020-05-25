using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class OrdersRepository : IRepository<Order>
    {
        private AntiCafeDb db;

        public OrdersRepository(AntiCafeDb context)
        {
            db = context;
        }
        public void Create(Order item)
        {
            if (item != null)
            {
                db.Orders.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var order = db.Orders.Where(o => o.Id == id).FirstOrDefault();
            if(order != null)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }

        public List<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        public Order GetT(int id)
        {
            return db.Orders.Where(o => o.Id == id).FirstOrDefault();
        }

        public void Update(Order item)
        {
            var oldOrder = db.Orders.Where(o => o.Id == item.Id).FirstOrDefault();
            if(oldOrder != null)
            {
                oldOrder.Name = item.Name;
                oldOrder.StartDate = item.StartDate;
                oldOrder.Hours = item.Hours;
                oldOrder.Surname = item.Surname;
                oldOrder.OrderedRooms = item.OrderedRooms;
                db.SaveChanges();
            }
        }
    }
}
