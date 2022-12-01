using System.Collections;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class PrepareGameComponent : MonoBehaviour, IComponent, IConstructable
    {
        public delegate void PrepareGameChangeDelegate();

        public event PrepareGameChangeDelegate OnGameLaunch;
        private InGameComponent inGameComponent;


        public void Initialize(ComponentContainer componentContainer)
        {
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            StartCoroutine(PreparingGame());
        }

        private IEnumerator PreparingGame()
        {
            yield return inGameComponent.StartCoroutine(inGameComponent.InitializeGame());
            OnGameLaunch?.Invoke();
        }
    }
}