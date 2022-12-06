using System;
using System.Collections;
using _Game.Scripts.Game.Gameplay.Runner.BallPositioning;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class BallAdderGate : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private int addPercentage;
        [SerializeField] private TextMeshProUGUI ballCountText;
        private int addSize = 1;
        private BallManager ballManager;

        private void OnEnable()
        {
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
                collider.enabled = false;
                BallManager.Instance.AddBall(addSize);
                gameObject.SetActive(false);
            }
        }

        private void CheckSize(int x)
        {
            float newAddSize = ballManager.TotalBallCount * ((float)addPercentage / 100);
            addSize = (int)Math.Round(newAddSize);
            if (addSize <= 0) addSize = 1;
            ballCountText.text = "+" + addSize;
        }
    }
}