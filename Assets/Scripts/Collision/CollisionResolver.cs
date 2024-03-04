using System;
using Data;
using Interfaces;

namespace Collision
{
    public class CollisionResolver
    {
        // Event triggered when a bonus is collided
        public Action<FieldBonusData> OnBonusCollided;
        // Event triggered when the player dies
        public Action OnPlayerDie;

        // Resolve collision between player and field object
        public void ResolveCollision(CustomBounds _player, IFieldObject _object)
        {
            // Check the type of the collided object
            switch (_object)
            {
                case FieldBonusData bonus:
                    // Invoke the event for bonus collision
                    OnBonusCollided?.Invoke(bonus);
                    // Return the bonus to the pool
                    bonus.ReturnToPool();
                    break;
                case FieldObstacleData:
                    // Check if the player is below the obstacle
                    if (_player.Center.y < _object.Bounds.Center.y + _object.Bounds.Extents.y)
                    {
                        // Invoke the event for player death
                        OnPlayerDie?.Invoke();
                    }
                    break;
            }
        }
    }
}