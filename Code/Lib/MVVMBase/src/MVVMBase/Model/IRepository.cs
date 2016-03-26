using System;
using System.Collections.Generic;

namespace MVVMBase.M
{
    public interface IRepository<T>
    {
        IEnumerable<T> RequestAll();
        IEnumerable<T> Request(Func<T,bool> selector);

        void Create(T value);
        void Update(T value, Func<T, T, bool> comparator);
        void Delete(T value);
    }
}