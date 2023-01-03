using System;
using System.Collections;
using System.Collections.Generic;
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
        private int loadedLevel;

        public EndGameController EndGameController { get; private set; }
        public LevelSpecs LevelSpecs() => levelSpecs;

        public void OnInstantiate(int _loadedLevel)
        {
            loadedLevel = _loadedLevel;
            int LevelCount = level.GetLevelCount();
            if (loadedLevel >= LevelCount) loadedLevel %= LevelCount;
            levelSpecs = level.GetLevels()[loadedLevel];
        }

        public IEnumerator CreateLevel()
        {
            createdLinesController=Instantiate(levelSpecs.linesController);
            SetEndGame();
            yield return StartCoroutine(createdLinesController.InitializeLines(EndGameController));
        }

        public void DestroyLevel()
        {
            Destroy(createdLinesController.gameObject);
        }

        private void SetEndGame()
        {
            List<EndGameController> endGames = level.GetEndGames();
            int index = loadedLevel % endGames.Count;
            EndGameController = Instantiate(endGames[index]);
        }
    }
}