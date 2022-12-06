using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleAdderGate : Gate
    {
        [SerializeField] private AdderType adderType;
        [Range(0, 100)] [SerializeField] private int maxAddPercentage;
        [SerializeField] private TextMeshProUGUI ballCountText;
        [SerializeField] private Gate ReverseGatePrefab;
        private int addSize = 1;
        private BallManager ballManager;
        private int currentAddPercentage;
        private Gate reverseGate;

        private void Start()
        {
            reverseGate = Instantiate(ReverseGatePrefab);
            reverseGate.transform.position = transform.position;
            reverseGate.transform.parent = transform.parent;
            reverseGate.gameObject.SetActive(false);
            OnEnterGate += AddScale;
        }

        private void OnEnable()
        {
            currentAddPercentage = UnityEngine.Random.Range(0, maxAddPercentage + 1);
            ballManager = BallManager.Instance;
            ballManager.OnGateCountCheck += StartChecking;
            StartChecking();
        }

        private void OnDisable()
        {
            ballManager.OnGateCountCheck -= StartChecking;
        }

        private void AddScale()
        {
            StopAllCoroutines();
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

        private void StartChecking()
        {
            Invoke("CheckSize", 1);
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
                    if(addSize+column>=ballManager.maxColumn)SwapGate();
                    writeSize = addSize;
                    writeSize *= floor * row;
                    break;
                case AdderType.UpAdder:
                    newRemoveSize = ballManager.currentFloor * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentFloor > ballManager.maxFloor)
                        addSize = ballManager.maxFloor - ballManager.currentFloor;
                    if (addSize <= 0) addSize = 1;
                    if(addSize+floor>=ballManager.maxFloor)SwapGate();
                    writeSize = addSize;
                    writeSize *= row * column;
                    break;
                case AdderType.LengthAdder:
                    newRemoveSize = ballManager.currentRow * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentRow > ballManager.maxRow)
                        addSize = ballManager.maxRow - ballManager.currentRow;
                    if (addSize <= 0) addSize = 1;
                    if(addSize+row>=ballManager.maxRow)SwapGate();
                    writeSize = addSize;
                    writeSize *= floor * column;
                    break;
            }

            writeSize += totalCubicBallCount - ballManager.TotalBallCount;
            ballCountText.text = "+" + writeSize;
        }

        private void SwapGate()
        {
            reverseGate.gameObject.SetActive(true);
            myDoubleGate.SwapGate(this,reverseGate);
            gameObject.SetActive(false);
        }

        public enum AdderType
        {
            RightAdder,
            UpAdder,
            LengthAdder
        }
    }
}