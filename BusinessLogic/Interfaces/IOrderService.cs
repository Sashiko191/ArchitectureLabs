using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        List<OrderDTO> GetActiveOrders(DateTime DesiredDateTime, RoomDTO room);
        void CreateNewOrder(OrderDTO orderDTO, List<RoomDTO> orderedRooms);
        bool HasIntersection(DateTime DesiredDate, int Hours, List<RoomDTO> rooms);
    }
}
