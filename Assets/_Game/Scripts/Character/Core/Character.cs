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

        private HealthHandler _healthHandler;
        private DirectionMoveComponent _directionMoveComponent;
        private TransformRotator _trasformRotator;

        private IInputService _inputService;

        private Vector3 _testPos;

        public Transform Transform => transform;

        public void Init(IInputService inputService)
        {
            _inputService = inputService;

            var characterController = GetComponent<CharacterController>();
            _directionMoveComponent = new(inputService, characterController);
            //_trasformRotator = new(transform, inputService);

            _healthHandler = new(_health);

            _inputService.LookPerformed += LookPerformed;
        }

        private void OnDestroy()
        {
            _directionMoveComponent.Dispose();
            //_trasformRotator.Dispose();

            _inputService.LookPerformed -= LookPerformed;
        }

        public void GetDamage(int damage)
        {
            Debug.Log("Player is get damage!");
            _healthHandler.SubjectHealth(_health);
        }

        private void LookPerformed(Vector3 vector)
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_testPos, 1f);
        }
    }
}