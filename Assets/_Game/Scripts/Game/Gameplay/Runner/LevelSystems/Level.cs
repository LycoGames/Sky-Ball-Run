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
        [SerializeField] private List<LevelSpecs> levelSpecs;
        [SerializeField] private List<EndGameController> endGames;
        public List<LevelSpecs> GetLevels() => levelSpecs;
        public List<EndGameController> GetEndGames() => endGames;
        public int GetLevelCount() => levelSpecs.Count;
    }
    [Serializable]
    public struct LevelSpecs
    {
        public LinesController linesController;
    }
}