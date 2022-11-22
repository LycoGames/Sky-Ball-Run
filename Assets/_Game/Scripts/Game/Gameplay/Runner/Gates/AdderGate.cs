using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class AdderGate : MonoBehaviour
    {
        [SerializeField] private AdderGateSpecs leftGate;
        [SerializeField] private AdderGateSpecs rightGate;

        [Serializable]
        public struct AdderGateSpecs
        {
            public AdderType adderType;
            public int addSize;
        }

        public enum AdderType
        {
            RightAdder,
            UpAdder
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AdderGateSpecs selectedGate = other.transform.position.x >= 0 ? rightGate : leftGate;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightAdder:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.RightAdder(selectedGate.addSize));
                        break;
                    case AdderType.UpAdder:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.UpAdder(selectedGate.addSize));
                        break;
                }
                gameObject.SetActive(false);
            }
        }
    }
    
}