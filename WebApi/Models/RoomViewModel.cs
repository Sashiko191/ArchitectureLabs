using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderViewModel> RoomOrders { get; set; }
        public List<ActivityViewModel> PosibleActivities { get; set; }
        public List<EquipmentViewModel> RoomEquipment { get; set; }
    }
}