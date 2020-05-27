using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IActivityService
    {
        List<ActivityDTO> GetAllActivities();
        ActivityDTO CreateNewActivity(ActivityDTO activityDTO, List<RoomDTO> roomDTOs);
    }
}
