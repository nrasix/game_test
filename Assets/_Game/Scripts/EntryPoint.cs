using Game.Services.Character.Data;
using Game.Services.Input;
using Game.Utilities;
using Game.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public sealed class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Game.Character _characterPrefab;
        [SerializeField] private int _characterHealth = 5;
        [SerializeField] private Vector3 _spawnCharacterPosition;

        [Space(5)]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private EnemySpawner _enemyService;

        [Space(5)]
        [SerializeField] private List<WeaponSpawnSettings> _weaponList;
        [SerializeField] private Bullet _prefabBullet;

        [Header("UI")]
        [SerializeField] private HealthView _healthView;
        [SerializeField] private LooseCanvas _looseCanvas;

        private HashSet<IInitializable> _initializables = new();
        private HashSet<IDisposable> _disposables = new();
        private HashSet<IFixedUpdatable> _fixedUpdatables = new();

        private Game.Character _character;
        private HealthHandler _healthHandler;

        private bool _isInitialized;

        private void Awake()
        {
            ExtensionCode.OnRestartGame += RestartGame;
        }

        public void InitializeGame()
        {
            _initializables = new();
            _disposables = new();
            _fixedUpdatables = new();

            var inputService = new InputService(_mainCamera, _groundLayer);
            _initializables.Add(inputService);
            _disposables.Add(inputService);
            _fixedUpdatables.Add(inputService);

            _healthHandler = new HealthHandler(_characterHealth, _characterHealth);

            _character = Instantiate(_characterPrefab, _spawnCharacterPosition, Quaternion.identity);
            _character.Init(inputService, _mainCamera, _weaponList, _prefabBullet, 20, _healthHandler);

            _enemyService.Init(_mainCamera, _character);
            _disposables.Add(_enemyService);

            _healthView.Init(_healthHandler);
            _disposables.Add(_healthView);

            _looseCanvas.Init(_character, inputService);
            _disposables.Add(_looseCanvas);

            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }

            _isInitialized = true;
        }

        public void OnDestroy()
        {
            DisposeGame();

            ExtensionCode.OnRestartGame -= RestartGame;
        }

        public void FixedUpdate()
        {
            if (!_isInitialized)
                return;

            foreach (var fixedUpdatable in _fixedUpdatables)
            {
                fixedUpdatable.FixedUpdate(Time.fixedDeltaTime);
            }
        }

        private void RestartGame()
        {
            if (!_isInitialized)
                return;

            DisposeGame();
        }

        private void DisposeGame()
        {
            if (!_isInitialized)
                return;

            _isInitialized = false;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            Destroy(_character.gameObject);

            _healthHandler = null;

            _disposables.Clear();
            _disposables = null;

            _fixedUpdatables.Clear();
            _fixedUpdatables = null;

            _initializables.Clear();
            _initializables = null;
        }
    }
}