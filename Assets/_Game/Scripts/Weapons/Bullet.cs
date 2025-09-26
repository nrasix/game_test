using System;
using UnityEngine;

namespace Game.Weapons
{
    public class Bullet : MonoBehaviour, IPooledObject<Bullet>
    {
        public event Action<Bullet> OnRevertToPool;
    }
}