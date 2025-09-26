using Game.Services.Input;
using System;
using UnityEngine;

namespace Game
{
    public class DirectionMoveComponent : IDisposable
    {
        private float _moveSpeed;

        private readonly IInputService _inputService;
        private readonly CharacterController _characterController;

        private readonly Camera _camera;

        private Vector2 _horizontalLimit;
        private Vector2 _verticalLimit;

        public DirectionMoveComponent(
            IInputService inputService,
            CharacterController characterController,
            Camera camera,
            float moveSpeed)
        {
            _inputService = inputService;
            _characterController = characterController;
            _camera = camera;

            _moveSpeed = moveSpeed;

            _inputService.OnMove += OnMoveInput;

            GetLimitForPlayerMove();
        }

        public void Dispose()
        {
            _inputService.OnMove -= OnMoveInput;
        }

        private void OnMoveInput(Vector3 direction)
        {
            _characterController.Move(direction.normalized * _moveSpeed * Time.deltaTime);

            ConstrainToCameraBounds();
        }

        private void ConstrainToCameraBounds()
        {
            Vector3 pos = _characterController.transform.position;

            pos.x = Mathf.Clamp(pos.x, _horizontalLimit.x, _horizontalLimit.y);
            pos.z = Mathf.Clamp(pos.z, _verticalLimit.x, _verticalLimit.y);

            _characterController.transform.position = pos;
        }

        private void GetLimitForPlayerMove()
        {
            float camHalfHeight = _camera.orthographicSize;
            float camHalfWidth = camHalfHeight * _camera.aspect;

            Vector3 camPos = _camera.transform.position;

            float charRadius = _characterController.radius;

            float leftLimit = camPos.x - camHalfWidth + charRadius;
            float rightLimit = camPos.x + camHalfWidth - charRadius;
            float bottomLimit = camPos.z - camHalfHeight + charRadius;
            float topLimit = camPos.z + camHalfHeight - charRadius;

            _horizontalLimit = new Vector2(leftLimit, rightLimit);
            _verticalLimit = new Vector2(bottomLimit, topLimit);
        }
    }
}