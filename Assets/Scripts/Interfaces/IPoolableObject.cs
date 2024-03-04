using System;

namespace Interfaces
{
    public interface IPoolable<T>
    {
        public Action<T> OnObjectDestroyed { get; set; }
        public void ReturnToPool();
    }
}