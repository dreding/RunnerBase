using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using UnityEngine;

namespace Pool
{
    public class BonusPool : BaseObjectPool<BonusData, FieldBonusData, BonusType>
    {
        private const int POOL_SIZE = 5;
        
        public BonusPool(IEnumerable<BonusData> _items) : base(_items)
        {
            PoolObjects = new Dictionary<BonusType, Stack<FieldBonusData>>();
            
            foreach (var bonus in Items)
            {
                var pool = new Stack<FieldBonusData>(POOL_SIZE);
                for (var i = 0; i < POOL_SIZE; i++)
                {
                    var newBonus = CreateNewObject(bonus);      
                    pool.Push(newBonus);
                }
                PoolObjects.Add(bonus.type, pool);
            }
        }

        public override FieldBonusData GetPoolObject(BonusType _type)
        {
            if (PoolObjects[_type].Count > 0)
            { 
                return PoolObjects[_type].Pop();
            }

            var data = Items.First(_x => _x.type == _type);
            return CreateNewObject(data);
        }

        public override void ReturnObjectToPool(FieldBonusData _object)
        {
            _object.ObjectView.gameObject.SetActive(false);
            _object.ResetPosition();
            PoolObjects[_object.BonusType].Push(_object);
        }

        protected sealed override FieldBonusData CreateNewObject(BonusData _data)
        {
            var view = Object.Instantiate(_data.bonusView);
            var newBonus = new FieldBonusData(view, Vector3Int.zero, new CustomBounds(Vector3.zero, Vector3.zero),
                _data.type);
            newBonus.OnObjectDestroyed += ReturnObjectToPool;
            view.gameObject.SetActive(false);

            return newBonus;
        }
    }
}