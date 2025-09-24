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

        private HashSet<IInitializable> _initializables;
        private HashSet<IDisposable> _disposables;
        private HashSet<IFixedUpdatable> _fixedUpdatables;

        public void Awake()
        {
            _initializables = new();
            _disposables = new();
            _fixedUpdatables = new();

            var inputService = new InputService(_mainCamera, _groundLayer);
            _initializables.Add(inputService);
            _disposables.Add(inputService);
            _fixedUpdatables.Add(inputService);

            _character.Init(inputService, _mainCamera);
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