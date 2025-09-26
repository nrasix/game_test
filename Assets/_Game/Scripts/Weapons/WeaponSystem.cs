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
        private readonly Transform _weaponSpawnPosition;
        private readonly IInputService _inputService;

        private readonly HashSet<BaseWeapon> _weapons;

        private BaseWeapon _currentWeapon;
        private int _weaponIndex;

        public WeaponSystem(
            Transform weaponSpawnPosition,
            IInputService inputService,
            IReadOnlyList<WeaponSpawnSettings> _prefabsWeapon)
        {
            _weapons = new(_prefabsWeapon.Count);

            _weaponSpawnPosition = weaponSpawnPosition;
            _inputService = inputService;

            _inputService.OnAttack += OnAttack;

            SpawnWeapon(_prefabsWeapon);
        }

        private void SpawnWeapon(IReadOnlyList<WeaponSpawnSettings> _prefabsWeapon)
        {
            for (int i = 0, count = _prefabsWeapon.Count; i < count; i++)
            {
                var weapon = GameObject.Instantiate(_prefabsWeapon[i]._weapon, _weaponSpawnPosition);
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
        }

        private void OnAttack()
        {
            _currentWeapon.Shoot();
        }
    }
}