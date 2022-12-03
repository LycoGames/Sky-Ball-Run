using System;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames
{
    public abstract class EndGameController : MonoBehaviour
    {
        public Action<int> GainedCoinDiamond;
        public Action EndGameEnded;

        protected PlayerController playerController;
        protected int GainedCoin { get; set; }

        public void Initialize(PlayerController _playerController)
        {
            playerController = _playerController;
        }

        public abstract void LaunchEndGame();
    }
}