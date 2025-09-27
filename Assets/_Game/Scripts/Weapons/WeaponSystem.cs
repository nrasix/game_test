using Game.Services.Input;
using Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public sealed class WeaponSystem : IDisposable
    {
        private Transform _weaponSpawnPosition;
        private IInputService _inputService;

        private HashSet<BaseWeapon> _weapons;
        private ObjectPool<Bullet> _bulletPool;

        private GameObject _bulletPoolGO;

        private BaseWeapon _currentWeapon;
        private int _weaponIndex;

        public WeaponSystem(
            Transform weaponSpawnPosition,
            IInputService inputService,
            IReadOnlyList<WeaponSpawnSettings> _prefabsWeapon,
            Bullet bulletPrefab,
            int initializeSizePool)
        {
            _weapons = new(_prefabsWeapon.Count);

            _weaponSpawnPosition = weaponSpawnPosition;
            _inputService = inputService;

            _inputService.OnAttack += OnAttack;
            _inputService.OnSwitchWeapon += OnSwitchWeapon;

            _bulletPoolGO = new GameObject("Bullet Pool");

            _bulletPool = new(bulletPrefab, initializeSizePool, _bulletPoolGO.transform);

            SpawnWeapon(_prefabsWeapon);
        }

        private void SpawnWeapon(IReadOnlyList<WeaponSpawnSettings> _prefabsWeapon)
        {
            for (int i = 0, count = _prefabsWeapon.Count; i < count; i++)
            {
                var weapon = GameObject.Instantiate(_prefabsWeapon[i]._weapon, _weaponSpawnPosition);
                weapon.Init(_bulletPool);
                weapon.transform.localPosition = _prefabsWeapon[i].SpawnPosition;
                weapon.gameObject.SetActive(false);
                _weapons.Add(weapon);
            }

            SwitchWeapon(_weaponIndex);
        }

        private void SwitchWeapon(int indexWeapon)
        {
            if (_currentWeapon != null)
                _currentWeapon.gameObject.SetActive(false);

            var weapon = _weapons.ElementAt(indexWeapon);
            _currentWeapon = weapon;
            _currentWeapon.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            _inputService.OnAttack -= OnAttack;
            _inputService.OnSwitchWeapon -= OnSwitchWeapon;
            _inputService = null;

            _bulletPool.Dispose();

            GameObject.Destroy(_bulletPoolGO);
            _bulletPoolGO = null;

            for (int i = 0, count = _weapons.Count; i < count; i++)
            {
                GameObject.Destroy(_weapons.ElementAt(i).gameObject);
            }

            _weapons.Clear();
            _weapons = null;
        }

        private void OnAttack()
        {
            if (_currentWeapon.IsCanShoot)
                _currentWeapon.Shoot();
        }

        private void OnSwitchWeapon()
        {
            var newIndex = (_weaponIndex + 1) % _weapons.Count;
            _weaponIndex = newIndex;
            SwitchWeapon(_weaponIndex);
        }
    }
}