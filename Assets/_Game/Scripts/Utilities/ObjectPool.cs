using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ObjectPool<T> : IDisposable where T : MonoBehaviour, IPooledObject<T>
    {
        private readonly Queue<T> _poolQueue;

        private readonly Transform _parentTransform;
        private readonly T _prefab;

        private int _countItems = 0;

        public IReadOnlyCollection<T> PoolQueue => _poolQueue;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parentTransform = parent;

            _poolQueue = new(initialSize);

            for (int i = 0; i < initialSize; i++)
            {
                _poolQueue.Enqueue(CreateNewObject());
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _poolQueue.Count; i++)
            {
                var pooledObject = _poolQueue.Dequeue();
                pooledObject.OnRevertToPool -= ReturnToPool;
            }
        }

        private T CreateNewObject(bool isActive = false)
        {
            T newObject = GameObject.Instantiate(_prefab, _parentTransform);
            newObject.OnRevertToPool += ReturnToPool;
            _countItems++;
            newObject.name = $"{newObject.name}_{_countItems}";
            newObject.gameObject.SetActive(isActive);
            return newObject;
        }

        public T Get(bool isActive)
        {
            T pooledObject;
            if (_poolQueue.Count > 0)
            {
                var objectPeek = _poolQueue.Peek();

                pooledObject = _poolQueue.Dequeue();
                pooledObject.gameObject.SetActive(isActive);
            }
            else
            {
                pooledObject = CreateNewObject(isActive);
            }

            return pooledObject;
        }

        public void ReturnToPool(T objectToReturn)
        {
            if (_poolQueue.Contains(objectToReturn))
                return;

            objectToReturn.gameObject.SetActive(false);
            _poolQueue.Enqueue(objectToReturn);
        }
    }
}