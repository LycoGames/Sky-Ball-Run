using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallGame : MonoBehaviour
    {
        [SerializeField] private List<WaterfallBasket> waterfallBasketList;

        private int totalBallCount;
        private int collectedBallCount;

        private PlayerController playerController;

        public void Setup(PlayerController _playerController)
        {
            playerController = _playerController;
            SetupBasketList();
            StartCoroutine(GameCoroutine());
        }


        private void SetupTotalBallCount()
        {
            totalBallCount = BallManager.Instance.TotalBallCount;
        }

        private IEnumerator GameCoroutine()
        {
            yield return GetWaterfallForm();
            yield return MoveDownwards();
        }

        private IEnumerator GetWaterfallForm()
        {
            Vector3 newPos = Vector3.zero;
            newPos.y = 60f;
            newPos.z = -95f;
            playerController.transform.DOMove(newPos, 2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(2f);
        }

        private IEnumerator MoveDownwards()
        {
            Vector3 newPos = transform.position;
            newPos.y = 0;
            playerController.transform.DOMove(newPos, 2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(2f);
        }

        private void IncreaseCoin(int coin)
        {
            // Coin += coin;
            // CoinChange?.Invoke(Coin.ToString());
            // IncreaseCollectedBallCount();
        }

        private void IncreaseCollectedBallCount()
        {
            collectedBallCount++;
            CheckWaterfallGameEnd();
        }

        private void CheckWaterfallGameEnd()
        {
            // if (collectedBallCount == totalBallCount)
            //     EndGameEnded?.Invoke();
        }

        private void ResetVariables()
        {
            totalBallCount = 0;
            collectedBallCount = 0;
        }


        private void UnRegisterActions()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected -= IncreaseCoin;
            }
        }

        private void SetupBasketList()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.GoldCollected += IncreaseCoin;
            }
        }
    }
}