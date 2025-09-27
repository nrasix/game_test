using Game.Services.Character;
using Game.Services.Character.Data;
using Game.Services.Input;
using Game.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ITarget, IDamageble
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _posSpawnWeapon;

        private CharacterController _characterController;

        private HealthHandler _healthHandler;
        private DirectionMoveComponent _directionMoveComponent;
        private TransformRotator _trasformRotator;
        private WeaponSystem _weaponSystem;

        public bool IsDead => _healthHandler.Health == 0;

        public Transform Transform => transform;

        public event Action OnLooseGame;

        public void Init(
            IInputService inputService,
            Camera mainCamera,
            IReadOnlyList<WeaponSpawnSettings> _prefabsWeapon,
            Bullet prefabBullet,
            int sizePoolBullet,
            HealthHandler healthHandler)
        {
            _characterController = GetComponent<CharacterController>();

            _directionMoveComponent = new(inputService, _characterController, mainCamera, _moveSpeed);
            _trasformRotator = new(transform, inputService);

            _weaponSystem = new(_posSpawnWeapon, inputService, _prefabsWeapon, prefabBullet, sizePoolBullet);

            _healthHandler = healthHandler;
        }

        private void OnDestroy()
        {
            _directionMoveComponent.Dispose();
            _trasformRotator.Dispose();
        }

        public void GetDamage(int damage)
        {
            if (IsDead)
                return;

            Debug.Log("Player is get damage!");
            _healthHandler.SubjectHealth(damage);

            if (IsDead)
                OnLooseGame?.Invoke();
        }
    }
}