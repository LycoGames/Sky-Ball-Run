using System.Collections;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class LoadingGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void LoadingTimeChangeDelegate(float time);

        public event LoadingTimeChangeDelegate OnLoadingSliderStart;

        public delegate void LoadingGameChangeDelegate();

        public event LoadingGameChangeDelegate OnLoadingComplete;

        private const float animationTime = 1f;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            StartCoroutine(LoadLevel());
        }

        public void OnDestruct()
        {
            StopCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            OnLoadingSliderStart?.Invoke(animationTime);
            yield return new WaitForSeconds(animationTime);
            OnLoadingComplete?.Invoke();
        }
    }
}