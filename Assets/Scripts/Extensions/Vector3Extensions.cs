using UnityEngine;

namespace DefaultNamespace
{
    public static class Vector3Extensions
    {
        public static Vector3Int ToIntVector(this Vector3 _vector)
        {
            return new Vector3Int((int) _vector.x, (int) _vector.y, (int) _vector.z);
        }
        
        public static Vector3Int VectorToCell(this Vector3 _pos, Vector3Int _size)
        {
            return new Vector3Int(
                (int) (_pos.x - _pos.x % _size.x),
                (int) (_pos.y - _pos.y % _size.y),
                (int) (_pos.z - _pos.z % _size.z));
        }
    }
}