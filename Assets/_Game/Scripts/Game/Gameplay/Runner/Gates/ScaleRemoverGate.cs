using System;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;


namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class ScaleRemoverGate : Gate
    {
        [SerializeField] public AdderType adderType;
        [Range(0, 100)] [SerializeField] private int maxRemovePercentage;
        [SerializeField] private TextMeshProUGUI sizeText;
        private int currentRemovePercentage;
        private int removeSize = 1;
        private BallManager ballManager;

        private void Start()
        {
            OnEnterGate += RemoveScale;
        }

        private void OnEnable()
        {
            currentRemovePercentage = UnityEngine.Random.Range(0, maxRemovePercentage + 1);
            ballManager = BallManager.Instance;
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

        private void RemoveScale()
        {
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

        private void CheckSize()
        {
            float newRemoveSize;
            int writeSize = 0;

            switch (adderType)
            {
                case AdderType.WidthRemover:
                    newRemoveSize = ballManager.currentColumn * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    removeSize = (int)Math.Round(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedColumn(removeSize);
                    break;
                case AdderType.HeightRemover:
                    newRemoveSize = ballManager.currentFloor * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    removeSize = (int)Math.Round(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedFloor(removeSize);
                    break;
                case AdderType.LengthRemover:
                    newRemoveSize = ballManager.currentRow * ((float)currentRemovePercentage / 100);
                    if (newRemoveSize < 1) newRemoveSize = 1;
                    removeSize = (int)Math.Round(newRemoveSize);
                    writeSize = ballManager.GetBallCountOnRemovedRow(removeSize);
                    break;
            }

            sizeText.text = "-" + writeSize;
        }


        public enum AdderType
        {
            WidthRemover,
            HeightRemover,
            LengthRemover
        }
    }
}