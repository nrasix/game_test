using Game.Weapons;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WeaponSpawnSettings
    {
        [field: SerializeField] public BaseWeapon _weapon;
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; }
    }
}