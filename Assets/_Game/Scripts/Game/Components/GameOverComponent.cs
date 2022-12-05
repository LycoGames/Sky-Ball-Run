using System.Collections;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class GameOverComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void GameOverChangeDelegate();

        public event GameOverChangeDelegate GameOverComplete;
        public event GameOverChangeDelegate ReviveComplete;

        private InGameComponent inGameComponent;


        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
        }

        public void OnConstruct()
        {
        }

        public void OnDestruct()
        {
        }

        public void RemoveGame()
        {
            StartCoroutine(DestroyGame());
        }

        public void Reviving()
        {
            inGameComponent.Reviving();
            ReviveComplete?.Invoke();
            
        }

        private IEnumerator DestroyGame()
        {
            yield return inGameComponent.DestroyGame();
            GameOverComplete?.Invoke();
        }
    }
}