using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.EndGames.Paintball;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using Cinemachine;
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

        private DataComponent dataComponent;


        private int lastSavedCoin;
        private int gainedCoin;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
            SetupCoin();
            SetupEndGame();
        }

        public void OnDestruct()
        {
            EndGameController.GainedCoinChanged -= ChangeCoin;
            EndGameController.EndGameEnded -= EndGameEnded;

            SaveCoinData();
        }

        public void SetEndGameController(EndGameController _endGameController)
        {
            EndGameController = _endGameController;
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
            gainedCoin = value;
            CoinChange?.Invoke((lastSavedCoin + gainedCoin).ToString());
        }

        private void SaveCoinData()
        {
            dataComponent.InventoryData.ownedCoin = lastSavedCoin + gainedCoin;
            dataComponent.SaveInventoryData();
        }

        private void SetupCoin()
        {
            lastSavedCoin = dataComponent.InventoryData.ownedCoin;
            CoinChange?.Invoke(lastSavedCoin.ToString());
        }
    }
}