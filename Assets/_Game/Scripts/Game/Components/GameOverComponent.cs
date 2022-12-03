using System.Collections;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class GameOverComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void GameOverChangeDelegate();

        public event GameOverChangeDelegate GameOverComplete;

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
        

        private IEnumerator DestroyGame()
        {
            yield return inGameComponent.DestroyGame();
            GameOverComplete?.Invoke();
        }
    }
}