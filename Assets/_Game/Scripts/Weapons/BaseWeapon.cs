using UnityEngine;

namespace Game.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public bool IsCanShoot { get; protected set; } = true;

        public abstract void Init(ObjectPool<Bullet> bullets);
        public abstract void Shoot();
    }
}