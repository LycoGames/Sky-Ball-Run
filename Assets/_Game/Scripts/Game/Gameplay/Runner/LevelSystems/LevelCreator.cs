using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.LevelSystems
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private Level level;

        private LinesController linesController;
        private int currentLevel = 0;
        private int LevelCount;
        public EndGameController EndGameController => level.EndGameController;

        private void Start()
        {
            LevelCount = level.GetLevelCount();
        }

        public IEnumerator CreateLevel()
        {
            int loadedLevel = currentLevel;
            if (loadedLevel > LevelCount) loadedLevel %= LevelCount;
            linesController = Instantiate(level.GetLevels()[loadedLevel]);
            currentLevel++;
            yield return StartCoroutine(linesController.InitializeLines());
        }

        public void DestroyLevel()
        {
            Destroy(linesController);
        }
    }
}