using Game.Services.Character;
using Game.Services.Character.Data;
using Game.Services.Input;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ITarget, IDamageble
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private int _health = 5;

        private CharacterController _characterController;

        private HealthHandler _healthHandler;
        private DirectionMoveComponent _directionMoveComponent;
        private TransformRotator _trasformRotator;

        private IInputService _inputService;

        public Transform Transform => transform;

        public void Init(IInputService inputService, Camera mainCamera)
        {
            _characterController = GetComponent<CharacterController>();
            _inputService = inputService;

            _directionMoveComponent = new(inputService, _characterController, mainCamera);
            _trasformRotator = new(transform, inputService);

            _healthHandler = new(_health);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Player has trigger!");
            //damageble.GetDamage(_damageAmount);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Player has collision {collision.gameObject.name}!");
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