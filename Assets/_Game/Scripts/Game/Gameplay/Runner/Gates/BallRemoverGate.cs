using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallRemoverGate : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [Range(0,100)][SerializeField] private int maxRemovePercentage;
        [SerializeField] private TextMeshProUGUI ballCountText;
        private int removeSize = 1;
        private BallManager ballManager;
        private int currentRemovePercentage;
        private void OnEnable()
        {
            currentRemovePercentage = UnityEngine.Random.Range(0,maxRemovePercentage + 1);
            ballManager = BallManager.Instance;
            ballManager.OnTotalBallCountChange += CheckSize;
            CheckSize(0);
        }

        private void OnDisable()
        {
            ballManager.OnTotalBallCountChange -= CheckSize;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                myCollider.enabled = false;
                BallManager.Instance.StartCoroutine(BallManager.Instance.RemoveBall(removeSize));
                gameObject.SetActive(false);
            }
        }

        private void CheckSize(int x)
        {
            float newRemoveSize = ballManager.TotalBallCount * ((float)currentRemovePercentage / 100);
            removeSize = (int)Math.Round(newRemoveSize);
            if (removeSize <= 0) removeSize = 1;
            ballCountText.text = "-" + removeSize;
        }
    }
}