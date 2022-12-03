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
        private EndGameComponent endGameComponent;
        private DataComponent dataComponent;


        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
            endGameComponent = componentContainer.GetComponent("EndGameComponent") as EndGameComponent;
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
        }

        public void OnDestruct()
        {
            SaveCoinData();
            SaveDiamondData();
            dataComponent.SaveInventoryData();
        }

        public void RemoveGame()
        {
            StartCoroutine(DestroyGame());
        }

        private void SaveCoinData()
        {
            dataComponent.InventoryData.ownedCoin += endGameComponent.GainedCoin;
        }

        private void SaveDiamondData()
        {
            dataComponent.InventoryData.ownedDiamond += inGameComponent.GainedDiamond;
        }

        private IEnumerator DestroyGame()
        {
            yield return inGameComponent.DestroyGame();
            GameOverComplete?.Invoke();
        }
    }
}