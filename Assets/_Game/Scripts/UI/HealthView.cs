using Game.Services.Character.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthView : MonoBehaviour
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
            _healthView.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int value)
        {
            string newValue = $"{value}/{_healthView.MaxHealth}";
            _text.text = newValue;
        }
    }
}