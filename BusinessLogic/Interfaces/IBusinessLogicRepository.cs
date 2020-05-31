using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IBusinessLogicRepository<T>
    {
        List<T> Get();
        List<T> GetWithEverything();
        T FindById(int id);
        T FindByIdWithEverything(int id);
        T Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
