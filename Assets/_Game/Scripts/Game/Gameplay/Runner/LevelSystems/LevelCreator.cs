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
        [SerializeField] private int showedLineCount;
        [SerializeField] private int easyLevelPartEnd=6;

        [Space][Header("BonusLevel")][Space]
        [SerializeField] private int bonusLevel = 6;
        [SerializeField] private EndGameController bonusLevelEndGame;

        private LevelSpecs levelSpecs;
        private LinesController createdLinesController;
        private int currentLevel;

        public EndGameController EndGameController { get; private set; }
        public BallSpecs BallSpecs() => levelSpecs.linesController.GetBallSpecs();

        public void OnInstantiate(int _currentLevel)
        {
            currentLevel = _currentLevel;
            int levelCount = level.GetLevelCount();
            int levelIndex = currentLevel;
            if (levelIndex >= levelCount)
            {
                 levelIndex = currentLevel % levelCount + easyLevelPartEnd;
                 if (levelIndex >= levelCount)
                 {
                     levelIndex = levelIndex % levelCount + easyLevelPartEnd;
                 }
            }
            levelSpecs = level.GetLevels()[levelIndex];
            Debug.Log("Loaded Index: "+levelIndex);
        }

        public IEnumerator CreateLevel()
        {
            levelSpecs.linesController.DisableAllLines();
            createdLinesController=Instantiate(levelSpecs.linesController);
            SetEndGame();
            yield return StartCoroutine(createdLinesController.InitializeLines(EndGameController,showedLineCount));
        }

        public void DestroyLevel()
        {
            Destroy(createdLinesController.gameObject);
        }

        private void SetEndGame()
        {
            if ((currentLevel + 1) % bonusLevel == 0)
            {
                EndGameController = Instantiate(bonusLevelEndGame);
                return;
            }
            List<EndGameController> endGames = level.GetEndGames();
            int index = currentLevel % endGames.Count;
            EndGameController = Instantiate(endGames[index]);
        }
    }
}