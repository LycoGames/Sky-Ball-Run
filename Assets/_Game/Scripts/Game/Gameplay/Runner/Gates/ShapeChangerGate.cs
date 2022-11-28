using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ShapeChangerGate : MonoBehaviour
    {
        
        [SerializeField] private GateSpecs leftGate;
        [SerializeField] private GateSpecs rightGate;
        
        private bool isActive = true;
        
        [Serializable]
        public struct GateSpecs
        {
            public GateType gateType;
            public int newIndex;
        }
        public enum GateType
        {
            Horizontal,Vertical
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isActive)
            {
                isActive = false;
                GateSpecs selectedGate = other.transform.position.x >= 0 ? rightGate : leftGate;
                SetGates(selectedGate);
                DisableGate();
            }
        }
        private void DisableGate()
        {
            gameObject.SetActive(false);
        }

        private void SetGates(GateSpecs gateSpecs)
        {
            switch (gateSpecs.gateType)
            {
                case GateType.Vertical:
                    BallManager.Instance.ReshapeTaller(gateSpecs.newIndex);
                    break;
                case GateType.Horizontal:
                    BallManager.Instance.ReshapeWider(gateSpecs.newIndex);
                    break;
            }
        }
    }
    
}

