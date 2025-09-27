using Game.Services.Character;
using Game.Services.Character.Data;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class BombEnemy : BaseEnemy, IDamageble
    {
        [SerializeField] private CharacterController _characterContoller;

        [Space(5)]
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _moveSpeed = 4;
        [SerializeField] private int _rotateSpeed = 10;

        [Space(5)]
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private float _detonationDistance = 1.5f;
        [SerializeField] private float _detonationDelay = 0.5f;

        private bool _isDetonating = false;
        private float _detonationTimer = 0f;

        private Collider[] _colliderCashe = new Collider[64];

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

            _isDetonating = false;
            _detonationTimer = 0f;

            _healthHandler.AddHealth(_healthAmount);
            _moveToTargetComponent.SetStateMove(true);

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (IsDead || _moveToTargetComponent == null)
                return;

            if (_isDetonating)
            {
                _detonationTimer += Time.deltaTime;
                if (_detonationTimer >= _detonationDelay)
                    Explode();

                return;
            }

            _moveToTargetComponent.Update();

            float distanceToTarget = Vector3.Distance(transform.position, _targetObject.Transform.position);
            if (distanceToTarget <= _detonationDistance)
            {
                StartDetonation();
            }
        }

        private void StartDetonation()
        {
            _isDetonating = true;
            _moveToTargetComponent.SetStateMove(false);
        }

        private void Explode()
        {
            var amountCollider = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _colliderCashe);
            for (int i = 0; i < amountCollider; i++)
            {
                var collider = _colliderCashe[i];
                if (collider == null)
                    continue;

                if (collider.TryGetComponent<IDamageble>(out var damageable))
                {
                    damageable.GetDamage(_damageAmount);
                }

                _colliderCashe[i] = null;
            }

            Debug.Log("Bomb enemy invoke explode!");

            Die();
        }

        private void Die()
        {
            IsDead = true;
            OnEnemyDied?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void GetDamage(int damage)
        {
            if (IsDead)
                return;

            _healthHandler.SubjectHealth(damage);

            if (_healthHandler.Health == 0)
            {
                Die();
            }
        }
    }
}