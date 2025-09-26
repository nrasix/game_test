using System;
using UnityEngine;

namespace Game
{
    public interface IPooledObject<T> where T : MonoBehaviour
    {
        event Action<T> OnRevertToPool;
    }
}