using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleRemoverGate : Gate
    {
        [SerializeField] public AdderType adderType;
        [Range(0, 99)] [SerializeField] private int minRemovePercentage;
        [Range(0, 100)] [SerializeField] private int maxRemovePercentage;
        [SerializeField] private TMP_Text ballCountText;
        [SerializeField] private float distance = 8f;
        [SerializeField] private float checkTime = 1f;

        private WaitForSeconds wfsCheckTime;
        private int currentRemovePercentage;
        private int removeSize = 1;
        private BallManager ballManager;

        private void Start()
        {
            wfsCheckTime = new WaitForSeconds(checkTime);
            OnEnterGate += RemoveScale;
            OnDisableGate += RemoveChecking;
        }

        private void OnEnable()
        {
            Debug.Log("Gate Activated");
            removeSize=Int32.MaxValue;
            ballCountText.text = "";
            ballCountText.enabled = false;
            currentRemovePercentage = UnityEngine.Random.Range(minRemovePercentage, maxRemovePercentage + 1);
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

        private void RemoveScale()
        {
            RemoveChecking();
            switch (adderType)
            {
                case AdderType.WidthRemover:
                    BallManager.Instance.StartCoroutine(BallManager.Instance.RightRemover(removeSize));
                    break;
                case AdderType.HeightRemover:
                    BallManager.Instance.StartCoroutine(BallManager.Instance.UpRemover(removeSize));
                    break;
                case AdderType.LengthRemover:
                    BallManager.Instance.StartCoroutine(BallManager.Instance.LengthRemover(removeSize));
                    break;
            }
            gameObject.SetActive(false);
        }

        private void RemoveChecking()
        {
            ballManager.OnGateCountCheck -= StartChecking;
        }

        private void CheckSize()
        {
            if (ballManager.TotalBallCount <= 0||!canCheckSize) return;
            float newRemoveSize;
            int writeSize = 0;

            switch (adderType)
            {
                case AdderType.WidthRemover:
                    newRemoveSize = ballManager.currentColumn * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    SetRemoveSize(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedColumn(removeSize);
                    break;
                case AdderType.HeightRemover:
                    newRemoveSize = ballManager.currentFloor * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    SetRemoveSize(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedFloor(removeSize);
                    break;
                case AdderType.LengthRemover:
                    newRemoveSize = ballManager.currentRow * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    SetRemoveSize(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedRow(removeSize);
                    break;
            }

            if (writeSize <= 0)
            {
                Invoke("CheckSize", .1f);
                return;
            }

            ballCountText.text = "-" + writeSize;
        }

        private void SetRemoveSize(float newRemoveSize)
        {
            if (removeSize <= newRemoveSize) return;
            removeSize = (int)Math.Round(newRemoveSize);
        }


        public enum AdderType
        {
            WidthRemover,
            HeightRemover,
            LengthRemover
        }
    }
}