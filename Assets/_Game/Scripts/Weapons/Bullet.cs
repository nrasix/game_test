using System;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPooledObject<Bullet>
    {
        private const string ENEMY_TAG = "Enemy";
        private const string GROUND_TAG = "Ground";

        [SerializeField] private float _initialSpeed = 20f;
        [SerializeField] private int _amountDamage = 1;

        private Rigidbody _rigidbody;

        public event Action<Bullet> OnRevertToPool;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(Vector3 direction)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.AddForce(direction.normalized * _initialSpeed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(GROUND_TAG))
            {
                OnRevertToPool?.Invoke(this);
                return;
            }

            if (!collision.transform.CompareTag(ENEMY_TAG))
                return;

            if (!collision.transform.TryGetComponent<IDamageble>(out var damageble))
                return;

            damageble.GetDamage(_amountDamage);
            OnRevertToPool?.Invoke(this);
        }
    }
}