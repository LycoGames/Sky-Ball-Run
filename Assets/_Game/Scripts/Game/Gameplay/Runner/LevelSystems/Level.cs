using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.LevelSystems
{
    [CreateAssetMenu(menuName = "Level", fileName = "LevelScript")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<LineInteractables> lineInteractables;
        public List<LineInteractables> GetLineInteractables() => lineInteractables;
        
        [Serializable]
        public class LineInteractables
        {
            public List<GameObject> interactables;
        }
    }
}
