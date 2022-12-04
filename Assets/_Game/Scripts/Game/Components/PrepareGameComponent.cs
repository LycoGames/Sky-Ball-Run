using System;
using System.Collections;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class PrepareGameComponent : MonoBehaviour, IComponent, IConstructable
    {
        
        public delegate void PrepareGameChangeDelegate();

        public event PrepareGameChangeDelegate OnGameLaunch;

        [SerializeField] private AudioSourceController audioSourceControllerPrefab;

        public Action<string> SetLevelOnCanvas;
        public Action<string> DiamondChange;
        
        private InGameComponent inGameComponent;
        private DataComponent dataComponent;
        private AudioSourceController audioSourceController;
 
        
        


        public void Initialize(ComponentContainer componentContainer)
        {
            inGameComponent = componentContainer.GetComponent("InGameComponent") as InGameComponent;
            dataComponent=componentContainer.GetComponent("DataComponent") as DataComponent;
            
            AudioSourceControllerInitialize();
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
        }

        private void AudioSourceControllerInitialize()
        {
            audioSourceController = Instantiate(audioSourceControllerPrefab);
        }

        public void OnConstruct()
        {
            SetDiamond();
            StartCoroutine(PreparingGame());
        }

        private IEnumerator PreparingGame()
        {
            var level = GetLevel();
            SetLevelOnCanvas?.Invoke(level.ToString());
            yield return inGameComponent.StartCoroutine(inGameComponent.InitializeGame(level));
            OnGameLaunch?.Invoke();
        }

        private int GetLevel()
        {
            return dataComponent.LevelData.currentLevel;
        }
        private void SetDiamond()
        {
            var diamond = dataComponent.InventoryData.ownedDiamond;
            inGameComponent.SetupDiamond(diamond);
            DiamondChange?.Invoke(diamond.ToString());
        }
    }
}