using System;

namespace Game.Services.Character.Data
{
    public sealed class HealthHandler
    {
        private int _health;
        
        public int Health 
        {
            get => _health;
            private set
            {
                _health = value;
                OnHealthChanged?.Invoke(_health);
            }
        }

        public event Action<int> OnHealthChanged;

        public HealthHandler(int health)
        {
            _health = health;
        }

        public void SubjectHealth(int health)
        {
            if (health < 0)
                throw new ArgumentException(nameof(health));

            if (Health - health < 0)
                Health = 0;

            Health -= health;
        }
    }
}