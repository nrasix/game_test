using System;
using System.Collections.Generic;
using Game.Utilities;
using UnityEngine;
using Game;
using Game.Services.Input;

namespace Game.Services
{
    public sealed class EntyPoint : MonoBehaviour
    {
        [SerializeField] private Game.Character _character;
        
        private HashSet<IInitializable> _initializables;
        private HashSet<IDisposable> _disposables;
        private HashSet<IFixedUpdatable> _fixedUpdatables;
    
        public void Awake()
        {
            _initializables = new();
            _disposables = new();
            _fixedUpdatables = new();

            var inputService = new DesktopInputService();
            _initializables.Add(inputService);
            _disposables.Add(inputService);
            _fixedUpdatables.Add(inputService);
            
            _character.Init(inputService);
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