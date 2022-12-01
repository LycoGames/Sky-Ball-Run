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
    
        public delegate void GameOverDelegate();

        public event GameOverDelegate GameOver;
        
        public static GameManager Instance;

        private PlayerController playerController;
        private SwipeController swipeController;

        void Awake()
        {
            Instance = this;
        }

        public int GetBallCount()
        {
            return BallPool.Instance.GetAllActiveBall().Count;
        }

        public void OnGameOver()
        {
            GameOver?.Invoke();
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

        public void InitializeGameManager(PlayerController _playerController,SwipeController _swapController)
        {
            playerController = _playerController;
            swipeController = _swapController;
        }
    }
}