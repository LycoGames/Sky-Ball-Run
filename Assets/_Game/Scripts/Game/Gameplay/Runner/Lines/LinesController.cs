using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class LinesController : MonoBehaviour
    {
        [SerializeField] Line linePrefab;
        [SerializeField] private int linePoolCount;
        [SerializeField] private float lineForwardBound;
        [SerializeField] private List<Transform> linesTransforms;
        private bool firstRun = true;
        private int currentLine;

        private void SwapLine()
        {
            linesTransforms[currentLine].position += linesTransforms[currentLine].forward * lineForwardBound * linePoolCount;
            currentLine=currentLine >= linePoolCount-1?0:currentLine+1;
        }

        private void OnEnable()
        {
            if(firstRun)SpawnLines();
            firstRun = false;//todo refactor
        }

        void OnDisable()
        {
            ResetLinePos();
        }

        private void ResetLinePos()
        {
            var transformParent = transform;
            for (int i = 0; i < linePoolCount; i++)
            {
                linesTransforms[i].position = transformParent.position + (transformParent.forward * i * lineForwardBound);
                currentLine = 0;
            }
        }

        private void SpawnLines()
        {
            var transformParent = transform;
            for (int i = 0; i < linePoolCount; i++)
            {
                Line line = Instantiate(linePrefab,
                    transformParent.position + (transformParent.forward * i * lineForwardBound),
                    transformParent.rotation,
                    transformParent);
                line.OnLinePassed += SwapLine;
                linesTransforms.Add(line.transform);
            }
        }
    }
}