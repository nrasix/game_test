using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EnemySpawnSettings
    {
        [field: SerializeField] public BaseEnemy Enemy { get; private set; }
        [field: SerializeField] public int CountSpawn { get; private set; }
    }
}