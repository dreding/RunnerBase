using Controllers;
using Unity.Mathematics;
using UnityEngine;

namespace Movers
{
  public class BasePlayerMover
  {
    // Direction in which the player moves
    protected virtual Vector3 ForwardDirection => Vector3.forward;
    // Smoothing factor for player movement
    protected virtual float SmoothSpeed => 0.125f;
    // Speed of player movement
    protected virtual float Speed => 10f;
    // Speed of side movement
    protected virtual float SideSpeed => 0.125f;

    // Move the player
    public virtual bool MovePlayer(PlayerController _player, float _deltaTime, out Vector3 _newPos)
    {
      // Calculate the desired position based on current position, direction, and speed
      var desiredPosition = _player.PlayerPosition + ForwardDirection * Speed * _deltaTime;
      // Smoothly move the player towards the desired position
      _newPos = Vector3.Lerp(_player.PlayerPosition, desiredPosition, SmoothSpeed);
      // Adjust the player's position vertically based on ground height
      _newPos.y = PointHeight(_newPos, _player.Size, _deltaTime) + _player.Size.y / 2f;
      // Adjust the player's position horizontally towards the target X position
      _newPos.x = Mathf.Lerp(_player.PlayerPosition.x, _player.TargetX, SideSpeed);

      // Check if the new position is valid
      return !float.IsNaN(_newPos.y);
    }

    // Calculate the height of the point above the ground
    protected virtual float PointHeight(Vector3 _point, Vector2 _size, float _deltaTime)
    { 
      // Move the point to the top of the player
      _point.y += _size.y;
      _point.x += _size.x / 2;
        
      // Perform a raycast downwards to find the ground height
      if (Physics.Raycast(_point, Vector3.down, out var ray))
      {
        return ray.point.y;
      }

      // Return NaN if no ground is found
      return float.NaN;
    }
  }
}
