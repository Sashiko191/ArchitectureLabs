using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class OrderSearchViewModel
    {
        public DateTime DesiredDateTime { get; set; }

        public List<RoomViewModel> RoomViewModels { get; set; }
    }
}