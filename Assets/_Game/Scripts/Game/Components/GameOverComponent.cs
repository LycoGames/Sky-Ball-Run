using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class GameOverComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
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
            inGameComponent.DestroyGame();
        }
    }
}