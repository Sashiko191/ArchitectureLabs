using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.WebApiViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public bool IsSpecialActivity { get; set; }

        public List<RoomViewModel> PosibleRooms { get; set; }
    }
}
