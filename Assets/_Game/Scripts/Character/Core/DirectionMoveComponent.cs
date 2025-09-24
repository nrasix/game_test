using Game.Services.Input;
using System;
using UnityEngine;

namespace Game
{
    public class DirectionMoveComponent : IDisposable
    {
        private float _moveSpeed = 5f;

        private IInputService _inputService;
        private CharacterController _characterController;

        public DirectionMoveComponent(IInputService inputService, CharacterController characterController)
        {
            _inputService = inputService;
            _characterController = characterController;

            _inputService.OnMove += OnMoveInput;
        }

        public void Dispose()
        {
            _inputService.OnMove -= OnMoveInput;
        }

        private void OnMoveInput(Vector3 direction)
        {
            _characterController.Move(direction.normalized * _moveSpeed * Time.deltaTime);
        }
    }
}