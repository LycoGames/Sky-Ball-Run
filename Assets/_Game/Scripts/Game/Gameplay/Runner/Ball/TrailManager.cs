using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class TrailManager : MonoBehaviour
    {
        
        [SerializeField] private RemoveProcess removeProcess;
        [SerializeField] private List<Trail> trails;
        private bool checkObstacle = true;
        
        private void Start()
        {
            removeProcess = Instantiate(removeProcess);
            removeProcess.Initialize(this);
        }

        public void StartRemoveProcess()
        {
            foreach (Trail trail in trails)
            {
                trail.StartRemoveProcess();
                checkObstacle = true;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle")&&checkObstacle)
            {
                removeProcess.isFollow = true;
                checkObstacle = false;
            }
        }
    }
}
