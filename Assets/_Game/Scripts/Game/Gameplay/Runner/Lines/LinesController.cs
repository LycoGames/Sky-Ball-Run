using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.EndGames;
using _Game.Scripts.Game.Gameplay.Runner.LevelSystems;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class LinesController : MonoBehaviour
    {
        [SerializeField] private int showedLineCount;

        [SerializeField] private List<Line> lineList;
        private int currentLine;
        
        [SerializeField] private EndGameController endGameController;

        public EndGameController EndGameController => endGameController;

        public IEnumerator InitializeLines()
        {
            yield return new WaitForSeconds(2f);
            int counter = showedLineCount;
            foreach (var line in lineList)
            {
                line.InitializeLine(SwapLine, lineList.IndexOf(line));
                if (counter >= 0)
                {
                    line.gameObject.SetActive(true);
                    counter--;
                }
            }
        }

        public Transform GetLastLine()
        {
            return lineList.Last().transform;
        }

        private void SwapLine(int index)
        {
            if(index>0)lineList[index-1].gameObject.SetActive(false);
            if (index + showedLineCount < lineList.Count) lineList[index + showedLineCount].gameObject.SetActive(true);
            else if(index + showedLineCount == lineList.Count)endGameController.gameObject.SetActive(true);
        }
    }
}