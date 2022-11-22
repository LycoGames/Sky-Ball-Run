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
        [SerializeField] private GameObject gateModels;

        private float ballMultiplier=1;
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
                DisableModels();
            }
        }
        private void DisableModels()
        {
            gateModels.SetActive(false);
        }
        private void DisableGate()
        {
            gameObject.SetActive(false);
        }

        private void SetGates(GateSpecs gateSpecs)
        {
            int newFloor=1;
            int newColumn=1;
            switch (gateSpecs.gateType)
            {
                case GateType.Vertical:
                    newFloor = (int)ballMultiplier * gateSpecs.newIndex;
                    newColumn = SetAnotherIndex(gateSpecs.newIndex);
                    break;
                case GateType.Horizontal:
                    newColumn = (int)ballMultiplier * gateSpecs.newIndex;
                    newFloor = SetAnotherIndex(gateSpecs.newIndex);
                    break;
            }
            BallManager.Instance.OnEnterGate(newColumn, newFloor,DisableGate);
        }
        private int SetAnotherIndex(int index)
        {
            int div = BallManager.Instance.maxRow * index;
            int ballCount = BallManager.Instance.totalBallCount;
            return ballCount / div + (ballCount % div == 0 ? 0 : 1);
        }
    }
    
}

