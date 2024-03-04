using UnityEngine;

namespace Controllers
{
    public class CameraFollow : MonoBehaviour
    {
        // Smoothing factor for camera movement
        [SerializeField] private float smoothSpeed = 0.125f;
        // Offset between camera and target
        [SerializeField] private Vector3 offset;
        // Target to follow
        private Transform target;

        // Method to initialize camera follow with a target
        public void InitCameraFollow(Transform _target)
        {
            target = _target;
        }

        // LateUpdate is called after Update each frame
        void LateUpdate ()
        {
            // Check if target exists
            if (!target)
            {
                return; // If target doesn't exist, exit the method
            }
        
            // Calculate the desired position for the camera
            var desiredPosition = target.position + offset;
            // Smoothly move the camera towards the desired position
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}
