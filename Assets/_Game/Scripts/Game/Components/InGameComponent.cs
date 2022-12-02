using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.LevelSystems;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class InGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void InGameChangeDelegate();

        public event InGameChangeDelegate OnInGameComplete;

        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private SwipeController swipeControllerPrefab;
        [SerializeField] private GameObject mainCameraPrefab;
        [SerializeField] private CinemachineVirtualCamera playerFollowerCameraPrefab;
        [SerializeField] private PlayerController playerControllerPrefab;
        [SerializeField] private LevelCreator levelCreatorPrefab;
        [SerializeField] private BallManager ballManagerPrefab;
        [SerializeField] private BallPool ballPoolPrefab;

        private BallPool ballPool;
        private GameObject mainCamera;
        private CinemachineVirtualCamera playerFollowerCamera;
        private PlayerController playerController;
        private LevelCreator levelCreator;
        private BallManager ballManager;
        private GameManager gameManager;
        private SwipeController swipeController;
        private EndGameComponent endGameComponent;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
        }

        public IEnumerator InitializeGame()
        {
            InitializePlayer();
            InitializeBallManager();
            InitializeBallPool();
            InitializeController();
            InitializeGameManager();
            InitializeCamera();
            InitializePlayerFollowerCamera();
            InitializeLevelCreator();

            yield return StartCoroutine(ballManager.InitializeBallManager(ballPool, playerController));
            yield return StartCoroutine(levelCreator.CreateLevel());

            SetupEndGame();
        }


        public void OnConstruct()
        {
            gameManager.GameOver += GameOver;
            gameManager.StartMove();
            swipeController.StartRotate();
        }

        public void OnDestruct()
        {
            gameManager.GameOver -= GameOver;
            gameManager.StopMove();
            swipeController.StopRotate();
        }

        public void DestroyGame()
        {
            DestroyPlayer();
            DestroyController();
            DestroyGameManager();
            DestroyCamera();
            DestroyPlayerFollowerCamera();
            DestroyLevelCreator();
            DestroyBallPool();
            DestroyBallManager();
        }

        private void SetupEndGame()
        {
            EndGameController endGameController = levelCreator.EndGameController;
            endGameController.Initialize(playerController);
            endGameComponent.SetEndGameController(endGameController);
        }

        private void DestroyBallManager()
        {
            Destroy(ballManager);
        }

        private void DestroyBallPool()
        {
            Destroy(ballPool);
        }

        private void DestroyLevelCreator()
        {
            levelCreator.DestroyLevel();
            Destroy(levelCreator);
        }

        private void DestroyCamera()
        {
            Destroy(mainCamera);
        }

        private void DestroyPlayerFollowerCamera()
        {
            Destroy(playerFollowerCamera);
        }

        private void DestroyGameManager()
        {
            Destroy(gameManager);
        }

        private void DestroyController()
        {
            Destroy(swipeController);
        }

        private void DestroyPlayer()
        {
            Destroy(playerController);
        }

        private void GameOver()
        {
            OnInGameComplete?.Invoke();
        }

        private void InitializeLevelCreator()
        {
            levelCreator = Instantiate(levelCreatorPrefab);
        }

        private void InitializeCamera()
        {
            mainCamera = Instantiate(mainCameraPrefab);
        }

        private void InitializePlayerFollowerCamera()
        {
            playerFollowerCamera = Instantiate(playerFollowerCameraPrefab);
            playerFollowerCamera.Follow = playerController.transform;
        }

        private void InitializeController()
        {
            swipeController = Instantiate(swipeControllerPrefab);
            swipeController.InitiliazeController(playerController);
        }

        private void InitializeGameManager()
        {
            gameManager = Instantiate(gameManagerPrefab);
            gameManager.InitializeGameManager(playerController, swipeController);
        }

        private void InitializeBallPool()
        {
            ballPool = Instantiate(ballPoolPrefab);
        }

        private void InitializePlayer()
        {
            playerController = Instantiate(playerControllerPrefab);
        }

        private void InitializeBallManager()
        {
            ballManager = Instantiate(ballManagerPrefab);
            ballManager.transform.parent = playerController.transform;
        }
    }
}