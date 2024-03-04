using System.Collections.Generic;
using Data;
using DefaultNamespace;
using Interfaces;
using UnityEngine;

namespace Collision
{
    public class CollisionChecker
    {
        // Dictionary to store field objects with their positions
        private readonly IDictionary<Vector3Int, IFieldObject>
            fieldObjects = new Dictionary<Vector3Int, IFieldObject>();

        // Size of each cell in the grid
        private Vector3Int cellSize;

        // Constructor for the collision checker
        public CollisionChecker(Vector3Int _cellSize)
        {
            cellSize = _cellSize;
        }

        // Add new platform objects to the collision checker
        public void AddNewPlatform(PlatformData _platform)
        {
            var platformCell = _platform.transform.position.VectorToCell(cellSize);
            foreach (var cellData in _platform.occupiedCells)
            {
                var worldCell = platformCell + cellData.Cell;
                var worldBounds = new CustomBounds(platformCell + cellData.Bounds.Center, cellData.ObjectView.size);
                var newFieldObstacle = new FieldObstacleData(cellData.ObjectView, worldCell, worldBounds);
                fieldObjects.Add(worldCell, newFieldObstacle);
            }
        }

        // Add objects to the collision checker
        public void AddObjects(List<IFieldObject> _objects)
        {
            foreach (var newObject in _objects)
            {
                fieldObjects.Add(newObject.Cell, newObject);
            }
        }

        // Remove an object from the collision checker
        public void RemoveObject(IFieldObject _fieldObject)
        {
            fieldObjects.Remove(_fieldObject.Cell);
        }

        // Remove platform objects from the collision checker
        public void RemovePlatform(PlatformData _platform)
        {
            foreach (var cellData in _platform.occupiedCells)
            {
                var worldCell = _platform.transform.position.ToIntVector() + cellData.Cell;
                fieldObjects.Remove(worldCell);
            }
        }

        // Check if the player is collided with any field object
        public bool IsPlayerCollided(CustomBounds _player, out IFieldObject _target)
        {
            _target = null;
            // Get the cell where the player is currently located
            var cell = _player.Center.VectorToCell(cellSize);
            // Get the cell where the player is about to move
            var nextCell = new Vector3Int(cell.x, cell.y, cell.z + cellSize.z);
            // List of objects to check for collision
            var objectsToCheck = new List<IFieldObject>()
            {
                fieldObjects.TryGetValue(cell, out var value) ? value : null,
                fieldObjects.TryGetValue(nextCell, out var nextValue) ? nextValue : null
            };

            // Check for intersection between player bounds and field object bounds
            foreach (var obj in objectsToCheck)
            {
                if (obj != null)
                {
                    if (_player.Intersects(obj.Bounds))
                    {
                        _target = obj;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}