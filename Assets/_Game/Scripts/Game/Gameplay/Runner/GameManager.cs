using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;
        public Action onStartGame;
        [SerializeField] private GameObject LoadingCanvas;
        [SerializeField] private GameObject StartCanvas;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private LinesController linesController;
        [SerializeField] private PlayerController playerController;
        

        void Awake()
        {
            gameManager = this;
            StartCoroutine(InitiliazeGame());
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
        IEnumerator InitiliazeGame()
        {
            LoadingCanvas.SetActive(true);
            StartCanvas.SetActive(false);
            Instantiate(linesController);
            playerController=Instantiate(playerController);
            mainCamera=Instantiate(mainCamera);
            yield return StartCoroutine(BallManager.ballManager.InitiliazeBallManager());
            yield return new WaitForSeconds(1f);
            LoadingCanvas.SetActive(false);
            StartCanvas.SetActive(true);
            yield return null;
        }
    }
}
