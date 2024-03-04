using System;
using DefaultNamespace;
using Interfaces;
using UnityEngine;

namespace Data
{
    public class FieldBonusData : IFieldObject, IPoolable<FieldBonusData>
    {
        public ObjectViewData ObjectView { get; }
        public Vector3Int Cell { get; private set; }
        public CustomBounds Bounds { get; private set; }
        public BonusType BonusType { get; }
        
        public Action<FieldBonusData> OnObjectDestroyed { get; set; }
        
        public FieldBonusData(ObjectViewData _view, Vector3Int _cell, CustomBounds _bounds, BonusType _type)
        {
            ObjectView = _view;
            Cell = _cell;
            Bounds = _bounds;
            BonusType = _type;
        }

        public void SetPosition(Vector3 _position, Vector3Int _cell)
        {
            ObjectView.transform.position = _position;
            Cell = _cell;
            Bounds = new CustomBounds(_position, ObjectView.size);
        }

        public void ResetPosition()
        {
            Cell = Vector3Int.zero;
            Bounds = new CustomBounds(Vector3.zero, Vector3.zero);
            ObjectView.transform.position = Vector3.zero;
        }

        public void ReturnToPool()
        {
            OnObjectDestroyed?.Invoke(this);
        }
    }
}