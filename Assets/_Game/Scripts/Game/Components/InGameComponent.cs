using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class InGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void InGameChangeDelegate();

        public event InGameChangeDelegate OnInGameComplete;

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