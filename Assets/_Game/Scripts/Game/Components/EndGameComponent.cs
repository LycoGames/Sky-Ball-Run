using System;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.EndGames.Paintball;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class EndGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public delegate void EndGameChangeDelegate();

        public event EndGameChangeDelegate OnSuccess;
        public event EndGameChangeDelegate OnFail;

        public Action<string> DiamondChange;
        public Action OnEndGameEnded;

        public EndGameController EndGameController { get; set; }
        public PlayerController PlayerController { get; set; }

        public int GainedDiamond { get; private set; }

        private DataComponent dataComponent;

        private int lastSavedDiamond;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
        }

        public void OnConstruct()
        {
            SetupEndGame();
        }


        public void OnDestruct()
        {
            EndGameController.GainedCoinDiamond -= ChangeDiamond;
            EndGameController.EndGameEnded -= EndGameEnded;
            SaveDiamondData();
            SaveLevel();
        }
        
        

        private void SetupEndGame()
        {
            EndGameController.GainedCoinDiamond += ChangeDiamond;
            EndGameController.EndGameEnded += EndGameEnded;

            var waterfallGame = EndGameController as WaterfallGame;
            if (waterfallGame != null)
                waterfallGame.Setup(PlayerController);
            else
            {
                var paintballGame = EndGameController as PaintballGame;
                if (paintballGame != null) paintballGame.Setup();
            }

            EndGameController.LaunchEndGame();
        }

        private void EndGameEnded()
        {
            OnEndGameEnded?.Invoke();
        }

        public void SetupDiamond(int value)
        {
            GainedDiamond = 0;
            lastSavedDiamond = value;
        }

        private void ChangeDiamond(int value)
        {
            GainedDiamond = value;
            DiamondChange?.Invoke((lastSavedDiamond + GainedDiamond).ToString());
        }
        private void SaveDiamondData()
        {
            dataComponent.InventoryData.ownedDiamond = GainedDiamond+lastSavedDiamond;
            dataComponent.SaveInventoryData();
        }

        private void SaveLevel()
        {
            dataComponent.LevelData.currentLevel += 1;
            dataComponent.SaveLevelData();
        }
    }
}