using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.LevelSystems;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class LinesController : MonoBehaviour
    {
        [SerializeField] private int showedLineCount;

        [SerializeField] private List<Line> lineList;
        private int currentLine;

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

        private void SwapLine(int index)
        {
            if (index + showedLineCount < lineList.Count)
                lineList[index + showedLineCount].gameObject.SetActive(true);
        }
    }
}