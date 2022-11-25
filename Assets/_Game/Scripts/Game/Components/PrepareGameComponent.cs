using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class PrepareGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void PrepareGameChangeDelegate();

        public event PrepareGameChangeDelegate OnGameLaunch;

        

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        public void OnConstruct()
        {
        }

        public void OnDestruct()
        {
        }
    }
}