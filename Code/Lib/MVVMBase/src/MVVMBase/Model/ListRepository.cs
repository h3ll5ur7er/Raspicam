using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMBase.M
{
    public class ListRepository<T> : IRepository<T>
    {
        private readonly List<T> storage = new List<T>();
        public virtual IEnumerable<T> RequestAll() => storage.ToList();

        public virtual IEnumerable<T> Request(Func<T, bool> selector) => storage.Where(selector);

        public virtual void Update(T value, Func<T,T,bool> comparator = null)
        {
            if (comparator == null) comparator = (v1, v2) => v1.Equals(v2);
            var entry = storage.Single(x => comparator(x, value));
            entry = value;
        }
        public virtual void Create(T value) => storage.Add(value);

        public virtual void Delete(T value) => storage.Remove(value);
    }
}