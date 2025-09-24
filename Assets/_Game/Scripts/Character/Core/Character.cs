using Game.Services.Character;
using Game.Services.Character.Data;
using Game.Services.Input;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, ITarget, IDamageble
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private int _health = 5;
        [SerializeField] private CapsuleCollider _capsuleCollider;

        private HealthHandler _healthHandler;
        private DirectionMoveComponent _directionMoveComponent;
        private TransformRotator _trasformRotator;

        private IInputService _inputService;

        public Transform Transform => transform;

        public void Init(IInputService inputService, Camera mainCamera)
        {
            _inputService = inputService;

            _directionMoveComponent = new(inputService, transform, mainCamera, _capsuleCollider.radius);
            _trasformRotator = new(transform, inputService);

            _healthHandler = new(_health);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enemy has trigger!");
            //damageble.GetDamage(_damageAmount);
        }

        private void OnDestroy()
        {
            _directionMoveComponent.Dispose();
            _trasformRotator.Dispose();
        }

        public void GetDamage(int damage)
        {
            Debug.Log("Player is get damage!");
            _healthHandler.SubjectHealth(_health);
        }
    }
}