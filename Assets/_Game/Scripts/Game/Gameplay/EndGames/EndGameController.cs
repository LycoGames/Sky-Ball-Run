using System;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames
{
    public abstract class EndGameController : MonoBehaviour
    {
        public Action<int> GainedCoinDiamond;
        public Action EndGameEnded;

        [SerializeField] private ParticleSystem confettiEffect;

        protected PlayerController playerController;
        protected int GainedCoin { get; set; }

        public void Initialize(PlayerController _playerController)
        {
            playerController = _playerController;
            EndGameEnded += PlayConfettiEffect;
        }

        private void PlayConfettiEffect()
        {
            confettiEffect.Play();
            EndGameEnded -= PlayConfettiEffect;
        }

        protected void SetConfettiPos(Vector3 pos)
        {
            confettiEffect.transform.position = pos;
        }

        public abstract void LaunchEndGame();
    }
}