using System;
using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using _Game.Scripts.Game.Gameplay.Runner.LevelSystems;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.ObjectPools;
using Cinemachine;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class InGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void InGameChangeDelegate();

        public event InGameChangeDelegate OnInGameComplete;
        public event InGameChangeDelegate OnLoseGame;
        
        public Action<string> DiamondChange;

        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private SwipeController swipeControllerPrefab;
        [SerializeField] private GameObject mainCameraPrefab;
        [SerializeField] private CinemachineVirtualCamera playerFollowerCameraPrefab;
        [SerializeField] private CameraTarget cameraTargetPrefab;
        [SerializeField] private PlayerController playerControllerPrefab;
        [SerializeField] private LevelCreator levelCreatorPrefab;
        [SerializeField] private BallManager ballManagerPrefab;
        [SerializeField] private BallPool ballPoolPrefab;

        public int GainedDiamond { get; private set; }

        private BallPool ballPool;
        private GameObject mainCamera;
        private CinemachineVirtualCamera playerFollowerCamera;
        private PlayerController playerController;
        private LevelCreator levelCreator;
        private BallManager ballManager;
        private GameManager gameManager;
        private SwipeController swipeController;
        private EndGameComponent endGameComponent;
        private CameraTarget cameraTarget;


        private int lastSavedDiamond;
        

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
        }

        public IEnumerator InitializeGame(int level)
        {
            InitializeCamera();
            InitializePlayer();
            InitializeBallManager();
            InitializeBallPool();
            InitializeController();
            InitializeGameManager();
            InitializeCameraTarget();
            InitializePlayerFollowerCamera();
            InitializeLevelCreator();
            
            
            yield return StartCoroutine(levelCreator.CreateLevel(level));
            yield return StartCoroutine(ballManager.InitializeBallManager(ballPool, playerController,levelCreator.LevelSpecs(),playerFollowerCamera));

            SetupEndGame();
        }

     
        public void SetupDiamond(int value)
        {
            GainedDiamond = 0;
            lastSavedDiamond = value;
        }


        public void OnConstruct()
        {
            gameManager.OnGainDiamond += ChangeDiamond;
            gameManager.ArriveEndLine += ArriveEndLine;
            gameManager.LoseGame += LoseGame;
            gameManager.StartMove();
            swipeController.StartRotate();
        }

        public void OnDestruct()
        {
            gameManager.OnGainDiamond -= ChangeDiamond;
            gameManager.ArriveEndLine -= ArriveEndLine;
            gameManager.LoseGame -= LoseGame;
            gameManager.StopMove();
            swipeController.StopRotate();
        }

        public IEnumerator DestroyGame()
        {
            DestroyPlayer();
            DestroyController();
            DestroyGameManager();
            DestroyCamera();
            DestroyPlayerFollowerCamera();
            DestroyLevelCreator();
            DestroyBallPool();
            DestroyBallManager();
            DestroyCameraTarget();
            yield return null;
        }


        private void SetupEndGame()
        {
            EndGameController endGameController = levelCreator.EndGameController;
            endGameController.Initialize(playerController);
            endGameComponent.EndGameController = endGameController;
            endGameComponent.PlayerController = playerController;
        }

        private void DestroyCameraTarget()
        {
            Destroy(cameraTarget.gameObject);
        }
        private void DestroyBallManager()
        {
            ballManager.DestroyBallManager();
            Destroy(ballManager.gameObject);
        }

        private void DestroyBallPool()
        {
            Destroy(ballPool.gameObject);
        }

        private void DestroyLevelCreator()
        {
            levelCreator.DestroyLevel();
            Destroy(levelCreator.gameObject);
        }

        private void DestroyCamera()
        {
            Destroy(mainCamera.gameObject);
        }

        private void DestroyPlayerFollowerCamera()
        {
            Destroy(playerFollowerCamera.gameObject);
        }

        private void DestroyGameManager()
        {
            Destroy(gameManager.gameObject);
        }

        private void DestroyController()
        {
            Destroy(swipeController.gameObject);
        }

        private void DestroyPlayer()
        {
            Destroy(playerController.gameObject);
        }

        private void ArriveEndLine()
        {
            endGameComponent.SetupDiamond(GainedDiamond+lastSavedDiamond);
            OnInGameComplete?.Invoke();
        }

        private void LoseGame()
        {
            OnLoseGame?.Invoke();
        }
        private void InitializeCameraTarget()
        {
            cameraTarget = Instantiate(cameraTargetPrefab);
            cameraTarget.InitiliazeCameraTarget(ballManager,playerController.transform);
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
            playerFollowerCamera.Follow = cameraTarget.transform;
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

        private void ChangeDiamond(int value)
        {
            GainedDiamond += value;
            DiamondChange?.Invoke((lastSavedDiamond + GainedDiamond).ToString());
        }
    }
}