using System;

namespace Game
{
    public static class ExtensionCode
    {
        public static event Action OnRestartGame;

        public static void RestartGame()
        {
            OnRestartGame?.Invoke();
        }
    }
}