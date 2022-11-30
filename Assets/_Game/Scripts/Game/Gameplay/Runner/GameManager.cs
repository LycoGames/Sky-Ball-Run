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
        public Action onEnterCheckpoint;
        public Action onExitCheckpoint;
        public delegate void GameOverDelegate();

        public event GameOverDelegate GameOver;
        
        public static GameManager Instance;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private LinesController linesController;
        [SerializeField] private PlayerController playerController;


        void Awake()
        {
            Instance = this;
        }

        public int GetBallCount()
        {
            return BallPool.Instance.GetAllActiveBall().Count;
        }
        public void ResetGame()
        {
            SceneManager.LoadScene(0);
        }

        public void OnGameOver()
        {
            GameOver?.Invoke();
        }

        public PlayerController GetPlayerController()
        {
            return playerController;
        }

        public void StopGame() => playerController.StopMove();

        public void StartGame() => playerController.StartMove();

        public IEnumerator InitializeGame()
        {
            linesController = Instantiate(linesController);
            playerController = Instantiate(playerController);
            mainCamera = Instantiate(mainCamera);
            //yield return StartCoroutine(linesController.InitializeLines());
            yield return StartCoroutine(BallManager.Instance.InitializeBallManager());
        }
    }
}