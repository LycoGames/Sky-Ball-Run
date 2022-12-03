using System;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.EndGames.Paintball;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class EndGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void EndGameChangeDelegate();

        public event EndGameChangeDelegate OnSuccess;
        public event EndGameChangeDelegate OnFail;

        public Action<string> CoinChange;
        public Action OnEndGameEnded;

        public EndGameController EndGameController { get; set; }
        public PlayerController PlayerController { get; set; }

        public int GainedCoin { get; private set; }

        private int lastSavedCoin;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            SetupEndGame();
        }

        public void OnDestruct()
        {
            EndGameController.GainedCoinChanged -= ChangeCoin;
            EndGameController.EndGameEnded -= EndGameEnded;
        }

        private void SetupEndGame()
        {
            EndGameController.GainedCoinChanged += ChangeCoin;
            EndGameController.EndGameEnded += EndGameEnded;

            var waterfallGame = EndGameController as WaterfallGame;
            if (waterfallGame != null)
                waterfallGame.Setup(PlayerController);
            else
            {
                var paintballGame = EndGameController as PaintballGame;
                if (paintballGame != null) paintballGame.Setup();
            }

            EndGameController.LaunchEndGame();
        }

        private void EndGameEnded()
        {
            OnEndGameEnded?.Invoke();
        }

        private void ChangeCoin(int value)
        {
            GainedCoin = value;
            CoinChange?.Invoke((lastSavedCoin + GainedCoin).ToString());
        }
    }
}