using Game.Services.Input;
using System;
using UnityEngine;

namespace Game
{
    public class TransformRotator : IDisposable
    {
        private readonly Transform _targetTransform;
        private readonly IInputService _inputService;

        public TransformRotator(Transform targetTransform, IInputService inputService)
        {
            _targetTransform = targetTransform;
            _inputService = inputService;

            _inputService.OnTouchScreen += OnLookPerformed;
        }

        public void Dispose()
        {
            _inputService.OnTouchScreen -= OnLookPerformed;
        }

        private void OnLookPerformed(Vector3 vector)
        {
            Vector3 direction = new Vector3(
                    vector.x - _targetTransform.position.x,
                    0,
                    vector.z - _targetTransform.position.z
                );

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _targetTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            }
        }
    }
}