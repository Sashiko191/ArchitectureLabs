using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.WebApiViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime StartDate { get; set; }

        public int Hours { get; set; }

        public List<RoomViewModel> OrderedRooms { get; set; }
    }
}
