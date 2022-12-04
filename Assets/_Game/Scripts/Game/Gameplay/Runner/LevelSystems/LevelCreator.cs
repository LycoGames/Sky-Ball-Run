using System;
using System.Collections;
using _Game.Scripts.Game.Components;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.LevelSystems
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private Level level;

        private LevelSpecs levelSpecs;
        private LinesController createdLinesController;
        private int currentLevel = 0;
        private int LevelCount;
        
        public EndGameController EndGameController { get; private set; }
        public LevelSpecs LevelSpecs() => levelSpecs;
        
        public IEnumerator CreateLevel(int loadedLevel)
        {
            LevelCount = level.GetLevelCount();
            if (loadedLevel >= LevelCount) loadedLevel %= LevelCount;
            levelSpecs = level.GetLevels()[loadedLevel];
            createdLinesController=Instantiate(levelSpecs.linesController);
            EndGameController = createdLinesController.EndGameController;
            yield return StartCoroutine(createdLinesController.InitializeLines());
        }

        public void DestroyLevel()
        {
            Destroy(createdLinesController.gameObject);
        }
    }
}