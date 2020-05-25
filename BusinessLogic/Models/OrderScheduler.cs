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
        public bool HasIntersection(DateTime DesiredDate, int Hours, List<Room> rooms)
        {
            DateTime StartDateTime = DesiredDate;
            DateTime EndDateTime = DesiredDate.AddHours(Hours);

            if (StartDateTime.Hour > 8 && EndDateTime.Hour > StartDateTime.Hour && EndDateTime.Hour < 24)
            {

                for (int i = 0; i < rooms.Count; i++)
                {
                    List<Order> AllOrdersForRoom = GetActiveOrders(DesiredDate, rooms[i]);
                    for (int j = 0; j < AllOrdersForRoom.Count; j++)
                    {
                        DateTime OrderStartDateTime = AllOrdersForRoom[j].StartDate;
                        DateTime OrderEndDateTime = AllOrdersForRoom[j].StartDate.AddHours(AllOrdersForRoom[j].Hours);

                        if (StartDateTime < OrderStartDateTime)
                        {
                            if (EndDateTime <= OrderStartDateTime)
                            {

                            }
                            else
                                return true;
                        }
                        else if (StartDateTime >= OrderEndDateTime)
                        {
                            if (EndDateTime > OrderEndDateTime)
                            {

                            }
                            else
                                return true;
                        }
                        else
                            return true;

                    }
                }
            }
            else
                return true;

            return false;
        }
    }
}
