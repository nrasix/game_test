using Game.Services.Input;
using System;
using UnityEngine;

namespace Game
{
    public class DirectionMoveComponent : IDisposable
    {
        private float _moveSpeed = 5f;

        private readonly IInputService _inputService;
        private readonly Transform _characterTransform;

        private readonly Camera _camera;

        private float _radiusCollider;

        private Vector2 _horizontalLimit;
        private Vector2 _verticalLimit;

        public DirectionMoveComponent(IInputService inputService, Transform characterTransform, Camera camera, float radiusCollider)
        {
            _inputService = inputService;
            _characterTransform = characterTransform;
            _camera = camera;

            _radiusCollider = radiusCollider;

            _inputService.OnMove += OnMoveInput;

            GetLimitForPlayerMove();
        }

        public void Dispose()
        {
            _inputService.OnMove -= OnMoveInput;
        }

        private void OnMoveInput(Vector3 direction)
        {
            _characterTransform.position += direction.normalized * _moveSpeed * Time.deltaTime;

            ConstrainToCameraBounds();
        }

        private void ConstrainToCameraBounds()
        {
            Vector3 pos = _characterTransform.position;

            pos.x = Mathf.Clamp(pos.x, _horizontalLimit.x, _horizontalLimit.y);
            pos.z = Mathf.Clamp(pos.z, _verticalLimit.x, _verticalLimit.y);

            _characterTransform.position = pos;
        }

        private void GetLimitForPlayerMove()
        {
            float camHalfHeight = _camera.orthographicSize;
            float camHalfWidth = camHalfHeight * _camera.aspect;

            Vector3 camPos = _camera.transform.position;

            float charRadius = _radiusCollider;

            float leftLimit = camPos.x - camHalfWidth + charRadius;
            float rightLimit = camPos.x + camHalfWidth - charRadius;
            float bottomLimit = camPos.z - camHalfHeight + charRadius;
            float topLimit = camPos.z + camHalfHeight - charRadius;

            _horizontalLimit = new Vector2(leftLimit, rightLimit);
            _verticalLimit = new Vector2(bottomLimit, topLimit);
        }
    }
}