using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
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
        public Action EndGameEnded;

        private PlayerController playerController;
        private EndGameController endGameController;

        private DataComponent dataComponent;


        private int coin;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
            SetupCoin();
            endGameController.LaunchEndGame();
        }

        public void OnDestruct()
        {
            SaveCoinData();
        }

        public void SetEndGameController(EndGameController _endGameController)
        {
            endGameController = _endGameController;
        }

        private void ChangeCoin(int value)
        {
            coin = value;
            CoinChange?.Invoke(coin.ToString());
        }

        private void SaveCoinData()
        {
            dataComponent.InventoryData.ownedCoin = coin;
            dataComponent.SaveInventoryData();
        }

        private void SetupCoin()
        {
            coin = dataComponent.InventoryData.ownedCoin;
            CoinChange?.Invoke(coin.ToString());
        }
    }
}