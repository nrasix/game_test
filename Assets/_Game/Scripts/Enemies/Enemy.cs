using Game.Services.Character;
using Game.Services.Character.Data;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : BaseEnemy, IDamageble
    {
        [SerializeField] private CharacterController _characterContoller;

        [Space(5)]
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _moveSpeed = 2;
        [SerializeField] private int _rotateSpeed = 10;

        private ITarget _targetObject;
        private MoveToTargetComponent _moveToTargetComponent;
        private HealthHandler _healthHandler;

        public override event Action<BaseEnemy> OnEnemyDied;

        public bool IsDead { get; private set; }

        public override void Init(ITarget targetObject)
        {
            _targetObject = targetObject;

            _healthHandler = new HealthHandler(_healthAmount, _healthAmount);

            _moveToTargetComponent = new MoveToTargetComponent
                (targetObject, _characterContoller, _moveSpeed, _rotateSpeed);
        }

        public override void ResetEnemy()
        {
            IsDead = false;
            _healthHandler.AddHealth(_healthAmount);
            _moveToTargetComponent.SetStateMove(true);

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (IsDead || _moveToTargetComponent == null)
                return;

            _moveToTargetComponent.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform != _targetObject.Transform)
                return;

            if (!other.transform.TryGetComponent<IDamageble>(out var damageble))
                return;

            _moveToTargetComponent.SetStateMove(false);
            damageble.GetDamage(_damageAmount);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform != _targetObject.Transform)
                return;

            _moveToTargetComponent.SetStateMove(true);
        }

        public void GetDamage(int damage)
        {
            if (IsDead)
                return;

            _healthHandler.SubjectHealth(damage);

            if (_healthHandler.Health == 0)
            {
                IsDead = true;
                OnEnemyDied?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}