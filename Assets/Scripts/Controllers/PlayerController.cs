using Data;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers
{
    public class PlayerController
    {
        // Animation states
        private static readonly int Fly = Animator.StringToHash("Fly");
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Die = Animator.StringToHash("Lose");

        // Player size
        public Vector3 Size => Vector3.one;

        // Flag indicating if the player is flying
        public bool IsFly { get; private set; }

        // Bounds of the player
        public CustomBounds PlayerBounds { get; }

        // Current position of the player
        public Vector3 PlayerPosition => playerView.position;

        private readonly Animator animator; // Player animator component
        private readonly Transform playerView; // Player view transform

        // Target line and position for the player
        public int TargetLine { get; private set; }
        public float TargetX { get; private set; }

        // Constructor for the PlayerController
        public PlayerController(Transform _playerView, int _startLine, float _targetX)
        {
            playerView = _playerView;
            TargetLine = _startLine;
            TargetX = _targetX;
            animator = playerView.GetComponentInChildren<Animator>();

            // Initialize player bounds
            PlayerBounds = new CustomBounds(playerView.position, Size);
        }

        // Method to update player position
        public float UpdatePlayerPosition(Vector3 _newPos)
        {
            // Calculate the distance traveled by the player
            var dist = Mathf.Abs(_newPos.z - playerView.position.z);
            // Update player bounds
            PlayerBounds.ChangeCenter(_newPos);
            // Adjust player position
            _newPos.y += Size.y / 2;
            playerView.position = _newPos;

            return dist;
        }

        // Method to set the player's flying state
        public void FlyState(bool _isFly)
        {
            animator.SetBool(Fly, _isFly);
        }

        // Method to trigger player's death animation
        public void PlayerDie()
        {
            animator.SetTrigger(Die);
        }

        // Method to change the target line and position for the player
        public void ChangeLine(int _targetLine, float _targetX)
        {
            TargetLine = _targetLine;
            TargetX = _targetX;
        }

        // Method to set the player's movement state
        public void MoveState(bool _moveState)
        {
            animator.SetBool(Move, _moveState);
        }
    }
}
