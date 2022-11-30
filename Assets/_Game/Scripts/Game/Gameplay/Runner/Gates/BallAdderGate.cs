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
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float checkTime = 2f;
        private int addSize = 1;
        private BallManager ballManager;
        private WaitForSeconds wfsForCheckSize;

        private void OnEnable()
        {
            wfsForCheckSize = new WaitForSeconds(checkTime);
            ballManager = BallManager.Instance;
            StartCoroutine(CheckSize());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
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

        private IEnumerator CheckSize()
        {
            float newRemoveSize = 0;
            while (true)
            {
                newRemoveSize = ballManager.TotalBallCount * ((float)addPercentage / 100);
                addSize = (int)Math.Round(newRemoveSize);
                if (addSize <= 0) addSize = 1;
                text.text = "-" + addSize;
                yield return wfsForCheckSize;
            }
        }
    }
}