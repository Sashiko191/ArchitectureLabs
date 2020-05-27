using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Layer.Interfaces;
using DB_Layer.Models;

namespace DB_Layer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AntiCafeDb db;
        private DbSet<T> _dbSet;

        public GenericRepository(AntiCafeDb context)
        {
            db = context;
            _dbSet = context.Set<T>();
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
            db.SaveChanges();
        }

        public T FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Get()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            db.SaveChanges();
        }

        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
