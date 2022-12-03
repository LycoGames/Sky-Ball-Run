using System;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class StartGameComponent : MonoBehaviour, IComponent, IConstructable
    {
        public delegate void StartGameWealthChangeDelegate(string value);

        public event StartGameWealthChangeDelegate CoinChange;
        public event StartGameWealthChangeDelegate DiamondChange;

        private DataComponent dataComponent;

        public void Initialize(ComponentContainer _componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = _componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
            SetupCoin();
            SetupDiamond();
        }

        private void SetupCoin()
        {
            var coin = dataComponent.InventoryData.ownedCoin;
            CoinChange?.Invoke(coin.ToString());
        }

        private void SetupDiamond()
        {
            var diamond = dataComponent.InventoryData.ownedDiamond;
            DiamondChange?.Invoke(diamond.ToString());
        }
    }
}