using Game.Services.Character;
using UnityEngine;

namespace Game.Enemies
{
    public class MoveToTargetComponent
    {
        private readonly ITarget _target;
        private readonly CharacterController _characterContoller;
        private readonly Rigidbody _rigidbody;

        private bool _isTouchingPlayer;
        private int _moveSpeed;
        private int _speedRotate;

        private Transform TargetTransform => _target.Transform;
        private Transform CurrentTransform => _rigidbody.transform;

        public MoveToTargetComponent(
            ITarget target,
            CharacterController characterContoller,
            int moveSpeed,
            int speedRotate,
            Rigidbody rigidbody)
        {
            _target = target;
            _characterContoller = characterContoller;
            _rigidbody = rigidbody;

            _moveSpeed = moveSpeed;
            _speedRotate = speedRotate;
        }

        public void Update()
        {
            if (_isTouchingPlayer)
                return;

            if (_target == null)
                return;

            Vector3 direction = (TargetTransform.position - CurrentTransform.position).normalized;
            if (direction == Vector3.zero)
                return;

            Vector3 move = direction * _moveSpeed * Time.deltaTime;
            _characterContoller.Move(move);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            CurrentTransform.rotation =
                Quaternion.Slerp(
                    CurrentTransform.rotation,
                    targetRotation,
                    Time.deltaTime * _speedRotate
                );
        }

        public void FixedUpdate()
        {
            if (_isTouchingPlayer)
                return;

            if (_target == null)
                return;

            Vector3 direction = (TargetTransform.position - CurrentTransform.position).normalized;
            if (direction == Vector3.zero)
                return;

            //Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //_rigidbody.linearVelocity = direction * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.Move(
                direction * _moveSpeed,
                Quaternion.Slerp(
                    CurrentTransform.rotation,
                    targetRotation,
                    Time.deltaTime * _speedRotate
                )
            );

            /*Quaternion targetRotation = Quaternion.LookRotation(direction);
            CurrentTransform.rotation =
                Quaternion.Slerp(
                    CurrentTransform.rotation,
                    targetRotation,
                    Time.deltaTime * _speedRotate
                );*/
        }

        public void OnTouchWithPlayer(bool value)
        {
            _isTouchingPlayer = value;
        }
    }
}