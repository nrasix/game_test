using Game.Services.Input;
using UnityEngine;

namespace Game
{
    public sealed class LooseCanvas : MonoBehaviour
    {
        private Character _character;
        private IInputService _inputService;

        public void Init(Character character, IInputService inputService)
        {
            _character = character;
            _inputService = inputService;

            _character.OnLooseGame += OnLooseGame;
        }

        private void OnDestroy()
        {
            _character.OnLooseGame -= OnLooseGame;
        }

        private void OnLooseGame()
        {
            _inputService.SetGameInput(false);
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
    }
}