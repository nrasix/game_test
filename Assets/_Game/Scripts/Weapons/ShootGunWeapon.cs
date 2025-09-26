using Game.Weapons;
using UnityEngine;

namespace Game
{
    public sealed class ShootGunWeapon : BaseWeapon
    {
        [SerializeField] private Transform _spawnBullet;

        private ObjectPool<Bullet> _bullets;

        public override void Init(ObjectPool<Bullet> bullets)
        {
            _bullets = bullets;
        }

        public override void Shoot()
        {
            Debug.Log("Shoot from ShotGun Weapon!");
        }
    }
}