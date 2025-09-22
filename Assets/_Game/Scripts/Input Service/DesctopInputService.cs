using Game.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Services.Input
{
    public sealed class DesktopInputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputSystem_Actions _inputActions;

        public event Action<Vector3> MovePerformed;

        public DesktopInputService()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Enable();
        }

        public void Initialize()
        {
            _inputActions.Player.Move.performed += OnMovePerformed;
        }

        public void Dispose()
        {
            _inputActions.Player.Move.performed -= OnMovePerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var inputVector = context.ReadValue<Vector2>();
            Vector3 moveVector = new Vector3(inputVector.X, 0f, inputVector.Y);

            MovePerformed?.Invoke(moveVector);
        }
    }
}