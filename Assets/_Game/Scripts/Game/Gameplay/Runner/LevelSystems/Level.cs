using System;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner.Lines;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.LevelSystems
{
    [CreateAssetMenu(menuName = "Level", fileName = "LevelScript")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<LinesController> levels;
        [SerializeField] private EndGameController endGameControllerPrefab;

        public EndGameController EndGameControllerPrefab => endGameControllerPrefab;

        public List<LinesController> GetLevels() => levels;
        public int GetLevelCount() => levels.Count;
    }
}