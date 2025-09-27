using Game.Weapons;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public sealed class ShootGunWeapon : BaseWeapon
    {
        [SerializeField] private Transform _spawnBullet;

        [SerializeField] private int _countBulletShoot = 4;
        [SerializeField] private float _spreadAngleShoot = 15f;

        [Space(5)]
        [SerializeField] private float _timeToCooldownShoot = 2f;

        private ObjectPool<Bullet> _bullets;

        public override void Init(ObjectPool<Bullet> bullets)
        {
            _bullets = bullets;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public override void Shoot()
        {
            IsCanShoot = false;

            for (int i = 0; i < _countBulletShoot; i++)
            {
                var bullet = _bullets.Get(true);
                bullet.transform.position = _spawnBullet.position;
                Vector3 direction = CalculatePelletDirection(i);
                bullet.Init(direction);
            }

            CooldownShoot();
        }

        private Vector3 CalculatePelletDirection(int index)
        {
            if (_countBulletShoot <= 1)
                return _spawnBullet.forward;

            float angleStep = _spreadAngleShoot * 2f / (_countBulletShoot - 1);
            float currentAngle = -_spreadAngleShoot + angleStep * index;

            Quaternion rotation = Quaternion.AngleAxis(currentAngle, _spawnBullet.right);
            return rotation * _spawnBullet.forward;
        }

        private async void CooldownShoot()
        {
            await Task.Delay((int)(_timeToCooldownShoot * 1000), destroyCancellationToken);

            IsCanShoot = true;
        }
    }
}