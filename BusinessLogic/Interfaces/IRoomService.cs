using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRoomService
    {
        List<RoomDTO> GetAllRooms();
        RoomDTO GetRoom(int id);
    }
}
