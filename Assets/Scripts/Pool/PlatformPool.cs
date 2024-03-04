using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using UnityEngine;

namespace Pool
{
    public class PlatformPool : BaseObjectPool<PlatformData, PlatformData, PlatformType>
    {
        private const int POOL_SIZE = 1;
        
        public PlatformPool(IEnumerable<PlatformData> _items) : base(_items)
        {
            PoolObjects = new Dictionary<PlatformType, Stack<PlatformData>>();

            foreach (var data in Items)
            {
                var type = data.PlatformType;
                var pool = new Stack<PlatformData>(POOL_SIZE);
                for (var i = 0; i < POOL_SIZE; i++)
                {
                    var platform = CreateNewObject(data);
                    pool.Push(platform);
                }

                PoolObjects.Add(type, pool);
            }
        }

        public override PlatformData GetPoolObject(PlatformType _type)
        {
            if (PoolObjects[_type].Count > 0)
            {
                return PoolObjects[_type].Pop();
            }

            return CreateNewObject(Items.First(_x => _x.PlatformType == _type));
        }

        public override void ReturnObjectToPool(PlatformData _object)
        {
            _object.gameObject.SetActive(false);
            PoolObjects[_object.PlatformType].Push(_object);
        }

        protected sealed override PlatformData CreateNewObject(PlatformData _data)
        {
            var newPlatform = Object.Instantiate(_data);
            newPlatform.gameObject.SetActive(false);
            newPlatform.OnObjectDestroyed += ReturnObjectToPool;
            return newPlatform;
        }
    }
}