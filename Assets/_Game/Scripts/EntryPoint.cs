using Game.Services.Input;
using Game.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public sealed class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Game.Character _character;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private EnemySpawner _enemyService;

        [Header("UI")]
        [SerializeField] private HealthView _healthView;
        [SerializeField] private LooseCanvas _looseCanvas;

        private HashSet<IInitializable> _initializables = new();
        private HashSet<IDisposable> _disposables = new();
        private HashSet<IFixedUpdatable> _fixedUpdatables = new();

        public void Awake()
        {
            var inputService = new InputService(_mainCamera, _groundLayer);
            _initializables.Add(inputService);
            _disposables.Add(inputService);
            _fixedUpdatables.Add(inputService);

            _character.Init(inputService, _mainCamera);
            _enemyService.Init(_mainCamera, _character);

            _healthView.Init(_character.HealthView);
            _looseCanvas.Init(_character);
        }

        public void Start()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
        }

        public void FixedUpdate()
        {
            foreach (var fixedUpdatable in _fixedUpdatables)
            {
                fixedUpdatable.FixedUpdate(Time.fixedDeltaTime);
            }
        }

        public void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}