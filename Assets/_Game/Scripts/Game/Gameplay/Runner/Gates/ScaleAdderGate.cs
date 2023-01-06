using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleAdderGate : Gate
    {
        [SerializeField] private AdderType adderType;
        [Range(0, 399)] [SerializeField] private int minAddPercentage;
        [Range(0, 400)] [SerializeField] private int maxAddPercentage;
        [SerializeField] private TMP_Text ballCountText;
        [SerializeField] private Gate ReverseGatePrefab;
        [SerializeField] private float distance = 8f;
        [SerializeField] private float checkTime = 1f;
        [SerializeField] private bool neverReturn;

        private WaitForSeconds wfsCheckTime;
        private int addSize = 1;
        private BallManager ballManager;
        private int currentAddPercentage;
        private Gate reverseGate;

        private void Start()
        {
            wfsCheckTime = new WaitForSeconds(checkTime);
            CreateReverseGate();
            OnEnterGate += AddScale;
            OnDisableGate += RemoveChecking;
        }

        private void OnEnable()
        {
            Debug.Log("Gate Activated");
            ballCountText.text = "";
            ballCountText.enabled = false;
            currentAddPercentage = UnityEngine.Random.Range(minAddPercentage, maxAddPercentage + 1);
            ballManager = BallManager.Instance;
            StartCoroutine(DistanceCheck());
            ballManager.OnGateCountCheck += StartChecking;
        }

        private void OnDisable()
        {
            Debug.Log("Gate Deactivated");
            RemoveChecking();
            StopAllCoroutines();
        }

        private void AddScale()
        {
            RemoveChecking();
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

        private void RemoveChecking()
        {
            ballManager.OnGateCountCheck -= StartChecking;
        }

        private IEnumerator DistanceCheck()
        {
            while (Vector3.Distance(ballManager.transform.position, transform.position) > distance)
                yield return wfsCheckTime;
            ballCountText.enabled = true;
            CheckSize();
        }

        private void StartChecking()
        {
            Invoke("CheckSize", .25f);
        }

        private void CheckSize()
        {
            if (ballManager.TotalBallCount <= 0 || !canCheckSize) return;
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
                    if (addSize + column >= ballManager.maxColumn)
                    {
                        if (ballManager.maxColumn <= column && !neverReturn)
                        {
                            SwapGate();
                            return;
                        }

                        addSize = ballManager.maxColumn - column;
                    }

                    writeSize = addSize;
                    writeSize *= floor * row;
                    break;
                case AdderType.UpAdder:
                    newRemoveSize = ballManager.currentFloor * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentFloor > ballManager.maxFloor)
                        addSize = ballManager.maxFloor - ballManager.currentFloor;
                    if (addSize <= 0) addSize = 1;
                    if (addSize + floor >= ballManager.maxFloor)
                    {
                        if (ballManager.maxFloor <= floor && !neverReturn)
                        {
                            SwapGate();
                            return;
                        }

                        addSize = ballManager.maxFloor - floor;
                    }

                    writeSize = addSize;
                    writeSize *= row * column;
                    break;
                case AdderType.LengthAdder:
                    newRemoveSize = ballManager.currentRow * ((float)currentAddPercentage / 100);
                    addSize = (int)Math.Round(newRemoveSize);
                    if (addSize + ballManager.currentRow > ballManager.maxRow)
                        addSize = ballManager.maxRow - ballManager.currentRow;
                    if (addSize <= 0) addSize = 1;
                    if (addSize + row >= ballManager.maxRow)
                    {
                        if (ballManager.maxRow <= row && !neverReturn)
                        {
                            SwapGate();
                            return;
                        }

                        addSize = ballManager.maxRow - row;
                    }

                    writeSize = addSize;
                    writeSize *= floor * column;
                    break;
            }

            writeSize += totalCubicBallCount - ballManager.TotalBallCount;
            if (writeSize < 0)
            {
                Invoke("CheckSize", .1f);
                return;
            }

            ballCountText.text = "+" + writeSize;
        }

        private void SwapGate()
        {
            reverseGate.gameObject.SetActive(true);
            if (myDoubleGate != null) myDoubleGate.SwapGate(this, reverseGate);
            if (myAnimator.enabled) reverseGate.EnableMyAnimator();
            gameObject.SetActive(false);
        }
        private void CreateReverseGate()
        {
            reverseGate = Instantiate(ReverseGatePrefab);
            reverseGate.transform.parent = transform.parent;
            reverseGate.gameObject.SetActive(false);
            reverseGate.transform.localPosition = transform.localPosition;
        }

        public enum AdderType
        {
            RightAdder,
            UpAdder,
            LengthAdder
        }
    }
}