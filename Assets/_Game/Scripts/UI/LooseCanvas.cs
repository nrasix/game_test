using Game.Services.Input;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class LooseCanvas : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _restartGame;
        [SerializeField] private StartCanvas _startCanvas;

        private Character _character;
        private IInputService _inputService;

        private void OnEnable()
        {
            _restartGame.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            _restartGame.onClick.RemoveListener(RestartGame);
        }

        public void Init(Character character, IInputService inputService)
        {
            _character = character;
            _inputService = inputService;

            _character.OnLooseGame += OnLooseGame;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnLooseGame()
        {
            _inputService.SetGameInput(false);
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }

        private void RestartGame()
        {
            Time.timeScale = 1;
            ExtensionCode.RestartGame();

            gameObject.SetActive(false);
            _startCanvas.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            if (_character != null)
            {
                _character.OnLooseGame -= OnLooseGame;
                _character = null;
            }

            _inputService = null;
        }
    }
}