using System;
using Game.Services.Character;
using Game.Services.Character.Data;
using Game.Services.Input;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, ITarget, IDamageble
    {
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private int _health = 5;

        private HealthHandler _healthHandler;
        private DirectionMoveComponent _directionMoveComponent;

        public Transform Transform => _characterController.transform;

        public void Init(IInputService inputService)
        {
            _directionMoveComponent = new(inputService, _characterController);

            _healthHandler = new(_health);
        }

        private void OnDestroy()
        {
            _directionMoveComponent.Dispose();
        }

        public void GetDamage(int damage)
        {
            Debug.Log("Player is get damage!");
            _healthHandler.SubjectHealth(_health);
        }
    }
}