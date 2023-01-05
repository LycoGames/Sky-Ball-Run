using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.FlooredEndGame
{
    public class FlooredEndGame : EndGameController
    {
        
        [SerializeField] private DiamondRewardVisualizer diamondRewardVisualizerPrefab;
        [SerializeField] private List<Floor> floorList;
        [SerializeField] private Vector3 camRotation;
        [SerializeField] private float xOffset = -10f;
        [SerializeField] private float cameraHeight=20f;
        [SerializeField] private float cameraRotateDuration=5f;
        [SerializeField] private float floorDistance = 7.75f;
        [SerializeField] private float playerForwardSpeed=20f;
        [SerializeField] private float waitForEndGame = 3f;
        [SerializeField] private Transform cameraStopPos;
        
        private DiamondRewardVisualizer diamondRewardVisualizer;
        private CinemachineVirtualCamera virtualCamera;
        private Transform cameraTarget;
        private Tweener cameraUpper;
        
        private int totalBallCount;
        private int collectedBallCount;
        private float nextCamYPos;
        public void Setup(PlayerController _playerController,Transform parent,CinemachineVirtualCamera _virtualCamera)
        {
            virtualCamera=_virtualCamera;
            diamondRewardVisualizer = Instantiate(diamondRewardVisualizerPrefab);
            diamondRewardVisualizer.DiamondCollected += IncreaseCoin;
            diamondRewardVisualizer.Parent = parent;
            playerController = _playerController;
            SetupTotalBallCount();
            SetupFloorList();
        }
        

        public override void LaunchEndGame()
        {
            MoveTowardPlayerController();
            SetupCamera();
        }
        

        private void MoveTowardPlayerController()
        {
            playerController.SetSpeed(playerForwardSpeed);
            playerController.StartMove();
            playerController.SetXPosition(playerController.transform.position.x);
        }

        private void SetupCamera()
        {
            Debug.Log("Call Setup Camera");
            cameraTarget = new GameObject("CameraTarget").transform;
            cameraTarget.parent = playerController.transform;
            nextCamYPos = cameraHeight;
            cameraTarget.transform.localPosition = new Vector3(0, cameraHeight, 0);
            virtualCamera.Follow = cameraTarget;
            virtualCamera.transform.DORotate(camRotation,cameraRotateDuration);
            cameraTarget.transform.DOLocalMoveX(xOffset, cameraRotateDuration);
            StartCoroutine(CameraChecker());
        }

        private IEnumerator CameraChecker()
        {
            while (cameraTarget.position.z<cameraStopPos.position.z)
            {
                yield return null;
            }
            cameraTarget.parent = transform;
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
            CheckFlooredGameEnd();
        }
        private void CheckFlooredGameEnd()
        {
            if (collectedBallCount == totalBallCount)
            {
                Invoke("WaitForEnd",waitForEndGame);
            }
        }

        private void WaitForEnd()
        {
            playerController.StopMove();
            StopAllCoroutines();
            EndGameEnded?.Invoke();
        }

        private void SetupTotalBallCount()
        {
            totalBallCount = BallManager.Instance.TotalBallCount;
        }
        private void SetupFloorList()
        {
            foreach (var floor in floorList)
            {
                floor.DiamondRewardVisualizer = diamondRewardVisualizer;
                floor.OnBallHit += IncreaseCoin;
                floor.OnFirstHit += MoveCameraUp;
            }
        }
        private void MoveCameraUp()
        {
            nextCamYPos += floorDistance;
            if(cameraUpper is {active:true})cameraUpper.Kill();
            cameraUpper = cameraTarget.transform.DOLocalMoveY(nextCamYPos,cameraRotateDuration);
        }
    }
}
