using Game.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Services.Input
{
    public sealed class InputService : IInputService, IInitializable, IDisposable, IFixedUpdatable
    {
        private readonly InputSystem_Actions _inputActions;
        private readonly Camera _mainCamera;

        private LayerMask _groundLayer;

        public event Action<Vector3> OnMove;
        public event Action<Vector3> OnTouchScreen;
        public event Action OnAttack;
        public event Action OnSwitchWeapon;

        public InputService(Camera camera, LayerMask groundLayer)
        {
            _inputActions = new InputSystem_Actions();

            _groundLayer = groundLayer;
            _mainCamera = camera;
        }

        public void Initialize()
        {
            _inputActions.Enable();

            _inputActions.Player.Attack.performed += OnAttackCallback;
            _inputActions.Player.SwitchWeapon.performed += SwitchWeapon;
        }

        public void Dispose()
        {
            _inputActions.Player.Attack.performed -= OnAttackCallback;
            _inputActions.Player.SwitchWeapon.performed -= SwitchWeapon;

            _inputActions.Dispose();
        }

        public void FixedUpdate(float fixedDeltaTile)
        {
            var inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
            if (inputVector == Vector2.zero)
                return;

            Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);
            OnMove?.Invoke(moveVector);
        }

        private void OnAttackCallback(InputAction.CallbackContext context)
        {
            var mousePos2D = _inputActions.Player.Point.ReadValue<Vector2>();

            Ray ray = _mainCamera.ScreenPointToRay(mousePos2D);

            if (Physics.Raycast(ray, out var hitPoint, Mathf.Infinity, _groundLayer))
            {
                Vector3 worldPoint = hitPoint.point;
                OnTouchScreen?.Invoke(worldPoint);
            }

            OnAttack?.Invoke();
        }

        private void SwitchWeapon(InputAction.CallbackContext value) => OnSwitchWeapon?.Invoke();

        public void SetGameInput(bool value)
        {
            if (value)
                _inputActions.Player.Enable();
            else
                _inputActions.Player.Disable();
        }
    }
}