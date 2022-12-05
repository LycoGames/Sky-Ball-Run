using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using _Game.Scripts.Game.States.InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnArriveEndLineDelegate();
        public delegate void OnLoseGameDelegate();

        public event OnArriveEndLineDelegate ArriveEndLine;

        public Action<int> OnGainDiamond;
        public Action OnRevive;

        public event OnLoseGameDelegate LoseGame;
        public static GameManager Instance;

        [SerializeField] private float reviveTime=3;

        private PlayerController playerController;
        private SwipeController swipeController;

        void Awake()
        {
            Instance = this;
        }

        public void Reviving()
        {
            playerController.ReviveSpeedSet();
            OnRevive?.Invoke();
        }
        public int GetBallCount()
        {
            return BallPool.Instance.GetAllActiveBall().Count;
        }

        public void GainedDiamond(int value)
        {
            PlayDiamondCollectedSound();
            OnGainDiamond?.Invoke(value);
        }

        public void OnLoseGame()
        {
            LoseGame?.Invoke();
        }
        public void OnGameOver()
        {
            ArriveEndLine?.Invoke();
        }

        public PlayerController GetPlayerController()
        {
            return playerController;
        }

        public void StopMove()
        {
            playerController.StopMove();
            swipeController.StopRotate();
        }

        public void StartMove()
        {
            playerController.StartMove();
            swipeController.StartRotate();
        }

        public void InitializeGameManager(PlayerController _playerController, SwipeController _swapController)
        {
            playerController = _playerController;
            swipeController = _swapController;
        }
        private static void PlayDiamondCollectedSound()
        {
            AudioSourceController.Instance.PlaySoundType(SoundType.DiamondCollected);
        }
    }
}