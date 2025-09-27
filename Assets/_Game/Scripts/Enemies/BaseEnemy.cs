using Game.Services.Character;
using System;
using UnityEngine;

namespace Game
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        [SerializeField] protected int _healthAmount = 1;

        public abstract event Action<BaseEnemy> OnEnemyDied;
        public abstract void Init(ITarget targetObject);
        public abstract void ResetEnemy();
    }
}