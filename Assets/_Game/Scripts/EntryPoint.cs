using System;
using System.Collections.Generic;
using Game.Utilities;
using UnityEngine;

namespace Game.Services
{
    public sealed class EntyPoint : MonoBehaviour
    {
        private HashSet<IInitializable> _initializables;
        private HashSet<IDisposable> _disposables;
    
        public void Awake()
        {
            _initializables = new();
            _disposables = new();
        }

        public void Start()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
        }

        public void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        public void RegisterInitializable()
        {
        
        }

        public void RegisterDisposables()
        {
        
        }
    }
}