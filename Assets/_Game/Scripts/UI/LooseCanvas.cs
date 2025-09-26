using UnityEngine;

namespace Game
{
    public sealed class LooseCanvas : MonoBehaviour
    {
        private Character _character;

        public void Init(Character character)
        {
            _character = character;

            _character.OnLooseGame += OnLooseGame;
        }

        private void OnDestroy()
        {
            _character.OnLooseGame -= OnLooseGame;
        }

        private void OnLooseGame()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
    }
}