using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StartCanvas : MonoBehaviour
    {
        [SerializeField] private EntryPoint _entryPoint;
        [SerializeField] private Button _startGameButton;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            _entryPoint.InitializeGame();
            gameObject.SetActive(false);
        }
    }
}