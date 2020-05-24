using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Models
{
    public class AntiCafeDb : DbContext
    {
        public AntiCafeDb() : base("Server=DESKTOP-I7HP3V8\\SQLEXPRESS;Database=AntiCafe;Trusted_Connection=True") { }
        static AntiCafeDb()
        {
            Database.SetInitializer<AntiCafeDb>(new AntiCafeDatabaseInitializer());
        }
        public DbSet<Activity> Activitites { get; set; }
        public DbSet<Equipment> CafeEquipment { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
