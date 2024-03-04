using System;
using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CustomBounds
    {
        [field: SerializeField, ReadOnlyField]
        public Vector3 Center { get; private set; }
        [field: SerializeField, ReadOnlyField]
        public Vector3 Extents { get; private set;}

        public CustomBounds(Vector3 _center, Vector3 _size)
        {
            Center = _center;
            Extents = _size * 0.5f;
        }

        public void ChangeCenter(Vector3 _newCenter)
        {
            Center = _newCenter;
        }

        public bool Intersects(CustomBounds _other)
        {
            if (math.abs(Center.x - _other.Center.x) - (Extents.x + _other.Extents.x) >= -0.001f) return false;
            if (math.abs(Center.y - _other.Center.y) - (Extents.y + _other.Extents.y) >= -0.001f) return false;
            if (math.abs(Center.z - _other.Center.z) - (Extents.z + _other.Extents.z) >= -0.001f) return false;

            // We have an overlap
            return true;
        }
    }
}
