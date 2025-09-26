using System;

namespace Game.Services.Character.Data
{
    public sealed class HealthHandler : IHealthView
    {
        private int _health;
        private int _maxHealth;

        public int Health
        {
            get => _health;
            private set
            {
                _health = value;
                OnHealthChanged?.Invoke(_health);
            }
        }

        public int MaxHealth => _maxHealth;

        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        public HealthHandler(int health, int maxHealth)
        {
            _health = health;
            _maxHealth = maxHealth;
        }

        public void SubjectHealth(int health)
        {
            if (health < 0)
                throw new ArgumentException(nameof(health));

            if (Health - health < 0)
            {
                Health = 0;
                OnDead?.Invoke();
                return;
            }

            Health -= health;
        }

        public void AddHealth(int health)
        {
            if (Health + health < 0)
                throw new ArgumentException(nameof(health));

            if (Health + health > _maxHealth)
            {
                Health = _maxHealth;
            }
            else
            {
                Health += health;
            }
        }
    }

    public interface IHealthView
    {
        int Health { get; }
        int MaxHealth { get; }
        event Action<int> OnHealthChanged;
    }
}