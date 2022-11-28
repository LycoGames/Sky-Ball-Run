using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class GameManager : MonoBehaviour
    {
        public Action onEnterCheckpoint;
        public Action onExitCheckpoint;
        public delegate void EndOfLineDelegate();

        public event EndOfLineDelegate OnEndOfLine;
        
        public static GameManager Instance;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private LinesController linesController;
        [SerializeField] private PlayerController playerController;


        void Awake()
        {
            Instance = this;
        }
        public void ResetGame()
        {
            SceneManager.LoadScene(0);
        }

        public void ArriveEndOfLine()
        {
            OnEndOfLine?.Invoke();
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
            yield return StartCoroutine(linesController.InitializeLines());
            yield return StartCoroutine(BallManager.Instance.InitializeBallManager());
        }
    }
}