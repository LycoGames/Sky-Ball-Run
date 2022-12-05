using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class DoubleAdderGate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI leftCount;
        [SerializeField] private TextMeshProUGUI rightCount;
        [SerializeField] private AdderGateSpecs leftGate;
        [SerializeField] private AdderGateSpecs rightGate;

        private BallManager ballManager;
        private int row, column, floor, totalCubicBallCount;
        
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

        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            StartCoroutine(CheckSize());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator CheckSize()
        {
            
            while (true)
            {
                CheckBallSize();
                GateTextWriter(leftCount, leftGate);
                GateTextWriter(rightCount,rightGate);
                yield return null;
            }
        }

        private void CheckBallSize()
        {
            row = ballManager.currentRow;
            column = ballManager.currentColumn;
            floor = ballManager.currentFloor;
            totalCubicBallCount = (row * column * floor)-ballManager.TotalBallCount;
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

        private void GateTextWriter(TextMeshProUGUI text, AdderGateSpecs specs)
        {
            switch (specs.adderType)
            {
                case AdderType.RightAdder:
                    text.text = (specs.addSize * row * floor+totalCubicBallCount).ToString();
                    break;
                case AdderType.UpAdder:
                    text.text = (specs.addSize * row * column+totalCubicBallCount).ToString();
                    break;
            }
        }
    }
}