using Game.Services.Input;
using System;
using UnityEngine;

namespace Game
{
    public class TransformRotator : IDisposable
    {
        private readonly Transform _targetTransform;
        private readonly IInputService _inputService;
        private float _rotationSpeed = 5f;

        public TransformRotator(Transform targetTransform, IInputService inputService)
        {
            _targetTransform = targetTransform;
            _inputService = inputService;

            _inputService.LookPerformed += OnLookPerformed;
        }

        public void Dispose()
        {
            _inputService.LookPerformed -= OnLookPerformed;
        }

        private void OnLookPerformed(Vector3 vector)
        {

        }
    }
}