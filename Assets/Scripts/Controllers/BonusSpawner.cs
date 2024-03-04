using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using Pool;
using UnityEngine;

namespace Controllers
{
   // Responsible for spawning bonuses on platforms
    public class BonusSpawner
    {
        // Height at which bonuses should fly.
        // Also used as ray casting position height.
        private readonly float flyHeight;
        // Chance of spawning a bonus
        private readonly int bonusSpawnChance;
        // Total chance of all bonuses combined
        private readonly int totalBonusChance;
        // List of available bonuses
        private readonly List<BonusData> bonuses;
        // Object pool for bonuses
        private readonly BonusPool pool;

        // Constructor for BonusSpawner
        public BonusSpawner(int _bonusSpawnChance, float _flyHeight, IEnumerable<BonusData> _bonuses)
        {
            bonusSpawnChance = _bonusSpawnChance;
            bonuses = _bonuses.ToList();
            flyHeight = _flyHeight;
            pool = new BonusPool(bonuses);

            // Calculate the total bonus chance
            foreach (var bonusData in bonuses)
            {
                totalBonusChance += bonusData.bonusChance;
            }
        }

        // Spawns bonuses on the given platform
        public IEnumerable<FieldBonusData> SpawnBonuses(PlatformData _platform)
        {
            var spawnedBonuses = new List<FieldBonusData>();

            // Loop through each empty cell on the platform
            foreach (var emptyCell in _platform.emptyCells)
            {
                // Check if a bonus should be spawned based on chance
                if (Random.Range(0, 100) <= bonusSpawnChance)
                {
                    var bonusData = GetRandomBonusData();
                    if (bonusData != null)
                    {
                        // Get a bonus object from the pool and set its position
                        var newBonus = pool.GetPoolObject(bonusData.type);
                        var position = CalculateSpawnPosition(_platform, emptyCell.Center);
                        newBonus.SetPosition(position, position.VectorToCell(Vector3Int.one * 2));
                        newBonus.ObjectView.gameObject.SetActive(true);
                        spawnedBonuses.Add(newBonus);
                    }
                }
            }

            return spawnedBonuses;
        }

        // Get a random bonus data based on their chances
        private BonusData GetRandomBonusData()
        {
            if (totalBonusChance <= 0 || bonuses.Count == 0)
            {
                return null;
            }

            var roll = Random.Range(0, totalBonusChance);
            int cumulativeChance = 0;

            // Loop through each bonus and check if the roll falls within its chance range
            foreach (var bonus in bonuses)
            {
                cumulativeChance += bonus.bonusChance;
                if (roll < cumulativeChance)
                {
                    return bonus;
                }
            }

            return null;
        }

        // Calculate the spawn position of the bonus
        private Vector3 CalculateSpawnPosition(PlatformData _platform, Vector3 _center)
        {
            var position = _platform.transform.position + _center;
            position.y = flyHeight;

            // Raycast to find the ground position if the platform is above ground
            if (Physics.Raycast(position, Vector3.down, out var ray))
            {
                position.y = ray.point.y;
            }

            return position;
        }
    }
}