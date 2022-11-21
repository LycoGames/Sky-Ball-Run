using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class LinesController : MonoBehaviour
    {
        [SerializeField] Line linePrefab;
        [SerializeField] private int linePoolCount;
        [SerializeField] private float lineForwardBound;
        [SerializeField] private Level.Level level;
        [SerializeField]private int showedLineCount;
        [SerializeField] private float height=-0.5f;
        
        private List<Transform> linesTransforms=new List<Transform>();
        private bool firstRun = true;
        private int currentLine;

        public IEnumerator InitializeLines()
        {
            SpawnLines();
            yield return null;
        }
        private void SwapLine(int index)
        {
            if(index+showedLineCount<linesTransforms.Count)
            linesTransforms[index+showedLineCount].gameObject.SetActive(true);
        }
        private void SpawnLines()
        {
            int i = 0;
            foreach (Level.Level.LineInteractables lineInteractable in level.GetLineInteractables())
            {
                Line spawnedLined = Instantiate(linePrefab);
                linesTransforms.Add(spawnedLined.transform);
                linesTransforms.Last().position = new Vector3(0, height, lineForwardBound * (linesTransforms.Count-1));
                spawnedLined.InitializeLine(lineInteractable.interactables, SwapLine, linesTransforms.Count - 1);
                if(i>=showedLineCount)spawnedLined.gameObject.SetActive(false);
                i++;
            }
        }
    }
}