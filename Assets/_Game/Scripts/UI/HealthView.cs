using Game.Services.Character.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Text _text;

        private IHealthView _healthView;

        public void Init(IHealthView healthView)
        {
            _healthView = healthView;

            OnHealthChanged(_healthView.Health);
            _healthView.OnHealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnHealthChanged(int value)
        {
            string newValue = $"{value}/{_healthView.MaxHealth}";
            _text.text = newValue;
        }

        public void Dispose()
        {
            if (_healthView != null)
            {
                _healthView.OnHealthChanged -= OnHealthChanged;
                _healthView = null;
            }
        }
    }
}