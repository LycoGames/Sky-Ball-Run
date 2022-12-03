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
        public EndGameController EndGameControllerPrefab => level.EndGameControllerPrefab;
        public EndGameController EndGameController { get; private set; }

        private void Start()
        {
            LevelCount = level.GetLevelCount();
        }

        public IEnumerator CreateLevel()
        {
            int loadedLevel = currentLevel;
            if (loadedLevel > LevelCount) loadedLevel %= LevelCount;
            linesController = Instantiate(level.GetLevels()[loadedLevel]);
            Transform lastLineTransform = linesController.GetLastLine();
            EndGameController = Instantiate(EndGameControllerPrefab,
                EndGameControllerPrefab.transform.position + lastLineTransform.position,
                lastLineTransform.rotation, linesController.transform);
            currentLevel++;
            yield return StartCoroutine(linesController.InitializeLines());
        }

        public void DestroyLevel()
        {
            Destroy(linesController.gameObject);
        }
    }
}