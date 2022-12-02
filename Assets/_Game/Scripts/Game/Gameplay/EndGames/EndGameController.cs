using System;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames
{
    public abstract class EndGameController : MonoBehaviour
    {
        protected PlayerController playerController;

        public void Initialize(PlayerController _playerController)
        {
            playerController = _playerController;
        }


        public abstract void LaunchEndGame();
    }
}