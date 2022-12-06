using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;



namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleAdderGate : MonoBehaviour
    {
        [SerializeField] private AdderType adderType;
        [Range(0,100)][SerializeField] private int maxAddPercentage;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private TextMeshProUGUI ballCountText;
        private int addSize = 1;
        private BallManager ballManager;
        private int currentAddPercentage;
        private void OnEnable()
        {
            currentAddPercentage = UnityEngine.Random.Range(0, maxAddPercentage+1);
            ballManager = BallManager.Instance;
            ballManager.OnGateCountCheck += StartChecking;
            StartChecking();
        }

        private void OnDisable()
        {
            ballManager.OnGateCountCheck -= StartChecking;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                StopAllCoroutines();
                boxCollider.enabled = false;
                switch (adderType)
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

        private void StartChecking()
        {
            Invoke("CheckSize", .1f);
        }

        private void CheckSize()
        {
            float newRemoveSize = 0;
            int writeSize = 0;

            int row = ballManager.currentRow;
            int column = ballManager.currentColumn;
            int floor = ballManager.currentFloor;
            int totalCubicBallCount = row * column * floor;

            switch (adderType)
            {
                case AdderType.RightAdder:
                    newRemoveSize = ballManager.currentColumn * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentColumn > ballManager.maxColumn)
                        addSize = ballManager.maxColumn - ballManager.currentColumn;
                    if (addSize <= 0) addSize = 1;
                    writeSize = addSize;
                    writeSize *= floor * row;
                    break;
                case AdderType.UpAdder:
                    newRemoveSize = ballManager.currentFloor * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentFloor > ballManager.maxFloor)
                        addSize = ballManager.maxFloor - ballManager.currentFloor;
                    if (addSize <= 0) addSize = 1;
                    writeSize = addSize;
                    writeSize *= row * column;
                    break;
                case AdderType.LengthAdder:
                    newRemoveSize = ballManager.currentRow * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentRow > ballManager.maxRow)
                        addSize = ballManager.maxRow - ballManager.currentRow;
                    if (addSize <= 0) addSize = 1;
                    writeSize = addSize;
                    writeSize *= floor * column;
                    break;
            }

            writeSize += totalCubicBallCount - ballManager.TotalBallCount;
            ballCountText.text = "+" + writeSize;
        }


        public enum AdderType
        {
            RightAdder,
            UpAdder,
            LengthAdder
        }
    }
}