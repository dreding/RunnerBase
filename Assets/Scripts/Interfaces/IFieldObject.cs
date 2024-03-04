using Data;
using UnityEngine;

namespace Interfaces
{
    public interface IFieldObject
    {
        public ObjectViewData ObjectView { get; }
        public Vector3Int Cell { get; }
        public CustomBounds Bounds { get; }
    }
}