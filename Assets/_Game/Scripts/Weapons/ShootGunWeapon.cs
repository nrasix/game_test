using Game.Weapons;
using UnityEngine;

namespace Game
{
    public sealed class ShootGunWeapon : BaseWeapon
    {
        [SerializeField] private Transform _spawnBullet;

        public override void Shoot()
        {
            Debug.Log("Shoot from ShotGun Weapon!");
        }
    }
}