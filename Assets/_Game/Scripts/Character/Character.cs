using System;
using Game.Services.Character;
using Game.Services.Input;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, ITarget
    {
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private float _moveSpeed = 5f;

        private IInputService _inputService;

        public Transform Transform => _characterController.transform;

        public void Init(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.MovePerformed += OnMoveInput;
        }

        private void OnDestroy()
        {
            _inputService.MovePerformed -= OnMoveInput;
        }

        private void OnMoveInput(Vector3 direction)
        {
            _characterController.Move(direction.normalized * _moveSpeed * Time.deltaTime);
        }
    }
}