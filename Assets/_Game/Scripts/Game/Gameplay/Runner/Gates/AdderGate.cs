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
        private bool isFirstCall;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") & !isFirstCall)
            {
                AdderGateSpecs selectedGate = other.transform.position.x >= 0 ? rightGate : leftGate;
                if (selectedGate.adderType == AdderType.RightAdder)
                {
                    BallManager.Instance.StartCoroutine(BallManager.Instance.RightAdder(selectedGate.addSize));
                }
                else
                {
                    BallManager.Instance.StartCoroutine(BallManager.Instance.UpAdder(selectedGate.addSize));
                }

                gameObject.SetActive(false);
            }
        }
    }

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
}