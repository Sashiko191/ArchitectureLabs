using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.WebApiViewModels
{
    public class OrderSearchViewModel
    {
        public DateTime DesiredDateTime { get; set; }

        public List<RoomViewModel> RoomViewModels { get; set; }
    }
}
