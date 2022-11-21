using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Action onStartGame;
        [SerializeField] private GameObject LoadingCanvas;
        [SerializeField] private GameObject EndCanvas;
        [SerializeField] private GameObject StartCanvas;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private LinesController linesController;
        [SerializeField] private PlayerController playerController;
        
        
        void Awake()
        {
            Instance = this;
            StartCoroutine(InitializeGame());
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(0);
        }

        public void OnEndOfLine()
        {
            playerController.StopMove();
            EndCanvas.SetActive(true);
        }
        public PlayerController GetPlayerController()
        {
            return playerController;
        }

        public void StartGame()
        {
            StartCanvas.SetActive(false);
            onStartGame?.Invoke();
        }
        private IEnumerator InitializeGame()
        {
            LoadingCanvas.SetActive(true);
            StartCanvas.SetActive(false);
            EndCanvas.SetActive(false);
            linesController=Instantiate(linesController);
            playerController=Instantiate(playerController);
            mainCamera=Instantiate(mainCamera);
            yield return StartCoroutine(linesController.InitializeLines());
            yield return StartCoroutine(BallManager.Instance.InitializeBallManager());
            yield return new WaitForSeconds(1f);
            LoadingCanvas.SetActive(false);
            StartCanvas.SetActive(true);
            yield return null;
        }
    }
}
