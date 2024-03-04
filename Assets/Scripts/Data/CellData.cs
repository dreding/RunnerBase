using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CellData
    {
        [SerializeField, ReadOnlyField] private Vector3Int cell;
        [SerializeField, ReadOnlyField] private CustomBounds bounds;
        [SerializeField, ReadOnlyField] private ObjectViewData view;

        public Vector3Int Cell => cell;
        public CustomBounds Bounds => bounds;
        public ObjectViewData ObjectView => view;

        public CellData(CustomBounds _bounds, ObjectViewData _view, Vector3Int _cell)
        {
            cell = _cell;
            bounds = _bounds;
            view = _view;
        }
    }
}