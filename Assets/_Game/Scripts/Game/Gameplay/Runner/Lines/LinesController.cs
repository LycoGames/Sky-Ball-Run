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

        [SerializeField] private List<Transform> linesTransforms;
        private int currentLine;

        private void Start()
        {
            StartCoroutine(InitiliazeLines());
        }

        IEnumerator InitiliazeLines()
        {
            yield return new WaitForSeconds(2f);
            int counter = showedLineCount;
            foreach (Transform lineTransform in linesTransforms)
            {
                lineTransform.GetComponent<Line>().InitializeLine(SwapLine,linesTransforms.IndexOf(lineTransform));
                if (counter >= 0)
                {
                    lineTransform.gameObject.SetActive(true);
                    counter--;
                }
                
            }
        }

        private void SwapLine(int index)
        {
            if (index + showedLineCount < linesTransforms.Count)
                linesTransforms[index + showedLineCount].gameObject.SetActive(true);
        }
    }
}