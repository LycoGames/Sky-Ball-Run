using System.Collections;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class PrepareGameComponent : MonoBehaviour, IComponent, IConstructable
    {
        private ComponentContainer componentContainer;
        public delegate void PrepareGameChangeDelegate();

        public event PrepareGameChangeDelegate OnGameLaunch;



        public void Initialize(ComponentContainer _componentContainer)
        {
            componentContainer = _componentContainer;
            _componentContainer.AddComponent(gameObject.name,this);
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            StartCoroutine(PreparingGame());
        }

        private IEnumerator PreparingGame()
        {
            InGameComponent inGameComponent=componentContainer.GetComponent("InGameComponent") as InGameComponent;
            yield return inGameComponent.StartCoroutine(inGameComponent.InitiliazeGame());
            OnGameLaunch?.Invoke();
        }

        
    }
}