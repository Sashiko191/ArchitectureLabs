using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.WebApiViewModels
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
