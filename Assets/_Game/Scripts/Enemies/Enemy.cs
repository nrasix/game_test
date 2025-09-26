using Game.Services.Character;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    public class Enemy : BaseEnemy
    {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _moveSpeed = 2;
        [SerializeField] private int _rotateSpeed = 10;
        [SerializeField] private float _minDistanceToTarget = 0.5f;

        [SerializeField] private CharacterController _characterContoller;

        private ITarget _targetObject;
        private MoveToTargetComponent _moveToTargetComponent;

        public void Init(ITarget targetObject)
        {
            _targetObject = targetObject;

            _moveToTargetComponent = new MoveToTargetComponent(targetObject, _characterContoller, _moveSpeed, _rotateSpeed);
        }

        private void Update()
        {
            _moveToTargetComponent.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform != _targetObject.Transform)
                return;

            if (!other.transform.TryGetComponent<IDamageble>(out var damageble))
                return;

            _moveToTargetComponent.OnTouchWithPlayer(true);
            damageble.GetDamage(_damageAmount);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform != _targetObject.Transform)
                return;

            _moveToTargetComponent.OnTouchWithPlayer(false);
        }
    }

    public class MoveToTargetComponent
    {
        private readonly ITarget _target;
        private CharacterController _characterContoller;

        private bool _isTouchingPlayer;
        private int _moveSpeed;
        private int _speedRotate;

        public MoveToTargetComponent(
            ITarget target,
            CharacterController characterContoller,
            int moveSpeed,
            int speedRotate)
        {
            _target = target;
            _characterContoller = characterContoller;

            _moveSpeed = moveSpeed;
            _speedRotate = speedRotate;
        }

        public void Update()
        {
            if (_isTouchingPlayer)
                return;

            if (_target == null)
                return;

            Vector3 direction = (_target.Transform.position - _characterContoller.transform.position).normalized;
            if (direction == Vector3.zero)
                return;

            Vector3 move = direction * _moveSpeed * Time.deltaTime;
            _characterContoller.Move(move);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _characterContoller.transform.rotation = 
                Quaternion.Slerp(
                    _characterContoller.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * _speedRotate
                );
        }

        public void OnTouchWithPlayer(bool value)
        {
            _isTouchingPlayer = value;
        }
    }
}