using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class IntroComponent : MonoBehaviour, IComponent, IConstructable
    {
        public delegate void IntroTimeChangeDelegate(float time);

        public event IntroTimeChangeDelegate OnIntroAnimationStart;

        public delegate void IntroChangeDelegate();

        public event IntroChangeDelegate OnIntroAnimationComplete;

        [SerializeField] private float animationTime = 0.1f;
        [SerializeField] private float logoShowTime=0.5f;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(logoShowTime);
            
            OnIntroAnimationStart?.Invoke(animationTime);

            yield return new WaitForSeconds(animationTime);

            OnIntroAnimationComplete?.Invoke();
        }
    }
}
