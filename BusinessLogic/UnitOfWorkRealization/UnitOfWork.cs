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
    public class UnitOfWork
    {
        private static UnitOfWork instance;
        public AntiCafeDb db;

        public IGenericRepository<Order> ordersRepository;
        public IGenericRepository<Room> roomsRepository;
        public IGenericRepository<Equipment> equipmentRepository;
        public IGenericRepository<Activity> activitiesRepository;

        private UnitOfWork()
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
        public static UnitOfWork GetUnitOfWork()
        {
            if (instance == null)
                instance = new UnitOfWork();
            return instance;
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
