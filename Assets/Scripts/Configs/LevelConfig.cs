using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Config/NewLevel")]
    public class LevelConfig : ScriptableObject
    {
        public Vector3Int cellSize;
        public int maxBlocksInRowCount;
        public float flyHeight;
        [Range(0, 100)]
        public int bonusSpawnChance;
        public PlatformData startPlatform;
        public List<PlatformData> platforms;
        public List<BonusData> bonuses;
    }
}

