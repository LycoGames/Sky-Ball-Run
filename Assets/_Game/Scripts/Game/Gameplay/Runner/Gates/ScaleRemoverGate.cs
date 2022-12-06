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
        private int removeSize=1;
        private BallManager ballManager;

        private void OnEnable()
        {
            ballManager=BallManager.Instance;
            ballManager.OnGateCountCheck += StartChecking;
            StartChecking();
        }

        private void OnDisable()
        {
            ballManager.OnGateCountCheck -= StartChecking;
        }
        

        private void StartChecking()
        {
            Invoke("CheckSize", .1f);
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
        
        private void CheckSize()
        {
            float newRemoveSize=0;
            int writeSize=0;
        
                int row = ballManager.currentRow;
                int column = ballManager.currentColumn;
                int floor = ballManager.currentFloor;
                int totalCubicBallCount = row * column * floor;
                int totalBallCount = ballManager.TotalBallCount;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightRemover:
                        newRemoveSize = ballManager.currentColumn * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        removeSize=(int)Math.Round(newRemoveSize);
                        writeSize = removeSize * row * floor;
                        break;
                    case AdderType.UpRemover:
                        newRemoveSize = ballManager.currentFloor * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        removeSize=(int)Math.Round(newRemoveSize);
                        writeSize = removeSize * row * column;
                        break;
                    case AdderType.LengthRemover:
                        newRemoveSize = ballManager.currentRow * ((float)selectedGate.removePercentage/100);
                        if (newRemoveSize <= 1) newRemoveSize=1;
                        writeSize = removeSize * column * floor;
                        break;
                }
                writeSize -= totalCubicBallCount-totalBallCount;
                if (selectedGate.adderType == AdderType.UpRemover)
                {
                    Debug.Break();
                    Debug.Log(writeSize);
                }
                sizeText.text = "-" + writeSize;

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
