using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleRemoverGate : MonoBehaviour
    {
        [SerializeField] private AdderGateSpecs selectedGate;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private TextMeshProUGUI sizeText;
        [SerializeField] private float checkTime = 2f;
        private int removeSize=1;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;

        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            StartCoroutine(CheckSize());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            print("bbb");
            wfsForCheckSize = new WaitForSeconds(checkTime);
            // sizeText.text = "-" + removeSize;
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.CompareTag("Ball"))
            {
                boxCollider.enabled = false;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.RightRemover(removeSize));
                        break;
                    case AdderType.UpRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.UpRemover(removeSize));
                        break;
                    case AdderType.LengthRemover:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.LengthRemover(removeSize));
                        break;
                }
                gameObject.SetActive(false);
            }
        }
        
        private IEnumerator CheckSize()
        {
            float newRemoveSize=0;
            int row;
            int column;
            int floor;
            int totalCubicBallCount;
            int writeSize=0;
            while (true)
            {
                row = ballManager.currentRow;
                column = ballManager.currentColumn;
                floor = ballManager.currentFloor;
                totalCubicBallCount = row * column * floor;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        newRemoveSize = ballManager.currentColumn * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        writeSize = (int)Math.Round(newRemoveSize * row * floor);
                        break;
                    case AdderType.UpRemover:
                        newRemoveSize = ballManager.currentFloor * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        writeSize = (int)Math.Round(newRemoveSize * row * column);
                        break;
                    case AdderType.LengthRemover:
                        newRemoveSize = ballManager.currentRow * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        writeSize = (int)Math.Round(newRemoveSize * column * floor);
                        break;
                }
                writeSize -= totalCubicBallCount;
                removeSize=(int)Math.Round(newRemoveSize);
                sizeText.text = "-" + writeSize;
                yield return wfsForCheckSize;
            }
        }
        [Serializable]
        public struct AdderGateSpecs
        {
            public AdderType adderType;
            public int removePercentage;
        }

        public enum AdderType
        {
            RightRemover,
            UpRemover,
            LengthRemover
        }
    }
}
