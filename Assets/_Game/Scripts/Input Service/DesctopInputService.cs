using Game.Utilities;
using System;
using System.Numerics;
using UnityEngine.InputSystem;

namespace Game.Services.Input
{
    public sealed class DesctopInputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputSystem_Actions _inputActions;

        public event Action<Vector3> MovePerformed;

        public DesctopInputService()
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
            var result = context.ReadValue<Vector2>();
            MovePerformed?.Invoke(new Vector3(result.x, 0, result.y));
        }
    }
}