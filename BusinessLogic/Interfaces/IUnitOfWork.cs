using DB_Layer.Interfaces;
using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUnitOfWork<T>
    {
        IGenericRepository<Order> ordersRepository { get; set; }
        IGenericRepository<Room> roomsRepository { get; set; }
        IGenericRepository<Equipment> equipmentRepository { get; set; }
        IGenericRepository<Activity> activitiesRepository { get; set; }

        T db { get; set; }
        void SaveDB();
    }
}
