using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class UnitOfWork : IDisposable
    {
        public AntiCafeDb db;
        public OrdersRepository ordersRepository;
        public RoomsRepository roomsRepository;
        public EquipmentRepository equipmentRepository;
        public ActivitiesRepository activitiesRepository;

        public UnitOfWork()
        {
            db = new AntiCafeDb();
            ordersRepository = new OrdersRepository(db);
            roomsRepository = new RoomsRepository(db);
            equipmentRepository = new EquipmentRepository(db);
            activitiesRepository = new ActivitiesRepository(db);
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
