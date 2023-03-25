using System;
using System.Collections.Generic;

namespace ElectronicCommerce.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        int Save();
    }
}
