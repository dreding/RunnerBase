using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Data;
using DefaultNamespace;
using Pool;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class RoadController
    {
        private readonly List<PlatformData> createdPlatforms; // List to keep track of created platforms
        private readonly PlatformPool pool; // Pool for platform objects
        private float nextPlatformPos; // Position for the next platform to be created
        
        // Event triggered when a platform is created
        public Action<PlatformData> OnPlatformCreated;

        // Constructor for the RoadController
        public RoadController(LevelConfig _level)
        {
            // Initialize the platform pool
            pool = new PlatformPool(_level.platforms);

            // Initialize the list of created platforms
            createdPlatforms = new List<PlatformData>();

            // Instantiate the start platform and add it to the list of created platforms
            var newPlatform = Object.Instantiate(_level.startPlatform);
            createdPlatforms.Add(newPlatform);
            nextPlatformPos = newPlatform.transform.position.z; // Set the position for the next platform
        }

        // Method to handle changes in player position
        public void PlayerPositionChanged(Vector3 _position)
        {
            var platformToCheck = createdPlatforms[0]; // Get the platform at the front of the list
            // Check if the player has passed the platform
            if (_position.z > platformToCheck.transform.position.z + platformToCheck.size.z + 4)
            {
                // Remove the platform from the list and return it to the pool
                createdPlatforms.RemoveAt(0);
                platformToCheck.ReturnToPool();
            }

            // Check if it's time to create the next platform
            if (_position.z >= nextPlatformPos)
            {
                CreatePlatform(); // Create a new platform
            }
        }

        // Method to create a new platform
        private void CreatePlatform()
        {
            var prevPlatform = createdPlatforms.Last(); // Get the last platform in the list
            // Get a new platform object from the pool
            var newPlatform =
                pool.GetPoolObject((PlatformType)Random.Range(0, Enum.GetValues(typeof(PlatformType)).Length));
            createdPlatforms.Add(newPlatform); // Add the new platform to the list
            newPlatform.gameObject.SetActive(true); // Activate the platform object
            // Set the position for the new platform
            newPlatform.transform.position = new Vector3(0, 0, prevPlatform.transform.position.z + prevPlatform.size.z);
            // Set the position for the next platform to be created
            nextPlatformPos = newPlatform.transform.position.z + newPlatform.size.z / 3f;

            // Trigger the event for platform creation
            OnPlatformCreated?.Invoke(newPlatform);
        }
    }
}


