using Game.Services.Input;
using Game.Utilities;
using System;
using UnityEngine;

namespace Assets._Game.Scripts.Weapons
{
    public sealed class WeaponSystem : IInitializable, IDisposable
    {
        private readonly Transform _weaponSpawnPosition;
        private readonly IInputService _inputService;

        public WeaponSystem(Transform weaponSpawnPosition, IInputService inputService)
        {
            _weaponSpawnPosition = weaponSpawnPosition;
            _inputService = inputService;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
    }
}