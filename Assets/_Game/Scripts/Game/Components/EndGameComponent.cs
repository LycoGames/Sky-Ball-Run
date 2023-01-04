using System;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Enums;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.EndGames.Paintball;
using _Game.Scripts.Game.Gameplay.EndGames.Waterfall;
using _Game.Scripts.Game.Gameplay.Runner;
using _Game.Scripts.Game.Gameplay.Runner.Player;
using _Game.Scripts.Game.UserInterfaces.InGame;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class EndGameComponent : MonoBehaviour, IComponent, IConstructable, IDestructible
    {
        public Action<string> DiamondChange;
        public Action OnEndGameEnded;

        public EndGameController EndGameController { get; set; }
        public PlayerController PlayerController { get; set; }

        public int GainedDiamond { get; private set; }

        [SerializeField] private FixedJoystick fixedJoystick;

        private DataComponent dataComponent;
        private UIComponent uiComponent;
        private WealthCanvas wealthCanvas;

        private int lastSavedDiamond;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
            dataComponent = componentContainer.GetComponent("DataComponent") as DataComponent;
            uiComponent = componentContainer.GetComponent("UIComponent") as UIComponent;
            wealthCanvas = uiComponent.GetCanvas(CanvasTrigger.Wealth) as WealthCanvas;
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
                waterfallGame.Setup(PlayerController, wealthCanvas.transform);
            else
            {
                var paintballGame = EndGameController as PaintballGame;
                if (paintballGame != null) paintballGame.Setup(fixedJoystick);
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
            PlayDiamondCollectedSound();
            GainedDiamond = value;
            DiamondChange?.Invoke((lastSavedDiamond + GainedDiamond).ToString());
        }

        private void SaveDiamondData()
        {
            dataComponent.InventoryData.ownedDiamond = GainedDiamond + lastSavedDiamond;
            dataComponent.SaveInventoryData();
        }

        private void SaveLevel()
        {
            dataComponent.LevelData.currentLevel += 1;
            dataComponent.SaveLevelData();
        }

        private static void PlayDiamondCollectedSound()
        {
            //AudioSourceController.Instance.PlaySoundType(SoundType.DiamondCollected);
        }
    }
}