using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
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

        [SerializeField] private List<WaterfallBasket> waterfallBasketList;

        private DataComponent dataComponent;

        private int totalBallCount;
        private int collectedBallCount;

        public int Coin { get; set; }

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
            SetupCoin();
            SetupTotalBallCount();

            StartCoroutine(WaterfallEndGameCoroutine());
        }


        public void OnDestruct()
        {
            UnRegisterActions();
            ResetVariables();
            SaveCoinData();
        }

        private void SaveCoinData()
        {
            dataComponent.InventoryData.ownedCoin = Coin;
            dataComponent.SaveInventoryData();
        }

        private IEnumerator WaterfallEndGameCoroutine()
        {
            SetupBasketList();
            yield return BallManager.Instance.GetWaterfallForm();
            yield return BallManager.Instance.MoveDownwards();
        }

        private void SetupCoin()
        {
            Coin = dataComponent.InventoryData.ownedCoin;
            CoinChange?.Invoke(Coin.ToString());
        }

        private void SetupTotalBallCount()
        {
            totalBallCount = BallManager.Instance.TotalBallCount;
        }


        private void IncreaseCoin(int coin)
        {
            Coin += coin;
            CoinChange?.Invoke(Coin.ToString());
            IncreaseCollectedBallCount();
        }

        private void IncreaseCollectedBallCount()
        {
            collectedBallCount++;
            CheckWaterfallGameEnd();
        }

        private void CheckWaterfallGameEnd()
        {
            if (collectedBallCount == totalBallCount)
                EndGameEnded?.Invoke();
        }

        private void ResetVariables()
        {
            totalBallCount = 0;
            collectedBallCount = 0;
        }

        private void SetupBasketList()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected += IncreaseCoin;
            }
        }

        private void UnRegisterActions()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected -= IncreaseCoin;
            }
        }
    }
}