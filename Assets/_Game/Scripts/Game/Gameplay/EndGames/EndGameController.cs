using System;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames
{
    public class EndGameController : MonoBehaviour
    {
        protected Action OnEndGameStarted;
        private PlayerController playerController;

        public void Initialize(PlayerController _playerController)
        {
            playerController = _playerController;
        }

        public void EndGameStarted()
        {
            OnEndGameStarted?.Invoke();
        }
    }
}