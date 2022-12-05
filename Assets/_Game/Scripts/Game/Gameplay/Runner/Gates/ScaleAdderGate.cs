using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleAdderGate : MonoBehaviour
    {
        [SerializeField] private AdderGateSpecs selectedGate;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private float xPos;
        [SerializeField] private TextMeshProUGUI ballCountText;
        [SerializeField] private float checkTime = 2f;
        private int addSize=1;
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
            wfsForCheckSize = new WaitForSeconds(checkTime);
            Vector3 newPos = transform.position;
            newPos.x = xPos;
            transform.position = newPos;
            ballCountText.text = "+" + addSize;
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.CompareTag("Ball"))
            {
                StopAllCoroutines();
                boxCollider.enabled = false;
                switch (selectedGate.adderType)
                {
                    case AdderType.RightAdder:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.RightAdder(addSize));
                        break;
                    case AdderType.UpAdder:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.UpAdder(addSize));
                        break;
                    case AdderType.LengthAdder:
                        BallManager.Instance.StartCoroutine(BallManager.Instance.LengthAdder(addSize));
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
                    case AdderType.RightAdder:
                        newRemoveSize = ballManager.currentColumn * ((float)selectedGate.addPercentage/100);
                        addSize=(int)Math.Round(newRemoveSize);
                        if (addSize+ballManager.currentColumn > ballManager.maxColumn) addSize = ballManager.maxColumn-ballManager.currentColumn;
                        if (addSize <= 0) addSize = 1;
                        writeSize = addSize;
                        writeSize *= floor*row;
                        break;
                    case AdderType.UpAdder:
                        newRemoveSize = ballManager.currentFloor * ((float)selectedGate.addPercentage/100);
                        addSize=(int)Math.Round(newRemoveSize);
                        if (addSize+ballManager.currentFloor > ballManager.maxFloor) addSize = ballManager.maxFloor-ballManager.currentFloor;
                        if (addSize <= 0) addSize = 1;
                        writeSize = addSize;
                        writeSize *= row*column;
                        break;
                    case AdderType.LengthAdder:
                        newRemoveSize = ballManager.currentRow * ((float)selectedGate.addPercentage/100);
                        addSize=(int)Math.Round(newRemoveSize);
                        if (addSize+ballManager.currentRow > ballManager.maxRow) addSize = ballManager.maxRow-ballManager.currentRow;
                        if (addSize <= 0) addSize = 1;
                        writeSize = addSize;
                        writeSize *= floor*column;
                        break;
                }
                writeSize += totalCubicBallCount-ballManager.TotalBallCount;
                ballCountText.text = "+" + writeSize;
                yield return wfsForCheckSize;
            }
        }
        [Serializable]
        public struct AdderGateSpecs
        {
            public AdderType adderType;
            public int addPercentage;
        }

        public enum AdderType
        {
            RightAdder,
            UpAdder,
            LengthAdder
        }
    }
}
