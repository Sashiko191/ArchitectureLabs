using DB_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ActivitiesRepository : IRepository<Activity>
    {
        private AntiCafeDb db;

        public ActivitiesRepository(AntiCafeDb context)
        {
            db = context;
        }
        public void Create(Activity item)
        {
            if (item != null)
            {
                db.Activitites.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var activity = db.Activitites.Where(a => a.Id == id).FirstOrDefault();
            if (activity != null)
            {
                db.Activitites.Remove(activity);
                db.SaveChanges();
            }
        }

        public List<Activity> GetAll()
        {
            return db.Activitites.Where(a => a.IsSpecialActivity == false).ToList();
        }

        public Activity GetT(int id)
        {
            return db.Activitites.Where(a => a.Id == id).FirstOrDefault();
        }

        public void Update(Activity item)
        {
            int itemId = item.Id;
            var oldActivity = db.Activitites.Where(a => a.Id == itemId).FirstOrDefault();
            if(oldActivity != null)
            {
                oldActivity.IsSpecialActivity = item.IsSpecialActivity;
                oldActivity.Name = item.Name;
                oldActivity.Description = item.Description;
                oldActivity.PricePerHour = item.PricePerHour;
                oldActivity.PosibleRooms = item.PosibleRooms;
                db.SaveChanges();
            }
        }
    }
}
