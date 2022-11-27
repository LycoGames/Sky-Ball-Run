using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class InGameComponent : MonoBehaviour, IComponent, IConstructable,IDestructible
    {
        public delegate void InGameChangeDelegate();

        public event InGameChangeDelegate OnInGameComplete;

        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private SwipeController swipeControllerPrefab;

        public GameManager GameManager { get; private set; }
        public SwipeController SwipeController { get; private set; }

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            GameManager = Instantiate(gameManagerPrefab);
            SwipeController = Instantiate(swipeControllerPrefab);
        }

        public IEnumerator InitiliazeGame()
        {
            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.InitializeGame());
            SwipeController.InitiliazeController();
        }

        public void OnConstruct()
        {
            GameManager.OnEndOfLine += OnEndOfLine;
            GameManager.StartGame();
            SwipeController.StartRotate();
        }

        public void OnDestruct()
        {
            GameManager.OnEndOfLine -= OnEndOfLine;
            GameManager.StopGame();
            SwipeController.StopRotate();
        }

        public void DestroyGame()
        {
            
        }

        private void OnEndOfLine()
        {
            OnInGameComplete?.Invoke();
        }
    }
}