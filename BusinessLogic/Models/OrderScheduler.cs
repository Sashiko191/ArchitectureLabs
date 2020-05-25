using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class OrderScheduler
    {
        private UnitOfWork unitOfWork;
        public OrderScheduler(UnitOfWork u)
        {
            unitOfWork = u;
        }
        public List<Order> GetActiveOrders(DateTime desiredDate, Room room)
        {
            List<Order> AllOrders = unitOfWork.db.Orders.Where(o => DbFunctions.TruncateTime(o.StartDate) == desiredDate.Date).ToList();

            for (int i = 0; i < AllOrders.Count;i++)
            {
                if (!AllOrders[i].OrderedRooms.Contains(room))
                {
                    AllOrders.RemoveAt(i);
                    i = i - 1;
                    if (i < 0)
                        i = 0;
                }
            }

            return AllOrders;
        }  
    }
}
