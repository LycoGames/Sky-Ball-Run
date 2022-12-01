using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class GameOverComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
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