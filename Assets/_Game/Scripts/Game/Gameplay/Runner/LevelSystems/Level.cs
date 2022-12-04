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

        public List<LevelSpecs> GetLevels() => levelSpecs;
        public int GetLevelCount() => levelSpecs.Count;
    }
    [Serializable]
    public struct LevelSpecs
    {
        public LinesController linesController;
        public Color ballColors;
        public int column;
        public int row;
        public int floor;
    }
}