using System;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using UnityEngine;

namespace Data
{
    [ExecuteInEditMode]
    public class PlatformData : MonoBehaviour, IPoolable<PlatformData>
    {
        [SerializeField] 
        private Vector3Int cellSize = Vector3Int.one * 2;

        [SerializeField]
        private int cellsInRow = 3;

        [field: SerializeField] 
        public PlatformType PlatformType { get; private set; }

        private Vector3 HalfSize => new Vector3(cellSize.x / 2f, cellSize.y / 2f, cellSize.z / 2f);
    
        public Vector3 size;
        public List<ObjectViewData> obstacles;
    
        public List<CustomBounds> emptyCells;
        public List<CellData> occupiedCells;
        
        public Action<PlatformData> OnObjectDestroyed { get; set; }

        [ContextMenu("CheckCells")]
        protected virtual void CheckCells()
        {
            emptyCells = new List<CustomBounds>();
            occupiedCells = new List<CellData>();
            var blocksCount = size.z / cellSize.z;
            for (var i = 0; i < blocksCount; i++)
            {
                for (var j = 0; j < cellsInRow; j++)
                {
                    var cell = new Vector3(j * cellSize.x, 0, i * cellSize.z);
                    if (!IsBlockOccupied(obstacles, cell))
                    {
                        emptyCells.Add(new CustomBounds(cell + HalfSize, cellSize));
                    }
                }
            }
        }
    
        protected virtual bool IsBlockOccupied(List<ObjectViewData> _obstacles, Vector3 _cell)
        {
            var blockBounds = new CustomBounds(_cell + HalfSize, cellSize);
            foreach (var obstacle in _obstacles)
            {
                var center = obstacle.transform.localPosition + obstacle.size / 2;
                var obstacleBound = new CustomBounds(center, obstacle.size);
                if (blockBounds.Intersects(obstacleBound))
                {
                    var cell = new Vector3Int(
                        (int) (_cell.x),
                        (int) (_cell.y),
                        (int) (_cell.z));
                    var newCell = new CellData(obstacleBound, obstacle, cell);
                    occupiedCells.Add(newCell);
                    return true;
                }
            }

            return false;
        }

        public void ReturnToPool()
        {
            OnObjectDestroyed?.Invoke(this);
        }
    }
}