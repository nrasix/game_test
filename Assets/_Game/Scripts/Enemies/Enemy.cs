using Game.Services.Character;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Game.Enemies
{
    public class Enemy : BaseEnemy
    {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _moveSpeed = 3;
        [SerializeField] private float _minDistanceToTarget = 0.5f;

        private ITarget _targetObject;
        private MoveToTargetComponent _moveToTargetComponent;

        public void Init(ITarget targetObject)
        {
            _targetObject = targetObject;

            _moveToTargetComponent = new MoveToTargetComponent(targetObject, _navMeshAgent, transform);
            _moveToTargetComponent.OnCollisionTarget += OnCollisionTarget;
        }

        private void OnDestroy()
        {
            _moveToTargetComponent.OnCollisionTarget -= OnCollisionTarget;
        }

        private void Update()
        {
            _moveToTargetComponent.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform != _targetObject)
                return;

            if(!other.transform.TryGetComponent<IDamageble>(out var damageble))
                return;

            Debug.Log("Player has trigger!");
            //damageble.GetDamage(_damageAmount);
        }

        private void OnCollisionTarget()
        {
            /*if (!_targetObject.Transform.TryGetComponent<IDamageble>(out var damageble))
                return;

            damageble.GetDamage(_damageAmount);*/
        }
    }

    public class MoveToTargetComponent
    {
        private Transform _currentTransform;
        private readonly ITarget _target;
        private NavMeshAgent _navMeshAgent;

        private bool _isTouchingPlayer;

        public event Action OnCollisionTarget;

        public MoveToTargetComponent(ITarget target, NavMeshAgent navMeshAgent, Transform currentTransform)
        {
            _target = target;
            _navMeshAgent = navMeshAgent;
            _currentTransform = currentTransform;
        }

        public void Update()
        {
            if (_isTouchingPlayer)
                return;

            if (_target != null)
            {
                _navMeshAgent.SetDestination(_target.Transform.position);

                /*if (Vector3.Distance(_currentTransform.position, _target.Transform.position) <= 0.25f)
                {
                    _isTouchingPlayer = true;
                    OnCollisionTarget?.Invoke();
                }*/
            }
        }
    }
}