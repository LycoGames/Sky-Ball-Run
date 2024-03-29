using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.Waterfall
{
    public class WaterfallGame : EndGameController
    {
        [SerializeField] private List<WaterfallBasket> waterfallBasketList;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform ballSetupTransform;
        [SerializeField] private WaterfallCollider waterfallCollider;
        [SerializeField] private DiamondRewardVisualizer diamondRewardVisualizerPrefab;
        [SerializeField] private float columnDistance = 0.1f;

        private DiamondRewardVisualizer diamondRewardVisualizer;
        private Tweener playerMover;

        private int totalBallCount;
        private int collectedBallCount;

        //private PlayerController playerController;

        public void Setup(PlayerController _playerController, Transform parent)
        {
            diamondRewardVisualizer = Instantiate(diamondRewardVisualizerPrefab);
            diamondRewardVisualizer.DiamondCollected += IncreaseCoin;
            diamondRewardVisualizer.Parent = parent;
            playerController = _playerController;
            SetupTotalBallCount();
            SetupBasketList();
        }

        public override void LaunchEndGame()
        {
            StartCoroutine(GameCoroutine());
        }

        private void SetupTotalBallCount()
        {
            totalBallCount = BallManager.Instance.TotalBallCount;
        }

        private IEnumerator GameCoroutine()
        {
            SetupCamera();
            BallManager.Instance.SetBallColumnDistance(columnDistance);
            yield return GetWaterfallForm();
            yield return MoveDownwards();
        }

        private void SetupCamera()
        {
            virtualCamera.Priority = 15;
        }

        private IEnumerator GetWaterfallForm()
        {
            playerController.transform.DOMove(ballSetupTransform.position, 1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            waterfallCollider.GameStarted = true;
        }

        private IEnumerator MoveDownwards()
        {
            //Vector3 newPos = transform.position;
            //newPos.y = 0;
            playerMover = playerController.transform.DOMoveY(-50, 2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(2f);
        }

        private void IncreaseCoin(int coin)
        {
            GainedCoin += coin;
            GainedCoinDiamond?.Invoke(GainedCoin);
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
            {
                if (playerMover is { active: true }) playerMover.Kill();
                EndGameEnded?.Invoke();
            }
        }

        // private void UnRegisterActions()
        // {
        //     foreach (var basket in waterfallBasketList)
        //     {
        //         basket.GoldCollected -= IncreaseCoin;
        //     }
        // }

        private void SetupBasketList()
        {
            foreach (var basket in waterfallBasketList)
            {
                basket.DiamondRewardVisualizer = diamondRewardVisualizer;
            }
        }
    }
}