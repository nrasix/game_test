using Game.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Services.Input
{
    public sealed class InputService : IInputService, IInitializable, IDisposable, IFixedUpdatable
    {
        private readonly InputSystem_Actions _inputActions;

        public event Action<Vector3> MovePerformed;
        public event Action<Vector3> LookPerformed;

        public InputService()
        {
            _inputActions = new InputSystem_Actions();
        }

        public void Initialize()
        {
            _inputActions.Enable();

            _inputActions.Player.Attack.performed += OnAttack;
        }

        public void Dispose()
        {
            _inputActions.Player.Attack.performed -= OnAttack;

            _inputActions.Dispose();
        }

        public void FixedUpdate(float fixedDeltaTile)
        {
            var inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
            if (inputVector == Vector2.zero)
                return;

            Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);
            MovePerformed?.Invoke(moveVector);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            var mousePos2D = _inputActions.Player.Look.ReadValue<Vector2>();
        }
    }
}