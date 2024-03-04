using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Pool
{
    public abstract class BaseObjectPool<T1, T2, T3> where T2 : IPoolable<T2>
    {
        // List of items that can be used to create objects in the pool
        protected readonly List<T1> Items;
        // Dictionary to store pooled objects by their type
        protected Dictionary<T3, Stack<T2>> PoolObjects;

        // Constructor for the base object pool
        protected BaseObjectPool(IEnumerable<T1> _items)
        {
            // Initialize the list of items
            Items = _items.ToList();
        }

        // Method to get an object from the pool
        public abstract T2 GetPoolObject(T3 _type);

        // Method to return an object to the pool
        public abstract void ReturnObjectToPool(T2 _object);

        // Method to create a new object
        protected abstract T2 CreateNewObject(T1 _data);
    }
}