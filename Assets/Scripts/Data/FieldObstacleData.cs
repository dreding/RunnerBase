using Interfaces;
using UnityEngine;

namespace Data
{
    public class FieldObstacleData : IFieldObject
    {
        public ObjectViewData ObjectView { get; }
        public Vector3Int Cell { get; }
        public CustomBounds Bounds { get; }

        public FieldObstacleData(ObjectViewData _view, Vector3Int _cell, CustomBounds _bounds)
        {
            ObjectView = _view;
            Cell = _cell;
            Bounds = _bounds;
        }
    }
}