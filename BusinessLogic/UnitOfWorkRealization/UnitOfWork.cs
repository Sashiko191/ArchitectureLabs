using BusinessLogic.Interfaces;
using DB_Layer.Interfaces;
using DB_Layer.Models;
using DB_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.UnitOfWorkRealization
{
    public class UnitOfWork : IUnitOfWork<AntiCafeDb>,IDisposable
    {
        public AntiCafeDb db { get; set; }
        public IGenericRepository<Order> ordersRepository { get; set; }
        public IGenericRepository<Room> roomsRepository { get; set; }
        public IGenericRepository<Equipment> equipmentRepository { get; set; }
        public IGenericRepository<Activity> activitiesRepository { get; set; }

        public UnitOfWork()
        {
            db = new AntiCafeDb();
            ordersRepository = new GenericRepository<Order>(db);
            roomsRepository = new GenericRepository<Room>(db);
            equipmentRepository = new GenericRepository<Equipment>(db);
            activitiesRepository = new GenericRepository<Activity>(db);
        }
        public void SaveDB()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
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
