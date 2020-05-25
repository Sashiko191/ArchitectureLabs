using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Layer.Interfaces
{
    interface IRepository<T>
    {
        List<T> GetAll();
        T GetT(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
