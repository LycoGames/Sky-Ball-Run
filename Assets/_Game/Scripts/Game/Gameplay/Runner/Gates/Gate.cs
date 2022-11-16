using System;
using _Game.Scripts.Game.Gameplay.Runner.Ball;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private GateSpecs leftGate;
        [SerializeField] private GateSpecs rightGate;
        private bool isActive = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&&isActive)
            {
                GateSpecs selectedGate = other.transform.position.x >= 0 ? rightGate : leftGate;
                BallManager.ballManager.OnEnterGate(selectedGate);
                leftGate.gate.SetActive(false);
                rightGate.gate.SetActive(false);
                isActive = false;
            }
        }
    }

    [System.Serializable]
    public struct GateSpecs
    {
        public GameObject gate;
        public int newFloor;
        public int newColumn;
    }
}
