using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
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
                BallManager.ballManager.OnEnterGate(selectedGate,DisablingGate);
                leftGate.gate.SetActive(false);
                rightGate.gate.SetActive(false);
                isActive = false;
            }
        }

        private void DisablingGate()
        {
            gameObject.SetActive(false);
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
